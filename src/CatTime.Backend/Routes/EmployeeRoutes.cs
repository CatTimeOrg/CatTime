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

        group.MapGet("/", async (CatContext catContext, HttpContext httpContext) =>
        {
            var employees = await catContext.Employees.ToListAsync();

            return employees.Select(e => e.ToDTO()).ToList();
        });

        group.MapPost("/", async (CreateEmployeeRequest createEmployeeRequest, CatContext catContext, HttpContext httpContext) =>
        {
            var currentEmployee = await catContext.Employees.FindAsync(httpContext.User.GetEmployeeId()) ?? throw new SomethingFishyException();
            if (currentEmployee.Role != EmployeeRole.Admin)
            {
                return Results.Problem("Sie haben keine Berechtigung um Mitarbeiter hinzuzufügen.", statusCode: StatusCodes.Status403Forbidden);
            }

            var existingEmployee = await catContext.Employees.FirstOrDefaultAsync(e => e.EmailAddress == createEmployeeRequest.EmailAddress);
            if (existingEmployee != null)
            {
                return Results.Problem("E-Mail-Adresse bereits vergeben.", statusCode: StatusCodes.Status400BadRequest);
            }

            var newEmployee = new Employee
            {
                CompanyId = currentEmployee.CompanyId,
                FirstName = createEmployeeRequest.FirstName,
                LastName = createEmployeeRequest.LastName,
                EmailAddress = createEmployeeRequest.EmailAddress,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createEmployeeRequest.Password),
                Role = createEmployeeRequest.Role
            };

            await catContext.Employees.AddAsync(newEmployee);
            await catContext.SaveChangesAsync();

            return Results.Ok(newEmployee.ToDTO());
        });

        group.MapGet("/{id}", async (CatContext catContext, HttpContext httpContext, int id) =>
        {
            var employee = await catContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if(employee == null)
            {
                return Results.Problem($"Es wurde kein Mitarbeiter für Id:{id} gefunden.", statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(employee.ToDTO());
        });

        
    }
}