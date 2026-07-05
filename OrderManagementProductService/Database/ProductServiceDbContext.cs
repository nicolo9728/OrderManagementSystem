using Microsoft.EntityFrameworkCore;
using OrderManagementCommon.Models;
using OrderManagementProductService.Models;

namespace OrderManagementCommon.Database;

public class ProductServiceDbContext(IConfiguration configuration) : OrderManagementDbContext(configuration)
{
    public DbSet<Prodotto> Prodotti { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prodotto>((options) =>
        {
            options.HasKey((p)=>p.Codice);

            options.Property((p)=>p.Codice)
                .HasConversion((_)=>_.Id, (_)=>new IdProdotto(_));

            options.Property((p)=>p.Nome);

            options.Property((p)=>p.QuantitaDisponibile)
                .HasConversion((_)=>_.Valore, (_)=>new Quantita(_));
        });

        modelBuilder.Entity<Acquisto>((options) =>
        {
            options.HasKey((a)=>a.Id);

            options.Property((a)=>a.Id)
                .HasConversion((_)=>_.Id, (_)=>new IdAcquisto(_));

            options.Property((a)=>a.Momento);
            options.Property((a)=>a.IdUtente)
                .HasConversion((_)=>_.Id, (_)=>new IdCustomer(_));

            options.Property((a)=>a.QuantitaAcquistata)
                .HasConversion((_)=>_.Valore, (_)=>new Quantita(_));

            options.HasOne<Prodotto>()
                .WithMany()
                .HasForeignKey((a)=>a.IdProdotto);
        });
    }
}