

using BuildingBlocks.Enums;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Points.Queries.GetClusters;

public class GetClustersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetClustersQuery, GetClustersResponse>
{
    public async Task<GetClustersResponse> Handle(GetClustersQuery request, CancellationToken cancellationToken)
    {
        var clusters = await unitOfWork.CompanyPoints.GetClustersAsync(PointStatus.Active);
        return new GetClustersResponse
        {
            Clusters = clusters
        };
    }
}
