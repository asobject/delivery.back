

using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.ChangeUserLastName;

public class ChangeUserLastNameCommandHandler( IUserRepository userRepository)
    : IRequestHandler<ChangeUserLastNameCommand, ChangeUserLastNameResponse>
{
    public async Task<ChangeUserLastNameResponse> Handle
        (ChangeUserLastNameCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId) ??
                   throw new NotFoundException($"user with id={request.UserId} not found");
        user.LastName = request.NewLastName;
        await userRepository.UpdateAsync(user);
        return new ChangeUserLastNameResponse("user lastName changed successful");
    }
}
