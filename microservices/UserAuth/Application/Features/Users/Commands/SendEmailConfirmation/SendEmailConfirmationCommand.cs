

using MediatR;

namespace Application.Features.Users.Commands.SendEmailConfirmation;

public record SendEmailConfirmationCommand(string Email) : IRequest<SendEmailConfirmationResponse>;