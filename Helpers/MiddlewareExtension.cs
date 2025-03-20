using NLog;
using Scalar.AspNetCore;

namespace Warehouse.Helpers;

public static class MiddlewareExtension
{
    public static WebApplication AddMiddleware(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex, "Unhandled exception");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal Server Error");
            }
        });

        app.UseCors(corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        app.UseDeveloperExceptionPage();
        app.MapGet("/", () => Results.Redirect("scalar/v1"));

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseStaticFiles();

        return app;
    }
}