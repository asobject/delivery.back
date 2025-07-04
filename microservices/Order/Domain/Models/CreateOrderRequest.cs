

using BuildingBlocks.Enums;
using BuildingBlocks.Models;

namespace Domain.Models;

public class CreateOrderRequest
{
    public string ReceiverEmail { get; set; } = null!;
    public double Price { get; set; }
    public PaymentMethod PaymentType { get; set; }
    public PackageSize PackageSize { get; set; }
    public DeliveryMethod SenderDeliveryMethod { get; set; }
    public string? SenderAddress { get; set; }
    public GeoPoint? SenderCoordinates { get; set; }
    public int? SenderCompanyPointId { get; set; }
    public DeliveryMethod ReceiverDeliveryMethod { get; set; }
    public string? ReceiverAddress { get; set; } 
    public GeoPoint? ReceiverCoordinates { get; set; }
    public int? ReceiverCompanyPointId { get; set; }
}
