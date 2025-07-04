using BuildingBlocks.Models;
using Domain.DTOs;

namespace Application.Features.Orders.Queries.GetOrdersPaged;

public record GetOrdersPagedResponse(PagedResultDTO<OrderDTO> Orders);