

using BuildingBlocks.Enums;
using BuildingBlocks.Interfaces.Entities;

namespace Domain.Entities;

public class OrderStatusChange:IEntity
{
    public int Id { get; set; }
    public Guid Tracker { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime ChangeAt { get; set; } = DateTime.UtcNow;
}
