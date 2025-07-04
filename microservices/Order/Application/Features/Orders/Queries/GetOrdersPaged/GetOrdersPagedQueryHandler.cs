using Application.Mappings;
using BuildingBlocks.Enums;
using BuildingBlocks.Messaging.Requests;
using BuildingBlocks.Messaging.Responses;
using BuildingBlocks.Models;
using Domain.DTOs;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;

namespace Application.Features.Orders.Queries.GetOrdersPaged;

public class GetOrdersPagedQueryHandler(IUnitOfWork unitOfWork, IRequestClient<GetCompanyPointInfosRequest> client)
    : IRequestHandler<GetOrdersPagedQuery, GetOrdersPagedResponse>
{
    public async Task<GetOrdersPagedResponse> Handle(GetOrdersPagedQuery query, CancellationToken cancellationToken)
    {
        var orders = await unitOfWork.Orders.GetOrdersPagedAsync(query.PageNumber, query.PageSize,
          o => o.SenderId == query.UserId);
        var senderReceiverCompanyIds = orders.Data
     .SelectMany(o => new[]
     {
        (Point: o.SenderDeliveryPoint, Type: "sender"),
        (Point: o.ReceiverDeliveryPoint, Type: "receiver")
     })
     .Where(x => x.Point.Method == DeliveryMethod.PickupPoint)
     .Select(x => x.Point.CompanyPointId)
     .Where(id => id.HasValue)
     .Select(id => id!.Value);

        var currentCompanyIds = orders.Data
            .Select(o => o.CurrentPointId)
            .Where(id => id.HasValue)
            .Select(id => id!.Value);

        var companyPointIds = senderReceiverCompanyIds
            .Concat(currentCompanyIds)
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
        var dtos = orders.Data
          .Select(o => OrderMapper.MapOrder(o, addressLookup!))
          .ToList();

        return new GetOrdersPagedResponse(Orders: new PagedResultDTO<OrderDTO>
        {
            Data = dtos,
            TotalRecords = orders.TotalRecords
        });
    }
}
