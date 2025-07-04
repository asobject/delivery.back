

using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Points.Commands.UpdatePoint;

public class UpdatePointCommand:IRequest<UpdatePointResponse>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? PointStatus { get; set; }
    public int? PointType { get; set; }
}
