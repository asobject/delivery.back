

using MediatR;

namespace Application.Features.Users.Commands.EmailConfirmation;

public record EmailConfirmationCommand(string Sub, string Token) : IRequest<EmailConfirmationResponse>;