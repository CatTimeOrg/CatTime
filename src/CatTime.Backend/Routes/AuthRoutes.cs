﻿using System.Security.Claims;
using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
using CatTime.Backend.Extensions;
using CatTime.Shared;
using CatTime.Shared.Routes.Auth;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CatTime.Backend.Routes;

public static class AuthRoutes
{
    public static void MapAuth(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/auth");
        group.RequireAuthorization();

        group.MapPost("/login", async (LoginRequest request, CatContext catContext) =>
        {
            var employee = await catContext.Employees
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.EmailAddress == request.EmailAddress);
            
            if (employee == null)
            {
                return Results.Problem("E-Mail-Adresse oder Passwort ist ungültig.", statusCode: StatusCodes.Status400BadRequest);
            }

            if (BCrypt.Net.BCrypt.Verify(request.Password ?? string.Empty, employee.PasswordHash) is false)
            {
                return Results.Problem("E-Mail-Adresse oder Passwort ist ungültig.", statusCode: StatusCodes.Status400BadRequest);
            }

            var principal = EmployeeToPrincipal(employee);

            return Results.SignIn(principal);
        }).AllowAnonymous();

        group.MapPost("/refresh", async (RefreshRequest request, CatContext catContext, IOptionsMonitor<BearerTokenOptions> optionsMonitor) =>
        {
            var options = optionsMonitor.Get(BearerTokenDefaults.AuthenticationScheme);

            var ticket = options.RefreshTokenProtector.Unprotect(request.RefreshToken);
            if (ticket?.Properties?.ExpiresUtc is not { } expiresUtc)
            {
                return Results.Problem("Ungültiges Refresh-Token.", statusCode: StatusCodes.Status400BadRequest);
            }
            
            if (DateTime.UtcNow >= expiresUtc)
            {
                return Results.Problem("Refresh-Token ist abgelaufen.", statusCode: StatusCodes.Status400BadRequest);
            }

            var employeeId = ticket.Principal.GetEmployeeId();
            var employee = await catContext.Employees.IgnoreQueryFilters().SingleAsync(f => f.Id == employeeId);

            var principal = EmployeeToPrincipal(employee);

            return Results.SignIn(principal);

        }).AllowAnonymous();
        
        group.MapPost("/register", async (RegisterRequest request, CatContext catContext) =>
        {
            if (string.IsNullOrWhiteSpace(request?.FirstName))
                return Results.Problem("Vorname ist erforderlich.", statusCode: StatusCodes.Status400BadRequest);
            if (string.IsNullOrWhiteSpace(request?.LastName))
                return Results.Problem("Nachname ist erforderlich.", statusCode: StatusCodes.Status400BadRequest);
            if (string.IsNullOrWhiteSpace(request?.EmailAddress))
                return Results.Problem("E-Mail-Adresse ist erforderlich.", statusCode: StatusCodes.Status400BadRequest);
            if (string.IsNullOrWhiteSpace(request?.Password))
                return Results.Problem("Passwort ist erforderlich.", statusCode: StatusCodes.Status400BadRequest);
            if (request.Password.Length is < 8 or > 30)
                return Results.Problem("Passwort muss zwischen 8 und 30 Zeichen lang sein.", statusCode: StatusCodes.Status400BadRequest);
            
            var existingEmployee = await catContext.Employees.FirstOrDefaultAsync(e => e.EmailAddress == request.EmailAddress);
            if (existingEmployee != null)
            {
                return Results.Problem("E-Mail-Adresse bereits vergeben.", statusCode: StatusCodes.Status400BadRequest);
            }

            var company = new Company
            {
                Name = request.EmailAddress,
            };
            await catContext.Companies.AddAsync(company);

            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                
                EmailAddress = request.EmailAddress,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),

                CompanyId = company.Id,
                Company = company,
                Role = EmployeeRole.Admin,
                IsActive = true,
            };
            await catContext.Employees.AddAsync(employee);

            await catContext.SaveChangesAsync();

            return Results.Ok();
        }).AllowAnonymous();
        
        group.MapGet("/me", async (CatContext catContext, HttpContext context) =>
        {
            var employeeId = context.User.GetEmployeeId();
            var employee = await catContext.Employees.FindAsync(employeeId);

            return employee.ToDTO();
        });
    }

    private static ClaimsPrincipal EmployeeToPrincipal(Employee employee)
    {
        var identity = new ClaimsIdentity(BearerTokenDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim("EmployeeId", employee.Id.ToString()));
        identity.AddClaim(new Claim("CompanyId", employee.CompanyId.ToString()));

        var principal = new ClaimsPrincipal(identity);
        return principal;
    }
}