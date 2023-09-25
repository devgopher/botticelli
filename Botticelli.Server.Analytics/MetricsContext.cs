using Botticelli.Server.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Analytics;

public class MetricsContext : DbContext
{
    public MetricsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<MetricModel> MetricModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<MetricModel>();
}