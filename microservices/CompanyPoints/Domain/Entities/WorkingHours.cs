using BuildingBlocks.Interfaces.Entities;

namespace Domain.Entities;

public class WorkingHours : IEntity
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public int PointId { get; set; }
    public CompanyPoint Point { get; set; } = null!;
}