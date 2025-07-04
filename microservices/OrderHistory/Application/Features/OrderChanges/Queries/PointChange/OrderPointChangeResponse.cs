using Domain.DTOs;

namespace Application.Features.OrderChanges.Queries.PointChange;

public record OrderPointChangeResponse(IEnumerable<OrderPointChangeDTO> OrderPointChanges);