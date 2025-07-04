

using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using MassTransit;
using MediatR;

namespace Application.Features.Users.Commands.EmailConfirmation;

public class EmailConfirmationCommandHandler(IUserRepository userRepository, IPublishEndpoint publishEndpoint)
    : IRequestHandler<EmailConfirmationCommand, EmailConfirmationResponse>
{
    public async Task<EmailConfirmationResponse> Handle(EmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Sub);
        if (user is null)
            return new EmailConfirmationResponse("email confirmed successful");
        var result = await userRepository.ConfirmEmailAsync(user, request.Token);
        if (!result)
        {
            throw new ConflictException("invalid request");
        }
        await publishEndpoint.Publish(new EmailSendingEvent(
          To: user.Email!,
          Subject: "Подтвеждение почты",
          Body: "Почта подтвеждена успешно"
          ), cancellationToken);
        return new EmailConfirmationResponse("email confirmed successful");
    }
}
