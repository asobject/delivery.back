using MediatR;

namespace Application.Features.OrderChanges.Queries.PointChange;

public record OrderPointChangeQuery(Guid Tracker) : IRequest<OrderPointChangeResponse>;