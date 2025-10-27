using CleanAspCore.Core.Data.Models.Departments;
using CleanAspCore.Core.Data.Models.Employees;
using CleanAspCore.Core.Data.Models.Jobs;
using CleanAspCore.Core.Data.Models.Weapons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CleanAspCore.Core.Data;

public sealed class HrContext : DbContext
{
    public const string ConnectionStringName = "Database";
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<Weapon> Weapons => Set<Weapon>();

    private readonly string? _connectionString;

    public HrContext() { }

    public HrContext(DbContextOptions<HrContext> context, IConfiguration configuration) : base(context)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringName);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
