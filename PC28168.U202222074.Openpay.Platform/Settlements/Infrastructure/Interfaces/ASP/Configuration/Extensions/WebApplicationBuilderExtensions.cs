using PC28168.U202222074.Openpay.Platform.Settlements.Application.Internal.CommandServices;
using PC28168.U202222074.Openpay.Platform.Settlements.Application.Services;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U202222074.Openpay.Platform.Settlements.Infrastructure.Persistence.EFC.Repositories;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Infrastructure.Interfaces.ASP.Configuration.Extensions;

/// <summary>
///     Dependency injection of the Settlements bounded context.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Registers the repositories and application services of Settlements.
    /// </summary>
    /// <param name="builder">Web application builder.</param>
    /// <returns>The same builder.</returns>
    public static WebApplicationBuilder AddSettlementsContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDisbursementOrderRepository, DisbursementOrderRepository>();
        builder.Services.AddScoped<IDisbursementOrderCommandService, DisbursementOrderCommandService>();
        return builder;
    }
}
