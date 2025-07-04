using BuildingBlocks.Interfaces.Entities;

namespace Domain.Entities;

public class Country:IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Province> Provinces { get; set; } = [];
}