using BuildingBlocks.Enums;
using BuildingBlocks.Interfaces.Entities;
using BuildingBlocks.Models;

namespace Domain.Entities;

public class CompanyPoint : IPoint, IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;


    public PointType PointType { get; set; }
    public string Address { get; set; } = null!;

    public GeoPoint Coordinates { get; set; } = null!; // Использует NetTopologySuite.Geometries.Point
    public PointStatus PointStatus { get; set; }

    public ICollection<WorkingHours> WorkingHours { get; set; } = [];

    public ICollection<Contact> Contacts { get; set; } = [];

    public int LocalityId { get; set; }
    public Locality Locality { get; set; } = null!;
}
