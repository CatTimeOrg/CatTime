using System.Security.Claims;
using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
using CatTime.Shared;
using CatTime.Shared.Routes.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

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

            if (BCrypt.Net.BCrypt.Verify(request.Password, employee.PasswordHash) is false)
            {
                return Results.Problem("E-Mail-Adresse oder Passwort ist ungültig.", statusCode: StatusCodes.Status400BadRequest);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("EmployeeId", employee.Id.ToString()));
            identity.AddClaim(new Claim("CompanyId", employee.CompanyId.ToString()));

            var principal = new ClaimsPrincipal(identity);

            return Results.SignIn(principal, null, CookieAuthenticationDefaults.AuthenticationScheme);
        }).AllowAnonymous();

        group.MapGet("/logout", (HttpContext context) =>
        {
            return Results.SignOut(null, [CookieAuthenticationDefaults.AuthenticationScheme]);
        });

        group.MapPost("/register", async (RegisterRequest request, CatContext catContext) =>
        {
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
            };
            await catContext.Employees.AddAsync(employee);

            await catContext.SaveChangesAsync();

            return Results.Ok();
        }).AllowAnonymous();
        
        group.MapGet("/me", (HttpContext context) =>
        {
            var claims = context.User.Claims.Select(c => new ClaimDTO { Type = c.Type, Value = c.Value }).ToList();

            return Results.Ok(claims);
        });
    }
}