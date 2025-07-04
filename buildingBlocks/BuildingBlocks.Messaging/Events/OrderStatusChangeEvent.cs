

using BuildingBlocks.Enums;

namespace BuildingBlocks.Messaging.Events;

public record OrderStatusChangeEvent(Guid Tracker, OrderStatus Status);