

using Domain.DTOs;

namespace Application.Features.Points.Queries.GetPointById;

public class GetPointByIdResponse
{
    public CompanyPointDTO Data { get; set; } = null!;
}
