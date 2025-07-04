using BuildingBlocks.Interfaces.Entities;

namespace Domain.Entities;

public class Contact : IEntity
{
    public int Id { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int PointId { get; set; }
    public CompanyPoint Point { get; set; } = null!;
}