using BuildingBlocks.Enums;
using MediatR;

namespace Application.Features.Orders.Commands.StatusChange;

public record OrderStatusChangeCommand(Guid Tracker, OrderStatus Status) : IRequest<OrderStatusChangeResponse>;
