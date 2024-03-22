using System.Security.Claims;

namespace CatTime.Backend.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetEmployeeId(this ClaimsPrincipal claimsPrincipal) => int.Parse(claimsPrincipal.FindFirst("EmployeeId").Value);
    public static int GetCompanyId(this ClaimsPrincipal claimsPrincipal) => int.Parse(claimsPrincipal.FindFirst("CompanyId").Value);
}