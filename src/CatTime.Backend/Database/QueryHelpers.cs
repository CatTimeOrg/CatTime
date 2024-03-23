using CatTime.Backend.Database.Entities;

namespace CatTime.Backend.Database;

public static class QueryHelpers
{
    public static IQueryable<WorkingTime> WhereIsRecentWorkingTime(this IQueryable<WorkingTime> query)
    {
        // Either:
        // * the most recent time from today, or
        // * the last one from yesterday if we it's before 5am right now
        return query.Where(f => (f.Date == DateOnly.FromDateTime(DateTime.Today) && f.Start < TimeOnly.FromDateTime(DateTime.Now)) || 
                                (f.Date == DateOnly.FromDateTime(DateTime.Today.AddDays(-1)) && DateTime.Now.Hour < 5));
    }
}