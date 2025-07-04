
using MediatR;

namespace Application.Features.Points.Queries.GetPagedPoints;

public class GetPagedPointsQuery : IRequest<GetPagedPointsResponse>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
