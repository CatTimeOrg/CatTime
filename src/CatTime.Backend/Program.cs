using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
using CatTime.Backend.Routes;
using CatTime.Backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

MigrateDatabase(app.Services);
ConfigurePipeline(app);
ConfigureRoutes(app);

app.Run();

static void ConfigureServices(IServiceCollection services, ConfigurationManager config)
{
    services.AddDbContext<CatContext>(o => o.UseNpgsql(config.GetConnectionString("Postgres")), ServiceLifetime.Scoped);

    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
    services.AddAuthorization();
    
    services.AddHttpContextAccessor();
    services.AddScoped<ITenantService, TenantService>();
}

static void MigrateDatabase(IServiceProvider appServices)
{
    using (var scope = appServices.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CatContext>();
        context.Database.Migrate();
    }
}

static void ConfigurePipeline(IApplicationBuilder app)
{
    app.UseHttpsRedirection();
}

static void ConfigureRoutes(IEndpointRouteBuilder routes)
{
    routes.MapGet("/", () => "Welcome to CatTime-API!");

    routes.MapAuth();
    routes.MapCompany();
    routes.MapEmployee();
    routes.MapWorkingTime();
}