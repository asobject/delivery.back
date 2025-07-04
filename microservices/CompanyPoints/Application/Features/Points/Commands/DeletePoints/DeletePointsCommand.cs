

using MediatR;

namespace Application.Features.Points.Commands.DeletePoints;

public class DeletePointsCommand:IRequest<DeletePointsResponse>
{
    public IEnumerable<int> Ids { get; set; } = null!;
}
