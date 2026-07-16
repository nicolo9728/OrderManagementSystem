using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementCommon;

public static class DatabaseMigration
{
    extension(WebApplication app)
    {
        public void MigraDatabase()
        {
            var services = app.Services.CreateScope();
            var context = services.ServiceProvider.GetService<OrderManagementDbContext>();

            context!.Database.EnsureCreated();
            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}