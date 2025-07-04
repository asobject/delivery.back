
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.OrderChanges.Commands.StatusChange;

public class OrderStatusChangeCommandHandler(IUnitOfWork unitOfWork) :
    IRequestHandler<OrderStatusChangeCommand, OrderStatusChangeResponse>
{
    public async Task<OrderStatusChangeResponse> Handle(OrderStatusChangeCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.StatusChanges.AddAsync(
           new Domain.Entities.OrderStatusChange
           {
               Tracker = request.Tracker,
               Status = request.Status
           });
        await unitOfWork.CompleteAsync();
        return new OrderStatusChangeResponse(Message: "order status change successful");
    }
}