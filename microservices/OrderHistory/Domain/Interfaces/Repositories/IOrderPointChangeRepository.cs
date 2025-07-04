

using BuildingBlocks.Interfaces.Repositories;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrderPointChangeRepository:IRepository<OrderPointChange>
{
    Task<IEnumerable<OrderPointChange>> GetPointChangesAsync(Guid tracker);
}
