using BuildingBlocks.Messaging.Requests;
using BuildingBlocks.Messaging.Responses;
using Domain.DTOs;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.OrderChanges.Queries.PointChange;

public class OrderPointChangeQueryHandler(IUnitOfWork unitOfWork, IRequestClient<GetCompanyPointInfosRequest> client)
: IRequestHandler<OrderPointChangeQuery, OrderPointChangeResponse>
{
    public async Task<OrderPointChangeResponse> Handle(OrderPointChangeQuery request, CancellationToken cancellationToken)
    {
        var pointChanges = await unitOfWork.PointChanges.GetPointChangesAsync(request.Tracker);
        var pointIds = pointChanges.Select(pc => pc.PointId).ToArray();

        var pointInfoResponse = await client.GetResponse<GetCompanyPointInfosResponse>(
            new { RequestId = NewId.NextGuid(), CompanyPointIds = pointIds });

        var addressDictionary = pointInfoResponse.Message.Points
       .GroupBy(p => p.CompanyPointId)
       .ToDictionary(
           g => g.Key,
           g => g.First().Address ?? "not found");

        var orderPointChangeDtos = pointChanges
            .Select(pc => new OrderPointChangeDTO(
                Address: addressDictionary.TryGetValue(pc.PointId, out var address)
                    ? address
                    : "Адрес не найден",
                ChangeAt: pc.ChangeAt
            ))
            .ToArray();
        return new OrderPointChangeResponse(orderPointChangeDtos);
    }
}
