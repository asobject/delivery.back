

using BuildingBlocks.Enums;

namespace Domain.DTOs;

public record OrderStatusChangeDTO(OrderStatus Status, DateTime ChangeAt);