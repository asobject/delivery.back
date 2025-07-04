

using Application.Features.OrderChanges.Commands.StatusChange;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;

namespace Infrastructure.Consumers;

public class OrderStatusChangeConsumer(IMediator mediator)
    : IConsumer<OrderStatusChangeEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangeEvent> context)
    {
        var status = new OrderStatusChangeCommand(context.Message.Tracker, context.Message.Status);
        _ = await mediator.Send(status);
    }
}
