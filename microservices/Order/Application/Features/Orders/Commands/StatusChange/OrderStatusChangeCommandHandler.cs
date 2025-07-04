using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.Orders.Commands.StatusChange;

public class OrderStatusChangeCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) :
    IRequestHandler<OrderStatusChangeCommand, OrderStatusChangeResponse>
{
    public async Task<OrderStatusChangeResponse> Handle(OrderStatusChangeCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetOrderByTrackerAsync(request.Tracker) ?? throw new NotFoundException($"order with tracker={request.Tracker} not found");
        order.OrderStatus = request.Status;
        await unitOfWork.Orders.UpdateOrderStatusAsync(order.Id, request.Status);
        await publishEndpoint.Publish(new OrderStatusChangeEvent(Tracker: order.Tracker, Status: request.Status), cancellationToken);
        return new OrderStatusChangeResponse(Message: "order status change successful");
    }
}