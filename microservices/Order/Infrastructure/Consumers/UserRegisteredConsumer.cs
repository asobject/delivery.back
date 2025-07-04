
using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using MassTransit;

namespace Infrastructure.Consumers;

public class UserRegisteredConsumer(
    IUnitOfWork unitOfWork)
    : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        await unitOfWork.Orders.UpdateOrderReceiverIdByEmailAsync(context.Message.UserId, context.Message.Email);
        await unitOfWork.CompleteAsync();
    }
}
