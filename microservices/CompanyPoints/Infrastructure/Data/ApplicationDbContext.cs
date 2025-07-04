

using Domain.Entities;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CompanyPoint> CompanyPoints { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<WorkingHours> WorkingHours { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Locality> Localities { get; set; } = null!;
    public DbSet<Province> Provinces { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyPoint>(entity =>
        {

            // Преобразование GeoPoint через конвертер
            entity.Property(p => p.Coordinates)
                .HasConversion<GeoPointConverter>()
              .HasColumnType("geography(Point, 4326)");
            // Добавление индекса на Coordinates
            entity.HasIndex(p => p.Coordinates)
                .HasMethod("GIST");  // Используем GIST для географического индекса
        });
    }
}
