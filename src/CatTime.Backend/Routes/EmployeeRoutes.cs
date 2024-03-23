using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
using CatTime.Backend.Exceptions;
using CatTime.Backend.Extensions;
using CatTime.Shared;
using CatTime.Shared.Routes.Auth;
using CatTime.Shared.Routes.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

namespace CatTime.Backend.Routes;

public static class EmployeeRoutes
{
    public static void MapEmployee(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/employees");
        group.RequireAuthorization();

        group.MapGet("/all", async (CatContext catContext, HttpContext httpContext) =>
        {
            var employees = await catContext.Employees.ToListAsync();

            return employees.Select(e => e.ToDTO()).ToList();
        });

        group.MapGet("/", async (CatContext catContext, HttpContext httpContext) =>
        {
            var employees = await catContext.Employees.Where(e => e.IsActive == true).ToListAsync();

            return employees.Select(e => e.ToDTO()).ToList();
        });        

        group.MapPost("/", async (CreateEmployeeRequest request, CatContext catContext, HttpContext httpContext) =>
        {
            var currentEmployee = await catContext.Employees.FindAsync(httpContext.User.GetEmployeeId()) ?? throw new SomethingFishyException();
            if (currentEmployee.Role != EmployeeRole.Admin)
            {
                return Results.Problem("Sie haben keine Berechtigung um Mitarbeiter hinzuzufügen.", statusCode: StatusCodes.Status403Forbidden);
            }

            var existingEmployee = await catContext.Employees.FirstOrDefaultAsync(e => e.EmailAddress == request.EmailAddress);
            if (existingEmployee != null)
            {
                return Results.Problem("E-Mail-Adresse bereits vergeben.", statusCode: StatusCodes.Status400BadRequest);
            }

            var newEmployee = new Employee
            {
                CompanyId = currentEmployee.CompanyId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                IsActive = true
            };

            await catContext.Employees.AddAsync(newEmployee);
            await catContext.SaveChangesAsync();

            return Results.Ok(newEmployee.ToDTO());
        });

        group.MapGet("/{id}", async (CatContext catContext, HttpContext httpContext, int id) =>
        {
            var employee = await catContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return Results.Problem($"Es wurde kein Mitarbeiter für Id:{id} gefunden.", statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(employee.ToDTO());
        });

        group.MapPut("/{id}", async (UpdateEmployeeRequest request, CatContext catContext, HttpContext httpContext, int id) =>
        {
            var currentEmployee = await catContext.Employees.FindAsync(httpContext.User.GetEmployeeId()) ?? throw new SomethingFishyException();
            if (currentEmployee.Role != EmployeeRole.Admin)
            {
                return Results.Problem("Sie haben keine Berechtigung um Mitarbeiterdaten zu verändern.", statusCode: StatusCodes.Status403Forbidden);
            }

            // We ignore the query filters because the email must be unique in the whole database not only for the company. That is because of the login process.
            var existingEmployees = await catContext.Employees.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.EmailAddress == request.EmailAddress && e.Id != id);
            if(existingEmployees != null)
            {
                return Results.Problem("Die E-Mail-Adresse im Request ist bereits vergeben.", statusCode: StatusCodes.Status400BadRequest);
            }

            var employee = await catContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return Results.Problem($"Es wurde kein Mitarbeiter für Id:{id} gefunden", statusCode: StatusCodes.Status400BadRequest);
            }

            employee.FirstName = request.FirstName != null ? request.FirstName : employee.FirstName;
            employee.LastName = request.LastName != null ? request.LastName : employee.LastName;
            employee.EmailAddress = request.EmailAddress != null ? request.EmailAddress : employee.EmailAddress;
            employee.PasswordHash = request.Password != null ? BCrypt.Net.BCrypt.HashPassword(request.Password) : employee.PasswordHash;
            employee.Role = request.Role.HasValue ? request.Role.Value : employee.Role;
            employee.IsActive = request.IsActive.HasValue ? request.IsActive.Value : employee.IsActive;

            await catContext.SaveChangesAsync();

            return Results.Ok(employee.ToDTO());
        });

        group.MapPost("/actions/change-password", async (ChangePasswordRequest request, CatContext catContext, HttpContext httpContext) =>
        {
            var currentEmployee = await catContext.Employees.FindAsync(httpContext.User.GetEmployeeId()) ?? throw new SomethingFishyException();
            if (BCrypt.Net.BCrypt.Verify(request.CurrentPassword, currentEmployee.PasswordHash) is false)
            {
                return Results.Problem("Das angegebene aktuelle Passwort ist falsch.", statusCode: StatusCodes.Status400BadRequest);
            }

            currentEmployee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            await catContext.SaveChangesAsync();

            return Results.Ok();
        });
    }
}