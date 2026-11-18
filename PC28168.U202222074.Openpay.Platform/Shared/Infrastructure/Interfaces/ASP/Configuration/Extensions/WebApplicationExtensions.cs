using Microsoft.EntityFrameworkCore;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;

/// <summary>
///     Request pipeline helpers shared by every bounded context.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class WebApplicationExtensions
{
    private static readonly string[] SupportedCultures = ["en", "en-US", "es", "es-PE"];

    /// <summary>
    ///     Applies the pending EF Core migrations, creating the openpay_settlements schema when needed.
    /// </summary>
    /// <param name="app">Web application.</param>
    /// <returns>The same application.</returns>
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
        return app;
    }

    /// <summary>
    ///     Selects the response culture from the Accept-Language header (English by default).
    /// </summary>
    /// <param name="app">Web application.</param>
    /// <returns>The same application.</returns>
    public static WebApplication UseAcceptLanguageLocalization(this WebApplication app)
    {
        var options = new RequestLocalizationOptions()
            .SetDefaultCulture(SupportedCultures[0])
            .AddSupportedCultures(SupportedCultures)
            .AddSupportedUICultures(SupportedCultures);
        options.ApplyCurrentCultureToResponseHeaders = true;
        app.UseRequestLocalization(options);
        return app;
    }
}
