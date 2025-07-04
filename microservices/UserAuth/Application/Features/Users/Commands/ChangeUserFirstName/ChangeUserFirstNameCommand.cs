
using MediatR;

namespace Application.Features.Users.Commands.ChangeUserFirstName;

public record ChangeUserFirstNameCommand(string UserId, string NewFirstName) : IRequest<ChangeUserFirstNameResponse>;