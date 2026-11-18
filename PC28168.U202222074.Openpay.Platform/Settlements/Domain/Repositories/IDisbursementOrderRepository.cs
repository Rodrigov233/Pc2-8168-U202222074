using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202222074.Openpay.Platform.Shared.Domain.Repositories;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Domain.Repositories;

/// <summary>
///     Persistence port for <see cref="DisbursementOrder" /> aggregates.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public interface IDisbursementOrderRepository : IBaseRepository<DisbursementOrder>
{
    /// <summary>
    ///     Looks up an order by its Openpay reference.
    /// </summary>
    /// <param name="orderTrackingCode">Openpay reference to look for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching order, or <c>null</c> when none exists.</returns>
    Task<DisbursementOrder?> FindByOrderTrackingCodeAsync(OpenpayReference orderTrackingCode,
        CancellationToken cancellationToken = default);
}
