

using MediatR;

namespace Application.Features.Users.Commands.ChangeUserLastName;

public record ChangeUserLastNameCommand(string UserId, string NewLastName) : IRequest<ChangeUserLastNameResponse>;
