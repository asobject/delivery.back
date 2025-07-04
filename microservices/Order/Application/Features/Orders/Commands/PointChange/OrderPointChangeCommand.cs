using MediatR;

namespace Application.Features.Orders.Commands.PointChange;

public record OrderPointChangeCommand(Guid Tracker, int PointId) : IRequest<OrderPointChangeResponse>;
