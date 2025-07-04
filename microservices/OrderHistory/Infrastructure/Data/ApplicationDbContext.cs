

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<OrderStatusChange> StatusChanges { get; set; } = null!;
    public DbSet<OrderPointChange> PointChanges { get; set; } = null!;
}
