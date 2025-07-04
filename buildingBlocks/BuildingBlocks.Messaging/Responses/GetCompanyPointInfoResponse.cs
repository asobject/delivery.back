
using BuildingBlocks.Models;

namespace BuildingBlocks.Messaging.Responses;

public record GetCompanyPointInfosResponse
{
    public Guid RequestId { get; init; }
    public IEnumerable<CompanyPointInfoResponse> Points { get; init; } = [];
}
public record CompanyPointInfoResponse
{
    public int CompanyPointId { get; init; }
    public string? Address { get; init; }
    public GeoPoint? Coordinates { get; init; }
}