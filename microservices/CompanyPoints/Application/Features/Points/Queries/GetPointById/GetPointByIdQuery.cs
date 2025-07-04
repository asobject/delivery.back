

using MediatR;

namespace Application.Features.Points.Queries.GetPointById;

public class GetPointByIdQuery : IRequest<GetPointByIdResponse>
{
    public int Id { get; set; }
}
