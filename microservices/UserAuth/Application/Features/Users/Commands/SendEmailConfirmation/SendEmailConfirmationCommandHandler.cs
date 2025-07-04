using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using MassTransit;
using MediatR;
using System.Net;

namespace Application.Features.Users.Commands.SendEmailConfirmation;

public class SendEmailConfirmationCommandHandler(
    IUserRepository userRepository,
    IUserService userService,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<SendEmailConfirmationCommand, SendEmailConfirmationResponse>
{
    public async Task<SendEmailConfirmationResponse> Handle(
        SendEmailConfirmationCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null || user.EmailConfirmed)
            return new SendEmailConfirmationResponse("confirmation email sent successful");

        var token = await userService.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebUtility.UrlEncode(token);

        await publishEndpoint.Publish(new EmailSendingEvent(
            To: user.Email!,
            Subject: "Подтверждение почты",
            Body: $"Подтвердите вашу почту <a href='https://localhost:4200/confirm-email?token={encodedToken}&sub={user.Id}'>нажать здесь</a>"
        ), cancellationToken);

        return new SendEmailConfirmationResponse("confirmation email sent successful");
    }
}