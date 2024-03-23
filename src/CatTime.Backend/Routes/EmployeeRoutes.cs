using CatTime.Backend.Database;
using CatTime.Backend.Extensions;
using CatTime.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CatTime.Backend.Routes;

public static class EmployeeRoutes
{
    public static void MapEmployee(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/employees");
        group.RequireAuthorization();

        group.MapGet("/", async (CatContext catContext, HttpContext httpContext) =>
        {
            var companyId = httpContext.User.GetCompanyId();

            var employees = await catContext.Employees.Where(e => e.Company.Id == companyId).ToListAsync();

            return employees.Select(e => e.ToDTO()).ToList();
        });

        group.MapGet("/{id}", async (CatContext catContext, HttpContext httpContext, int id) =>
        {
            var companyId = httpContext.User.GetCompanyId();

            var employee = await catContext.Employees.FirstOrDefaultAsync(e => e.Id == id && e.Company.Id == companyId);

            if(employee == null)
            {
                return Results.Problem("Der Mitarbeiter wurde nicht gefunden oder sie haben kein Recht die Daten abzurufen.", statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(employee.ToDTO());
        });
    }
}