using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Points.Queries.GetCheckPointExistInRadius;

public class GetCheckPointExistInRadiusQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCheckPointExistInRadiusQuery, GetCheckPointExistInRadiusResponse>
{
    public async Task<GetCheckPointExistInRadiusResponse> Handle(GetCheckPointExistInRadiusQuery request, CancellationToken cancellationToken)
    {
        return new GetCheckPointExistInRadiusResponse(await unitOfWork.CompanyPoints.CompanyPointsExistInRadiusAsync(request.Coordinates, 200));
    }
}
