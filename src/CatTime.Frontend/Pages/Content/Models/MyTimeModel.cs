using CatTime.Shared;

namespace CatTime.Frontend.Pages.Content.Models;

public class MyTimeModel
{
    public DateTime? Date { get; set; }
    public TimeOnly? Start { get; set; }
    public TimeOnly? End { get; set; }
    public WorkingTimeType Type { get; set; }
    public string TotalWorkingTime
    {
        get
        {
            if (this.Start.HasValue && this.End.HasValue)
            {
                var difference = this.End.Value - this.Start.Value;
                return difference.ToString(@"hh\:mm");
            }
            
            return "00:00";
        }
    }
}