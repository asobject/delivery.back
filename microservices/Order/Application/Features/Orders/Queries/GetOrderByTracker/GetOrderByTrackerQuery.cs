

using MediatR;

namespace Application.Features.Orders.Queries.GetOrderByTracker;

public record GetOrderByTrackerQuery(Guid Tracker) : IRequest<GetOrderByTrackerResponse>;