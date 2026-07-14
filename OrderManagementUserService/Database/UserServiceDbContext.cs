using Microsoft.EntityFrameworkCore;
using OrderManagementCommon;
using OrderManagementCommon.Models;
using OrderManagementUserService.Models;

namespace OrderManagementUserService.Database;

public class UserServiceDbContext(IConfiguration configuration) : OrderManagementDbContext(configuration)
{
    public DbSet<Utente> Utenti { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Utente>((options) =>
        {
            options.Ignore((u)=>u.Id);
            options.HasKey("IdRaw");

            options.Property<Guid>("IdRaw")
                .HasColumnName("Id")
                .UsePropertyAccessMode(PropertyAccessMode.PreferProperty);


            options.ComplexProperty((u) => u.Credenziali, (options) =>
            {
                options.Property((c) => c.Username).HasColumnName("Username");
                options.Property((c) => c.Password)
                    .HasColumnName("Password")
                    .HasConversion((_)=>_.Value, (_)=>new Password(_));
            });

            options.ComplexProperty((u) => u.Generalita, (options) =>
            {
                options.Property((g) => g.Nome).HasColumnName("Nome");
                options.Property((g) => g.Cognome).HasColumnName("Cognome");
            });

            options.HasDiscriminator<string>("Ruolo")
                .HasValue<Admin>("Admin")
                .HasValue<DeliveryGuy>("DeliveryGuy")
                .HasValue<Customer>("Customer");
        });
    }
}