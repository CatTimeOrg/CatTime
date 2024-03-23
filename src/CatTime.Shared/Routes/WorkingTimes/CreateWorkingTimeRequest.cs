namespace CatTime.Shared.Routes.WorkingTimes;

public class CreateWorkingTimeRequest
{
    public int? EmployeeId { get; set; }
    
    public DateOnly Date { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly? End { get; set; }
    
    public WorkingTimeType Type { get; set; }
}