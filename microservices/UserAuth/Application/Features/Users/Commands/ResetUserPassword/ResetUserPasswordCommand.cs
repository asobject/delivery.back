

using MediatR;

namespace Application.Features.Users.Commands.ResetUserPassword;

public record ResetUserPasswordCommand(string Sub, string Token, string NewPassword) : IRequest<ResetUserPasswordResponse>;