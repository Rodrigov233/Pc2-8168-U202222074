using Microsoft.EntityFrameworkCore;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Interceptors;

namespace PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
/// <remarks>
///     Entity mappings live in each bounded context as IEntityTypeConfiguration classes and are
///     discovered from this assembly.
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="options">The options for this context.</param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.UseSnakeCaseNamingConvention();
    }
}
