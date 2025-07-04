using Domain.DTOs;

namespace Application.Features.OrderChanges.Queries.StatusChange;

public record OrderStatusChangeResponse(IEnumerable<OrderStatusChangeDTO> OrderStatusChanges);