using BuildingBlocks.Enums;
using BuildingBlocks.Interfaces.Repositories;
using BuildingBlocks.Models;
using Domain.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories;

public interface ICompanyPointRepository : IRepository<CompanyPoint>
{
    Task<string?> GetExistingPointName(CompanyPoint point, int? id = null);
    Task<bool> CompanyPointsExistInRadiusAsync(GeoPoint center, double radiusKm);
    Task<IEnumerable<ClusterDTO>> GetClustersAsync(params PointStatus[] statuses);
    Task<PagedResultDTO<CompanyPoint>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    Expression<Func<CompanyPoint, bool>>? predicate = null);
}