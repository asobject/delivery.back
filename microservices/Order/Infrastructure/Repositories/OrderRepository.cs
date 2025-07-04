
using BuildingBlocks.Enums;
using BuildingBlocks.Models;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetOrderByTrackerAsync(Guid tracker)
    {
        return await _dbSet.AsNoTracking().Include(o => o.SenderDeliveryPoint)
                .ThenInclude(dp => dp.CustomPoint)
            .Include(o => o.ReceiverDeliveryPoint)
                .ThenInclude(dp => dp.CustomPoint)
            .FirstOrDefaultAsync(o => o.Tracker == tracker);
    }

    public async Task<PagedResultDTO<Order>> GetOrdersPagedAsync(
     int pageNumber,
     int pageSize,
     Expression<Func<Order, bool>>? predicate = null)
    {
        var baseQuery = _dbSet.AsQueryable();

        if (predicate != null)
        {
            baseQuery = baseQuery.Where(predicate);
        }

        baseQuery = baseQuery
            .Include(o => o.SenderDeliveryPoint)
                .ThenInclude(dp => dp.CustomPoint)
            .Include(o => o.ReceiverDeliveryPoint)
                .ThenInclude(dp => dp.CustomPoint);

        int totalRecords = await baseQuery.CountAsync();

        var data = await baseQuery
            .OrderByDescending(o => o.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PagedResultDTO<Order>
        {
            Data = data,
            TotalRecords = totalRecords
        };
    }

    public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
     => await _dbSet
         .Where(o => o.Id == orderId)
         .ExecuteUpdateAsync(s => s
             .SetProperty(o => o.OrderStatus, status));
    public async Task UpdateCurrentPointAsync(int orderId, int pointId) => await _dbSet
         .Where(o => o.Id == orderId)
         .ExecuteUpdateAsync(s => s
             .SetProperty(o => o.CurrentPointId, pointId));
    public async Task UpdateOrderReceiverIdByEmailAsync(string userId, string email)
    => await _dbSet
        .Where(o => o.ReceiverEmail == email)
        .ExecuteUpdateAsync(s => s
            .SetProperty(o => o.ReceiverId, userId)
            .SetProperty(o => o.ReceiverEmail,  (string?)null)
        );

}