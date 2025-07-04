

using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderStatusChangeRepository(ApplicationDbContext context) : Repository<OrderStatusChange>(context), IOrderStatusChangeRepository
{
    public async Task<IEnumerable<OrderStatusChange>> GetStatusChangesAsync(Guid tracker)
    {
        return await _dbSet.AsNoTracking().Where(o => o.Tracker == tracker).ToListAsync();
    }
}
