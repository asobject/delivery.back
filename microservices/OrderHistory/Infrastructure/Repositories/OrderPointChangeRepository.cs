

using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderPointChangeRepository(ApplicationDbContext context) : Repository<OrderPointChange>(context), IOrderPointChangeRepository
{
    public async Task<IEnumerable<OrderPointChange>> GetPointChangesAsync(Guid tracker)
    {
       return await _dbSet.AsNoTracking().Where(o => o.Tracker == tracker).ToListAsync();
    }
}
