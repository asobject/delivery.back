

using Application.Features.OrderChanges.Commands.PointChange;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;

namespace Infrastructure.Consumers;

public class OrderPointChangeConsumer(IMediator mediator)
    : IConsumer<OrderPointChangeEvent>
{
    public async Task Consume(ConsumeContext<OrderPointChangeEvent> context)
    {
        var point = new OrderPointChangeCommand(context.Message.Tracker, context.Message.PointId);
        _ = await mediator.Send(point);
    }
}