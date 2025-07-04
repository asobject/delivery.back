

using BuildingBlocks.Enums;
using BuildingBlocks.Interfaces.Repositories;
using BuildingBlocks.Models;
using Domain.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories;

public interface IOrderRepository:IRepository<Order>
{
    Task<Order?> GetOrderByTrackerAsync(Guid tracker);
    Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task UpdateCurrentPointAsync(int orderId, int pointId);
    Task<PagedResultDTO<Order>> GetOrdersPagedAsync(
      int pageNumber,
      int pageSize,
      Expression<Func<Order, bool>>? predicate = null);
    Task UpdateOrderReceiverIdByEmailAsync(string userId, string email);
}
