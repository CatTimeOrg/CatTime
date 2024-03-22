using CatTime.Backend.Database;
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
            var companyId = int.Parse(httpContext.User.FindFirst("CompanyId")?.Value);
            var company = await catContext.Companies.FindAsync(companyId);

            return new CompanyDTO
            {
                Id = company.Id,
                Name = company.Name,
            };
        });

        group.MapPut("/me", async (CompanyDTO companyDTO, CatContext catContext, HttpContext httpContext) =>
        {
            var companyId = int.Parse(httpContext.User.FindFirst("CompanyId")?.Value);
            var company = await catContext.Companies.FindAsync(companyId);

            company.Name = companyDTO.Name;

            await catContext.SaveChangesAsync();

            return new CompanyDTO
            {
                Id = company.Id,
                Name = company.Name,
            };
        });
    }
}