using Warehouse.Helpers;
using NLog.Web;
using NLog;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.AddApplicationServices();

    var app = builder.Build();

    app.AddMiddleware();
    app.ApplyMigration();
    //app.UseHttpsRedirection();

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Приложение аварийно завершилось");
    throw;
}
finally
{
    LogManager.Shutdown();
}