using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
using CatTime.Backend.Extensions;
using CatTime.Shared;
using CatTime.Shared.Routes.WorkingTimes;

namespace CatTime.Backend.Routes;

public static class WorkingTimeRoutes
{
    public static void MapWorkingTime(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/workingtime");
        group.RequireAuthorization();

        group.MapPost("/", async (CreateWorkingTimeRequest request, CatContext catContext, HttpContext httpContext) =>
        {
            // Load Employee
            var employeeId = request.EmployeeId ?? httpContext.User.GetEmployeeId();
            var employee = catContext.Employees.Find(employeeId);
            if (employee == null)
            {
                return Results.Problem("Mitarbeiter nicht gefunden.", statusCode: StatusCodes.Status400BadRequest);
            }
            await catContext.Entry(employee).Reference(f => f.Company).LoadAsync();
            
            // Check if employee is from same company
            var companyId = httpContext.User.GetCompanyId();
            if (employee.Company.Id != companyId)
            {
                return Results.Problem("Mitarbeiter nicht gefunden.", statusCode: StatusCodes.Status400BadRequest);
            }
            var company = catContext.Companies.Find(companyId);

            var workingTimeEntity = new WorkingTime
            {
                Employee = employee,
                Company = company,
                
                Date = request.Date,
                Start = request.Start,
                End = request.End,
                
                Type = request.Type,
            };
            
            await catContext.WorkingTimes.AddAsync(workingTimeEntity);
            await catContext.SaveChangesAsync();
            
            var result = new WorkingTimeDTO
            {
                Id = workingTimeEntity.Id,
                
                EmployeeId = workingTimeEntity.Employee.Id,
                CompanyId = workingTimeEntity.Company.Id,

                Date = workingTimeEntity.Date,
                Start = workingTimeEntity.Start,
                End = workingTimeEntity.End,

                Type = workingTimeEntity.Type,
            };
            return Results.Ok(result);
        });
    }
}