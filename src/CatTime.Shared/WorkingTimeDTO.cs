namespace CatTime.Shared;

public class WorkingTimeDTO : BaseEntity
{
    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Start { get; set; }
    public TimeOnly? End { get; set; }

    public WorkingTimeType Type { get; set; }
}

