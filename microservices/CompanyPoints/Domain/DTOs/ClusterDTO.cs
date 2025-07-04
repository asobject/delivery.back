

using BuildingBlocks.Models;

namespace Domain.DTOs;

public class ClusterDTO
{
    public int Id { get; set; }
    public GeoPoint Coordinates { get; set; } = null!;
}
