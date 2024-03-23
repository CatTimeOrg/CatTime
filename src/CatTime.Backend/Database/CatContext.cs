using CatTime.Backend.Database.Entities;
using CatTime.Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace CatTime.Backend.Database;

public class CatContext : DbContext
{
    private readonly ITenantService _tenantService;

    public CatContext(DbContextOptions options, ITenantService tenantService) 
        : base(options)
    {
        this._tenantService = tenantService;
    }

    public DbSet<Company> Companies => this.Set<Company>();
    public DbSet<Employee> Employees => this.Set<Employee>();
    public DbSet<WorkingTime> WorkingTimes => this.Set<WorkingTime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .Entity<Company>()
            .HasQueryFilter(f => f.Id == this._tenantService.GetCurrentCompanyId());
        
        modelBuilder
            .Entity<Employee>()
            .HasQueryFilter(f => f.CompanyId == this._tenantService.GetCurrentCompanyId());
        
        modelBuilder
            .Entity<WorkingTime>()
            .HasQueryFilter(f => f.CompanyId == this._tenantService.GetCurrentCompanyId());
    }
}