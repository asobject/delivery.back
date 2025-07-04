using Domain.DTOs;

namespace Application.Features.Points.Queries.GetClusters;

public class GetClustersResponse
{
    public IEnumerable<ClusterDTO> Clusters { get; set; } = null!;
}
