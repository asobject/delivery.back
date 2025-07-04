using MediatR;

namespace Application.Features.OrderChanges.Commands.PointChange;

public record OrderPointChangeCommand(Guid Tracker, int PointId) : IRequest<OrderPointChangeResponse>;
