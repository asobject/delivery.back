using Domain.DTOs;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.OrderChanges.Queries.StatusChange;

public class OrderStatusChangeQueryHandler(IUnitOfWork unitOfWork)
: IRequestHandler<OrderStatusChangeQuery, OrderStatusChangeResponse>
{
    public async Task<OrderStatusChangeResponse> Handle(OrderStatusChangeQuery request, CancellationToken cancellationToken)
    {
        var response = await unitOfWork.StatusChanges.GetStatusChangesAsync(request.Tracker);
        var dtos = response.Select(sc => new OrderStatusChangeDTO(
            Status: sc.Status,
            ChangeAt: sc.ChangeAt
            )).ToArray();
        return new OrderStatusChangeResponse(dtos);
    }
}