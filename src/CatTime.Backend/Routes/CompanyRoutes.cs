using CatTime.Backend.Database;
using CatTime.Backend.Exceptions;
using CatTime.Backend.Extensions;
using CatTime.Shared;

namespace CatTime.Backend.Routes;

public static class CompanyRoutes
{
    public static void MapCompany(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/companies");
        group.RequireAuthorization();

        group.MapGet("/me", async (CatContext catContext, HttpContext httpContext) =>
        {
            var companyId = httpContext.User.GetCompanyId();
            var company = await catContext.Companies.FindAsync(companyId) ?? throw new SomethingFishyException();

            return company.ToDTO();
        });

        group.MapPut("/me", async (CompanyDTO companyDTO, CatContext catContext, HttpContext httpContext) =>
        {
            if (string.IsNullOrWhiteSpace(companyDTO?.Name))
                return Results.Problem("Name ist erforderlich.", statusCode: StatusCodes.Status400BadRequest);
            
            var companyId = httpContext.User.GetCompanyId();
            var company = await catContext.Companies.FindAsync(companyId) ?? throw new SomethingFishyException();

            company.Name = companyDTO.Name;

            await catContext.SaveChangesAsync();

            return Results.Ok(company.ToDTO());
        });
    }
}