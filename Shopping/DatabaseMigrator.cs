using Infrastracture.Catalogue.EF.Persistance;
using Infrastructure.Sale;
using Microsoft.EntityFrameworkCore;
using Security;

namespace Shopping
{
    public static class DatabaseMigrator
    {
        public static async Task MigrateAsync(IServiceProvider services, int maxAttempts = 15)
        {
            var delay = TimeSpan.FromSeconds(3);

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using var scope = services.CreateScope();
                    var provider = scope.ServiceProvider;

                    await provider.GetRequiredService<SecurityContext>().Database.MigrateAsync();
                    await provider.GetRequiredService<CatalogueContext>().Database.MigrateAsync();
                    await provider.GetRequiredService<SaleSystemContext>().Database.MigrateAsync();
                    return;
                }
                catch (Exception ex) when (attempt < maxAttempts)
                {
                    Console.WriteLine($"[Startup] SQL Server not ready (attempt {attempt}/{maxAttempts}): {ex.Message}");
                    await Task.Delay(delay);
                }
            }
        }
    }
}
