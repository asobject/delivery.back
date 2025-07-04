using BuildingBlocks.Interfaces.Entities;

namespace Domain.Entities;

public class Province:IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
    public ICollection<Locality> Localities { get; set; } = [];
}
