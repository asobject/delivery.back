
using BuildingBlocks.Interfaces.Repositories;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IOrderPointChangeRepository PointChanges { get; }
    IOrderStatusChangeRepository StatusChanges { get; }
    Task<int> CompleteAsync();
    Task<ITransaction> BeginTransactionAsync();
}