using Integrations.ModernService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Integrations.ModernService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<OrganizationSetting> OrganizationSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrganizationSetting>(entity =>
        {
            entity.ToTable("hrm_organization_settings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SettingValue).HasColumnName("value").IsRequired().HasMaxLength(255);
            entity.Property(e => e.SettingName).HasColumnName("name").IsRequired().HasMaxLength(255);
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id").IsRequired();
        });
    }
}