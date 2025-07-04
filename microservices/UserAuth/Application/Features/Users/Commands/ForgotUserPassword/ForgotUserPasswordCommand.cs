

using MediatR;

namespace Application.Features.Users.Commands.ForgotUserPassword;

public record ForgotUserPasswordCommand(string Email) : IRequest<ForgotUserPasswordResponse>;
