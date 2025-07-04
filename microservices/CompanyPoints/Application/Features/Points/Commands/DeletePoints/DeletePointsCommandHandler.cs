
using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Points.Commands.DeletePoints;

public class DeletePointsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeletePointsCommand, DeletePointsResponse>
{
    public async Task<DeletePointsResponse> Handle(DeletePointsCommand request, CancellationToken cancellationToken)
    {
        var existingPoints = await unitOfWork.CompanyPoints.GetByIdsAsync(request.Ids);
        if (existingPoints.Count() != request.Ids.Count())
        {
            throw new ConflictException("any points not found");
        }
        await unitOfWork.CompanyPoints.DeleteRangeAsync(existingPoints);
        await unitOfWork.CompleteAsync();
        return new DeletePointsResponse
        {
            Message = "points deleted successful"
        };
    }
}
