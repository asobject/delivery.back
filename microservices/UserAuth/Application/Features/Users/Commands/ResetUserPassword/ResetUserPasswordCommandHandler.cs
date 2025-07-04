

using Application.Features.Users.Commands.ForgotUserPassword;
using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using MassTransit;
using MediatR;

namespace Application.Features.Users.Commands.ResetUserPassword;

public class ResetUserPasswordCommandHandler(IUserRepository userRepository, IRefreshTokenStore refreshTokenStore, IPublishEndpoint publishEndpoint)
    : IRequestHandler<ResetUserPasswordCommand, ResetUserPasswordResponse>
{
    public async Task<ResetUserPasswordResponse> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Sub);
        if (user is null)
            return new ResetUserPasswordResponse("password reseted successful");
        var result = await userRepository.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result)
        {
            await publishEndpoint.Publish(new EmailSendingEvent(
         To: user.Email!,
         Subject: "Сброс пароля",
         Body: "Кто-то пытался сбросить ваш пароль"
         ), cancellationToken);
            throw new ConflictException("invalid request");
        }
        await refreshTokenStore.RevokeAllTokensAsync(user.Id);
        await publishEndpoint.Publish(new EmailSendingEvent(
           To: user.Email!,
           Subject: "Сброс пароля",
           Body: "Пароль сброшен изменен"
           ), cancellationToken);
        return new ResetUserPasswordResponse("password reseted successful");
    }
}
