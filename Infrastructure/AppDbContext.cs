using Microsoft.EntityFrameworkCore;
using Timescale.Api.Domain.Entities;

namespace Timescale.Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<FileEntity> Files => Set<FileEntity>();
    public DbSet<ValueEntity> Values => Set<ValueEntity>();
    public DbSet<ResultEntity> Results => Set<ResultEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}