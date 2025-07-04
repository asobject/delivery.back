
using BuildingBlocks.Interfaces.Repositories;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICompanyPointRepository CompanyPoints { get; }
    IRepository<Contact> Contacts { get; }
    IRepository<Country> Countries { get; }
    IRepository<Locality> Localities { get; }
    IRepository<Province> Provinces { get; }
    IRepository<WorkingHours> WorkingHours { get; }
    Task<int> CompleteAsync();
    Task<ITransaction> BeginTransactionAsync();
}