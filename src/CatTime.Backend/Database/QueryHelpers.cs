using CatTime.Backend.Database.Entities;

namespace CatTime.Backend.Database;

public static class QueryHelpers
{
    public static IQueryable<WorkingTime> WhereIsRecentWorkingTime(this IQueryable<WorkingTime> query)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var now = TimeOnly.FromDateTime(DateTime.Now);
        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        var isInMorning = DateTime.Now.Hour < 5;
        
        // Either:
        // * the most recent time from today, or
        // * the last one from yesterday if we it's before 5am right now
        return query.Where(f => (f.Date == today && f.Start < now) || 
                                (f.Date == yesterday && isInMorning));
    }
}