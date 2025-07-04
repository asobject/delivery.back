


using BuildingBlocks.Enums;
using BuildingBlocks.Models;
using MediatR;

namespace Application.Features.Points.Commands.CreatePoint;

public class CreatePointCommand : IRequest<CreatePointResponse>
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public GeoPoint Coordinates { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string Locality { get; set; } = null!;
    public PointType PointType { get; set; }
    public PointStatus PointStatus { get; set; }
}
