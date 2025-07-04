

using BuildingBlocks.Enums;
using BuildingBlocks.Models;
using Domain.Models;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand(CreateOrderRequest request, string senderId,string senderEmail) : IRequest<CreateOrderResponse>
{
    public string SenderId { get; set; } = senderId;
    public string SenderEmail { get; set; } = senderEmail;
    public string ReceiverEmail { get; set; } = request.ReceiverEmail;
    public double Price { get; set; } = request.Price;
    public PaymentMethod PaymentType { get; set; } = request.PaymentType;
    public PackageSize PackageSize { get; set; } = request.PackageSize;
    public DeliveryMethod SenderDeliveryMethod { get; set; } = request.SenderDeliveryMethod;
    public string? SenderAddress { get; set; } = request.SenderAddress;
    public GeoPoint? SenderCoordinates { get; set; } = request.SenderCoordinates;
    public int? SenderCompanyPointId { get; set; } = request.SenderCompanyPointId;
    public DeliveryMethod ReceiverDeliveryMethod { get; set; } = request.ReceiverDeliveryMethod;
    public string? ReceiverAddress { get; set; } = request.ReceiverAddress;
    public GeoPoint? ReceiverCoordinates { get; set; } = request.ReceiverCoordinates;
    public int? ReceiverCompanyPointId { get; set; } = request.ReceiverCompanyPointId;
}
