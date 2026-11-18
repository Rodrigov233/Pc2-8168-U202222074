using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PC28168.U202222074.Openpay.Platform.Resources;
using PC28168.U202222074.Openpay.Platform.Shared.Domain.Repositories;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;

/// <summary>
///     Service registrations shared by every bounded context.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Registers controllers with kebab-case routes, localized data annotations and a localized
    ///     validation problem response.
    /// </summary>
    /// <param name="builder">Web application builder.</param>
    /// <returns>The same builder.</returns>
    public static WebApplicationBuilder AddLocalizedControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddLocalization();
        builder.Services
            .AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
            .AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)))
            .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context =>
            {
                var services = context.HttpContext.RequestServices;
                var localizer = services.GetRequiredService<IStringLocalizer<SharedResource>>();
                var problem = services.GetRequiredService<ProblemDetailsFactory>().CreateValidationProblemDetails(
                    context.HttpContext, context.ModelState, StatusCodes.Status400BadRequest,
                    localizer["InvalidModelStateTitle"].Value);
                return new BadRequestObjectResult(problem) { ContentTypes = { "application/problem+json" } };
            });
        return builder;
    }

    /// <summary>
    ///     Registers RFC 7807 problem details with localized texts for unhandled errors.
    /// </summary>
    /// <param name="builder">Web application builder.</param>
    /// <returns>The same builder.</returns>
    public static WebApplicationBuilder AddLocalizedProblemDetails(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
        {
            if (context.ProblemDetails.Status is not (null or >= StatusCodes.Status500InternalServerError)) return;
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.ProblemDetails.Title ??= localizer["UnexpectedServerError"].Value;
            context.ProblemDetails.Detail ??= localizer["UnexpectedErrorProcessingRequest"].Value;
        });
        return builder;
    }

    /// <summary>
    ///     Registers the MySQL <see cref="AppDbContext" /> and the unit of work.
    /// </summary>
    /// <param name="builder">Web application builder.</param>
    /// <returns>The same builder.</returns>
    /// <exception cref="InvalidOperationException">Raised when the connection string is missing.</exception>
    public static WebApplicationBuilder AddSharedPersistence(this WebApplicationBuilder builder)
    {
        var template = builder.Configuration.GetConnectionString("DefaultConnection");
        var connectionString = string.IsNullOrWhiteSpace(template)
            ? throw new InvalidOperationException("Database connection string is not set in the configuration.")
            : Environment.ExpandEnvironmentVariables(template);

        builder.Services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options.UseMySQL(connectionString)
                .UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>())
                .EnableDetailedErrors();
            if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
        });
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        return builder;
    }
}
