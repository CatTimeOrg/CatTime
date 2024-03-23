using CatTime.Backend.Extensions;

namespace CatTime.Backend.Services;

public interface ITenantService
{
    int? GetCurrentCompanyId();
}

public class TenantService(IHttpContextAccessor httpContextAccessor) : ITenantService
{
    public int? GetCurrentCompanyId()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
            ? httpContextAccessor.HttpContext.User.GetCompanyId()
            : null;
    }
}