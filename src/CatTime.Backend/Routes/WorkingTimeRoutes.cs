using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
using CatTime.Backend.Extensions;
using CatTime.Shared;
using CatTime.Shared.Routes.WorkingTimes;
using Microsoft.EntityFrameworkCore;

namespace CatTime.Backend.Routes;

public static class WorkingTimeRoutes
{
    public static void MapWorkingTime(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/workingtime");
        group.RequireAuthorization();

        group.MapPost("/", async (CreateWorkingTimeRequest request, CatContext catContext, HttpContext httpContext) =>
        {
            var targetEmployee = await catContext.Employees.FindAsync(request.EmployeeId ?? httpContext.User.GetEmployeeId());
            if (targetEmployee is null)
            {
                return Results.Problem("Mitarbeiter nicht gefunden.", statusCode: StatusCodes.Status400BadRequest);
            }
            
            var currentEmployee = await catContext.Employees.FindAsync(httpContext.User.GetEmployeeId());
            if (currentEmployee != targetEmployee && currentEmployee.Role is not EmployeeRole.Admin)
            {
                return Results.Problem("Nicht autorisiert Zeiten für einen anderen Mitarbeiter anzulegen.", statusCode: StatusCodes.Status400BadRequest);
            }
            
            var workingTimeEntity = new WorkingTime
            {
                EmployeeId = targetEmployee.Id,
                CompanyId = targetEmployee.CompanyId,
                
                Date = request.Date,
                Start = request.Start,
                End = request.End,
                
                Type = request.Type,
            };
            
            await catContext.WorkingTimes.AddAsync(workingTimeEntity);
            await catContext.SaveChangesAsync();
            
            return Results.Ok(workingTimeEntity.ToDTO());
        });

        group.MapGet("/", async (int? employeeId, DateOnly? from, DateOnly? to, CatContext catContext, HttpContext httpContext) =>
        {
            var targetEmployee = await catContext.Employees.FindAsync(employeeId ?? httpContext.User.GetEmployeeId());
            if (targetEmployee is null)
            {
                return Results.Problem("Mitarbeiter nicht gefunden.", statusCode: StatusCodes.Status400BadRequest);
            }
            
            var currentEmployee = await catContext.Employees.FindAsync(httpContext.User.GetEmployeeId());
            if (currentEmployee != targetEmployee && currentEmployee.Role is not EmployeeRole.Admin)
            {
                return Results.Problem("Nicht autorisiert Zeiten für einen anderen Mitarbeiter aufzulisten.", statusCode: StatusCodes.Status400BadRequest);
            }
            
            var actualFrom = from ?? new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1);
            var actualTo = to ?? new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            
            var workingTimes = await catContext.WorkingTimes
                .Where(w => w.EmployeeId == targetEmployee.Id && w.Date >= actualFrom && w.Date <= actualTo)
                .ToListAsync();
            
            return Results.Ok(workingTimes.Select(w => w.ToDTO()));
        });
    }
}