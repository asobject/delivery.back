

using MediatR;

namespace Application.Features.Users.Commands.ChangeUserPassword;

public record ChangeUserPasswordCommand(string UserId, string CurrentPassword, string NewPassword) : IRequest<ChangeUserPasswordResponse>;

