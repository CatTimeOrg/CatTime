using CatTime.Backend.Database;
using CatTime.Backend.Extensions;
using CatTime.Shared;
using CatTime.Shared.Routes.Attendances;
using Microsoft.EntityFrameworkCore;

namespace CatTime.Backend.Routes;

public static class AttendanceRoutes
{
    public static void MapAttendance(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/attendance");
        group.RequireAuthorization();
        
        group.MapGet("/", async (CatContext catContext, HttpContext httpContext) =>
        {
            var allEmployees = await catContext.Employees.ToListAsync();
            var allEmployeeIds = allEmployees.Select(f => f.Id).ToList();

            var times = await catContext.WorkingTimes
                .WhereIsRecentWorkingTime()
                .Where(f => allEmployeeIds.Contains(f.EmployeeId))
                .OrderByDescending(f => f.Date)
                .ThenByDescending(f => f.Start)
                .ToListAsync();

            var result = new List<AttendanceDTO>();

            foreach (var employee in allEmployees)
            {
                var timeForEmployee = times
                    .Where(f => f.EmployeeId == employee.Id)
                    .FirstOrDefault();

                if (timeForEmployee is not null && timeForEmployee.End is not null)
                    timeForEmployee = null;
                
                var attendance = new AttendanceDTO
                {
                    Employee = employee.ToDTO(),
                    IsPresent = timeForEmployee is not null,
                    LastType = timeForEmployee?.Type,
                };

                result.Add(attendance);
            }

            return result
                .OrderByDescending(f => f.IsPresent) // Present first
                .ThenBy(f => f.LastType)
                .ToList();
        });
    }
}