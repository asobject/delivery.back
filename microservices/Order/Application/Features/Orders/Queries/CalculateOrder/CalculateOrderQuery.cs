using BuildingBlocks.Enums;
using BuildingBlocks.Models;
using MediatR;

namespace Application.Features.Orders.Queries.CalculateOrder;

public class CalculateOrderQuery : IRequest<CalculateOrderResponse>
{
    public GeoPoint SenderPointCoordinates { get; set; } = null!;
    public GeoPoint ReceiverPointCoordinates { get; set; } = null!;
    public DeliveryMethod SenderDeliveryMethod { get; set; }
    public DeliveryMethod ReceiverDeliveryMethod { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PackageSize PackageSize { get; set; }
}
