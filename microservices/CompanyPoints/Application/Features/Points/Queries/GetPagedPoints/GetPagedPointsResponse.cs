

using Domain.DTOs;

namespace Application.Features.Points.Queries.GetPagedPoints;

public class GetPagedPointsResponse
{
    public int TotalRecords { get; set; }
    public IEnumerable<CompanyPointDTO> Data { get; set; } = null!;
}
