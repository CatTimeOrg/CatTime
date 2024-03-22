using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
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
    services.AddDbContext<CatContext>(o => o.UseNpgsql(config.GetConnectionString("Postgres")));
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
    routes.MapGet("/test", (CatContext context) =>
    {
        context.Companies.Add(new Company
        {
            Name = "Test"
        });

        context.SaveChanges();
    });
}