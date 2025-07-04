using BuildingBlocks.Interfaces.Entities;

namespace Domain.Entities;

public class Locality : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
    public ICollection<CompanyPoint> Points { get; set; } = [];
}