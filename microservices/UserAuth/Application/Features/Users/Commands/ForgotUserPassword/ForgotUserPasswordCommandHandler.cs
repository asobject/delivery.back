

using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using MassTransit;
using MediatR;
using System.Net;

namespace Application.Features.Users.Commands.ForgotUserPassword;

public class ForgotUserPasswordCommandHandler(IUserRepository userRepository, IUserService userService, IPublishEndpoint publishEndpoint)
    : IRequestHandler<ForgotUserPasswordCommand, ForgotUserPasswordResponse>
{
    public async Task<ForgotUserPasswordResponse> Handle(ForgotUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return new ForgotUserPasswordResponse(Message: "email sent successful");
        var token = await userService.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebUtility.UrlEncode(token);
        await publishEndpoint.Publish(new EmailSendingEvent(
            To: user.Email!,
            Subject: "Восстановление пароля",
            Body: $"Восстановите ваш пароль <a href='https://localhost:4200/reset-password?token={encodedToken}&sub={user.Id}'>нажать здесь</a>"
            ), cancellationToken);
        return new ForgotUserPasswordResponse(Message: "email sent successful");
    }
}
