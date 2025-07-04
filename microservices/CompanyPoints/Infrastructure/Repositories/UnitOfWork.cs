
using Infrastructure.Data;
using BuildingBlocks.Interfaces.Repositories;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public ICompanyPointRepository CompanyPoints => new CompanyPointRepository(context);

    public IRepository<Contact> Contacts => new Repository<Contact>(context);

    public IRepository<Country> Countries => new Repository<Country>(context);

    public IRepository<Locality> Localities => new Repository<Locality>(context);

    public IRepository<Province> Provinces => new Repository<Province>(context);

    public IRepository<WorkingHours> WorkingHours => new Repository<WorkingHours>(context);

    public async Task<int> CompleteAsync() => await context.SaveChangesAsync();
    public async Task<ITransaction> BeginTransactionAsync()
    {
        var dbTransaction = await context.Database.BeginTransactionAsync();
        return new EFCoreTransaction(dbTransaction);
    }
    public void Dispose() => context.Dispose();
}
