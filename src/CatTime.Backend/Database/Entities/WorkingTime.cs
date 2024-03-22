using CatTime.Shared;

namespace CatTime.Backend.Database.Entities;

public class WorkingTime : BaseEntity
{
    public Company Company { get; set; }
    public Employee Employee { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Start { get; set; }
    public TimeOnly? End { get; set; }

    public WorkingTimeType Type { get; set; }
}

