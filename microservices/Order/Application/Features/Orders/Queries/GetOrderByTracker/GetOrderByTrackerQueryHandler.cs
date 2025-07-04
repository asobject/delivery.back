

using Application.Mappings;
using BuildingBlocks.Enums;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Requests;
using BuildingBlocks.Messaging.Responses;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.Orders.Queries.GetOrderByTracker;

public class GetOrderByTrackerQueryHandler(IUnitOfWork unitOfWork, IRequestClient<GetCompanyPointInfosRequest> client)
    : IRequestHandler<GetOrderByTrackerQuery, GetOrderByTrackerResponse>
{
    public async Task<GetOrderByTrackerResponse> Handle(GetOrderByTrackerQuery query, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetOrderByTrackerAsync(query.Tracker) ?? throw new NotFoundException($"order with tracker={query.Tracker} not found");
        var senderReceiverCompanyIds = new[]
      {
        (Point: order.SenderDeliveryPoint, Type: "sender"),
        (Point: order.ReceiverDeliveryPoint, Type: "receiver")
    }
      .Where(x => x.Point.Method == DeliveryMethod.PickupPoint)
      .Select(x => x.Point.CompanyPointId)
      .Where(id => id.HasValue)
      .Select(id => id!.Value);

        var currentCompanyId = order.CurrentPointId.HasValue
            ? [order.CurrentPointId.Value]
            : Enumerable.Empty<int>();

        var companyPointIds = senderReceiverCompanyIds
            .Concat(currentCompanyId)
            .Distinct()
            .ToArray();
        var addressLookup = new Dictionary<int, string>();

        if (companyPointIds.Length != 0)
        {
            var request = new GetCompanyPointInfosRequest
            {
                RequestId = NewId.NextGuid(),
                CompanyPointIds = companyPointIds
            };

            var response = await client.GetResponse<GetCompanyPointInfosResponse>(request);
            addressLookup = response.Message.Points
                .ToDictionary(p => p.CompanyPointId, p => p.Address ?? "");
        }

        var dto = OrderMapper.MapOrder(order, addressLookup);
        return new GetOrderByTrackerResponse(dto);
    }
}
