using CatTime.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatTime.Backend.Database;

public class CatContext : DbContext
{
    public CatContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Company> Companies => this.Set<Company>();
    public DbSet<Employee> Employees => this.Set<Employee>();
    public DbSet<WorkingTime> WorkingTimes => this.Set<WorkingTime>();
}