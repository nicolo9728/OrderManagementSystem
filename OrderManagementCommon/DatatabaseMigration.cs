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

            bool fine = false;
            while (!fine)
            {
                try
                {
                    context!.Database.EnsureCreated();
                    context.Database.Migrate();
                    fine = true;
                }
                catch (Exception)
                {
                    Thread.Sleep(500);
                }
            }
        }
    }
}