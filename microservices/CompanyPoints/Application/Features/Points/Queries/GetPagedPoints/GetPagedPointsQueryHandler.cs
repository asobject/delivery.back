

using Application.Mappings;
using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;
namespace Application.Features.Points.Queries.GetPagedPoints;

public class GetPagedPointsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPagedPointsQuery, GetPagedPointsResponse>
{
    public async Task<GetPagedPointsResponse> Handle(GetPagedPointsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber <= 0 || request.PageSize <= 0)
            throw new ConflictException("pageNumber or pageSize <= 0");
        var points = await unitOfWork.CompanyPoints.GetPagedAsync(
             request.PageNumber,
             request.PageSize, predicate: null);
        return new GetPagedPointsResponse
        {
            TotalRecords = points.TotalRecords,
            Data = points.Data.ToDTOs()
        };
    }
}
