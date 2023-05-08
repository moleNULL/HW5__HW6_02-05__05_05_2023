using HW_ADONET__EFCore.DatabaseHelpers.EFCoreHelpers.Configurations;
using HW_ADONET__EFCore.DatabaseHelpers.Entities;
using Microsoft.EntityFrameworkCore;

namespace HW_ADONET__EFCore.DatabaseHelpers.EFCoreRepositories;

public class ChiHomeworkDbContext : DbContext
{
    private readonly string _connectionString;
    public ChiHomeworkDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<AnalysisEntity> Analyses { get; set; }

    public DbSet<GroupEntity> Groups { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnalysisConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
    }
}