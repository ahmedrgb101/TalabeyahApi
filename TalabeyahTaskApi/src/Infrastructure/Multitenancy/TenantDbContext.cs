using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using TalabeyahTaskApi.Infrastructure.Persistence.Configuration;

namespace TalabeyahTaskApi.Infrastructure.Multitenancy;
public class TenantDbContext : EFCoreStoreDbContext<FSHTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FSHTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}