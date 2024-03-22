using CatTime.Backend.Database;
using CatTime.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<CatContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapGet("/test", (CatContext context) =>
{
    context.Companies.Add(new Company
    {
        Name = "Test"
    });

    context.SaveChanges();
});

app.Run();