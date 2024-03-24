namespace CatTime.Shared.Routes.WorkingTimes;

public class CurrentTimeDTO
{
    // The amount of work-time the user already had today (until LastCheckinTime)
    public TimeSpan WorkTime { get; set; }
    
    // Not null, if the user is currently checked-in
    public DateTime? LastCheckinTime { get; set; }
    public WorkingTimeType? LastCheckinType { get; set; }
}