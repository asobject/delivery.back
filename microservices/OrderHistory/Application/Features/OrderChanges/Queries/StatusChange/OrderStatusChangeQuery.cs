using MediatR;

namespace Application.Features.OrderChanges.Queries.StatusChange;

public record OrderStatusChangeQuery(Guid Tracker) : IRequest<OrderStatusChangeResponse>;