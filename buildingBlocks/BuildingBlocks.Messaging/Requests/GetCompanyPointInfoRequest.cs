

namespace BuildingBlocks.Messaging.Requests;

public record GetCompanyPointInfosRequest
{
    public Guid RequestId { get; init; }
    public IEnumerable<int> CompanyPointIds { get; init; } = [];
}