

using BuildingBlocks.Enums;
using BuildingBlocks.Models;
using Domain.Entities;

namespace Domain.DTOs;

public  class CompanyPointDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;


    public PointType PointType { get; set; }
    public string Address { get; set; } = null!;

    public GeoPoint Coordinates { get; set; } = null!; // Использует NetTopologySuite.Geometries.Point
    public PointStatus PointStatus { get; set; }

    //public ICollection<WorkingHours> WorkingHours { get; set; } = [];

    //public ICollection<Contact> Contacts { get; set; } = [];
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? Locality { get; set; }
}
