namespace CatTime.Shared.Routes.WorkingTimes;

public class UpdateWorkingTimeRequest
{
    public DateOnly Date { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly? End { get; set; }
    
    public WorkingTimeType Type { get; set; }
}