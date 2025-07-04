using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.Orders.Commands.PointChange;

public class OrderPointChangeCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
: IRequestHandler<OrderPointChangeCommand, OrderPointChangeResponse>
{
    public async Task<OrderPointChangeResponse> Handle(OrderPointChangeCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetOrderByTrackerAsync(request.Tracker) ?? throw new NotFoundException($"order with tracker={request.Tracker} not found");
        order.CurrentPointId = request.PointId;
        await unitOfWork.Orders.UpdateCurrentPointAsync(order.Id,request.PointId);
        await publishEndpoint.Publish(new OrderPointChangeEvent(Tracker: order.Tracker, PointId: request.PointId), cancellationToken);
        return new OrderPointChangeResponse(Message: "order current point change successful");
    }
}
