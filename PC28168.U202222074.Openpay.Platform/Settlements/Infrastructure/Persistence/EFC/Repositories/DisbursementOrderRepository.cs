using Microsoft.EntityFrameworkCore;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     EF Core adapter of <see cref="IDisbursementOrderRepository" />.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="context">Shared database context.</param>
public class DisbursementOrderRepository(AppDbContext context)
    : BaseRepository<DisbursementOrder>(context), IDisbursementOrderRepository
{
    /// <inheritdoc />
    public Task<DisbursementOrder?> FindByOrderTrackingCodeAsync(OpenpayReference orderTrackingCode,
        CancellationToken cancellationToken = default) =>
        Context.Set<DisbursementOrder>()
            .AsNoTracking()
            .SingleOrDefaultAsync(order => order.OrderTrackingCode == orderTrackingCode, cancellationToken);
}
