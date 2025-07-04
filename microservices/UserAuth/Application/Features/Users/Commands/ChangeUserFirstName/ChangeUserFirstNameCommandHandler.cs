

using Application.Features.Users.Commands.ChangeUserPassword;
using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.Users.Commands.ChangeUserFirstName;

public class ChangeUserFirstNameCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<ChangeUserFirstNameCommand, ChangeUserFirstNameResponse>
{
    public async Task<ChangeUserFirstNameResponse> Handle(ChangeUserFirstNameCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId) ?? throw new NotFoundException($"user with id={request.UserId} not found");
        user.FirstName = request.NewFirstName;
        await userRepository.UpdateAsync(user);
        return new ChangeUserFirstNameResponse("user firstName changed succsessful");
    }
}
