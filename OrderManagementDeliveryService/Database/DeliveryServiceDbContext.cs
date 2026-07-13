using Microsoft.EntityFrameworkCore;
using OrderManagementCommon;
using OrderManagementCommon.Models;
using OrderManagementDeliveryService.Models;

namespace OrderManagementDeliveryService.Database;

public class DeliveryServiceDbContext(IConfiguration configuration) : OrderManagementDbContext(configuration)
{
    public DbSet<Order> Ordini { get; private set; }

    public DbSet<DeliveryGuy> DeliveryGuys { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>((options) =>
        {
            options.HasKey((o) => o.Id);

            options.Property((o) => o.Id)
                .HasConversion((_) => _.Valore, (_) => new IdOrder(_));

            options.Property((o) => o.IdProdotto)
                .HasConversion((_) => _.Id, (_) => new IdProdotto(_));

            options.Property((o) => o.IdUtente)
                .HasConversion((_) => _.Id, (_) => new IdCustomer(_));

            options.Property((o) => o.Quantita)
                .HasConversion((_) => _.Valore, (_) => new Quantita(_));

            options.Property((o) => o.MomentoAcquisto);
            
            options.Property((o)=>o.IsDeliveryGuyNotified);

            options.HasIndex((o) => new { o.IdProdotto, o.IdUtente, o.MomentoAcquisto })
                .IsUnique();

            options.Property((o)=>o.Indirizzo)
                .HasConversion((_)=>_.Valore, (_)=>new Indirizzo(_));

            options.Ignore((o) => o.Status);
            options.Ignore((o)=>o.IdDeliveryGuyAssigned);

            options.ComplexProperty<OrderStatusRappresentation>("OrderStatusRappresentation", (options) =>
            {
                options.Property(p => p.MomentoCancellazione)
                    .HasColumnName("MomentoCancellazione");

                options.Property(p => p.MomentoConsegna)
                    .HasColumnName("MomentoConsegna");

                options.Property(p => p.RagioneCancellazione)
                    .HasColumnName("RagioneCancellazione");

                options.Property(p => p.IdDeliveryGuyAssegnato)
                    .HasColumnName("IdDeliveryGuyAssegnato")
                    .HasConversion<Guid?>((_) => _ != null ? _.Id : null, (_) => _ != null ? new IdDeliveryGuy(_.Value) : null);

                options.Property(p => p.Tipo)
                    .HasColumnName("Tipo");
            })
            .UsePropertyAccessMode(PropertyAccessMode.PreferProperty);
        });

        modelBuilder.Entity<DeliveryGuy>((options) =>
        {
            options.HasKey((o)=>o.Id);
            
            options.Property((o)=>o.Id)
                .HasConversion((_)=>_.Id, (_)=> new IdDeliveryGuy(_));

            options.Property((o)=>o.NumeroConsegneAttive);
        });
    }
}