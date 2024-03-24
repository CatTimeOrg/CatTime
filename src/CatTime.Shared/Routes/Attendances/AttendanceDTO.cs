namespace CatTime.Shared.Routes.Attendances;

public class AttendanceDTO
{
    public bool IsPresent { get; set; }
    public WorkingTimeType? LastType { get; set; }
    public EmployeeDTO Employee { get; set; }
}