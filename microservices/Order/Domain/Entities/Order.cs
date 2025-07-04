

using BuildingBlocks.Enums;
using BuildingBlocks.Interfaces.Entities;

namespace Domain.Entities;

public class Order : IEntity
{
    public int Id { get; set; }
    public Guid Tracker { get; set; } = Guid.NewGuid();
    public double Price { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Created;
    public int? CurrentPointId { get; set; }
    public PackageSize PackageSize { get; set; }

    // Отправитель (обязательно зарегистрирован)
    public string SenderId { get; set; } = null!;

    // Получатель (может быть незарегистрированным)
    public string? ReceiverId { get; set; }
    public string? ReceiverEmail { get; set; }

    public int SenderDeliveryPointId { get; set; }
    public DeliveryPoint SenderDeliveryPoint { get; set; } = null!;
    public int ReceiverDeliveryPointId { get; set; }
    public DeliveryPoint ReceiverDeliveryPoint { get; set; } = null!;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveryDate { get; set; } = null;
}