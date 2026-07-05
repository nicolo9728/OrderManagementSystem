using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OrderManagementCommon;

public abstract class OrderManagementDbContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Postgresql"));
    }
}