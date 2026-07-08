using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderManagementCommon.Events;
using OrderManagementCommon.Models;

namespace OrderManagementCommon;

public abstract class OrderManagementDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<DomainEventRow> Eventi { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Postgresql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.Entity<DomainEventRow>((options) =>
        {
            options.HasKey((d) => d.Id);

            options.Property((d) => d.Id)
                .HasConversion((_) => _.Id, (_) => new IdEvento(_));

            options.Property((d) => d.Tipo);
            options.Property((d) => d.Contenuto);
            options.Property((d) => d.IsCompletato);
            options.Property((d) => d.MomentoCreazione);
        });
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var eventsRow = ChangeTracker.Entries<AggregateRoot>()
            .Select((x) => x.Entity)
            .Where((x) => x.DomainEvents.Count > 0)
            .SelectMany(x =>
            {
                var events = x.DomainEvents.ToList();
                x.ClearDomainEvents();
                return events;
            })
            .Select(DomainEventRow.FromEvent)
            .ToList();

        if (eventsRow.Count > 0)
            await Eventi.AddRangeAsync(eventsRow, cancellationToken);


        return await base.SaveChangesAsync(cancellationToken);
    }
}