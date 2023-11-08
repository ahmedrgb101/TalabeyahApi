using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TalabeyahTaskApi.Application.Common.Events;
using TalabeyahTaskApi.Application.Common.Interfaces;
using TalabeyahTaskApi.Domain.Catalog;
using TalabeyahTaskApi.Domain.Common;
using TalabeyahTaskApi.Infrastructure.Persistence.Configuration;

namespace TalabeyahTaskApi.Infrastructure.Persistence.Context;
public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Ticket> Ticket => Set<Ticket>();
    public DbSet<Governorate> Governorate => Set<Governorate>();
    public DbSet<City> City => Set<City>();
    public DbSet<District> District => Set<District>();

    public DbSet<Notification> Notification => Set<Notification>();
    public DbSet<UserNotification> UserNotification => Set<UserNotification>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}