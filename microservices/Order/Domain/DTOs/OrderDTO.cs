

using BuildingBlocks.Enums;

namespace Domain.DTOs;

public record OrderDTO(Guid Tracker,string? CurrentPointAddress,string SenderAddress,string ReceiverAddress,OrderStatus Status);