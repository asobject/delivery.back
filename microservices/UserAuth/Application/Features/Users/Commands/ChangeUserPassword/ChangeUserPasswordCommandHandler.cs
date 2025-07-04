

using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.Users.Commands.ChangeUserPassword;

public class ChangeUserPasswordCommandHandler(
    IUserRepository userRepository, IRefreshTokenStore refreshTokenStore, IPublishEndpoint publishEndpoint)
    : IRequestHandler<ChangeUserPasswordCommand, ChangeUserPasswordResponse>
{
    public async Task<ChangeUserPasswordResponse> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return new ChangeUserPasswordResponse(Message: "password changed successful");
        var result = await userRepository.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result)
        {
            await publishEndpoint.Publish(new EmailSendingEvent(
           To: user.Email!,
           Subject: "Изменение пароля",
           Body: "Кто-то пытался измененить ваш пароль"
           ), cancellationToken);
            throw new ConflictException("invalid request");
        }
        await refreshTokenStore.RevokeAllTokensAsync(user.Id);
        await publishEndpoint.Publish(new EmailSendingEvent(
           To: user.Email!,
           Subject: "Изменение пароля",
           Body: "Пароль успешно изменен"
           ), cancellationToken);
        return new ChangeUserPasswordResponse(Message: "password changed successful");
    }
}
