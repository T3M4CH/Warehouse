using Microsoft.EntityFrameworkCore;
using NLog;
using Warehouse.Data;

namespace Warehouse.Helpers;

public static class MigrationExtension
{
    public static WebApplication ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            logger.Info("Applying migrations...");
            var context = services.GetRequiredService<DataContext>();
            context.Database.Migrate();
            logger.Info("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred while applying migrations.");
        }
        
        return app;
    }
}