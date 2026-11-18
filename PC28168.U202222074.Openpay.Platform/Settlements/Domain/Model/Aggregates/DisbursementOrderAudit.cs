using PC28168.U202222074.Openpay.Platform.Shared.Domain.Model;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;

/// <summary>
///     Audit trail of the <see cref="DisbursementOrder" /> aggregate.
/// </summary>
/// <remarks>
///     Filled by the Shared AuditableEntityInterceptor on every save.
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public partial class DisbursementOrder : IAuditableEntity
{
    /// <summary>Moment the order was first stored (created_at).</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Moment the order was last stored (updated_at).</summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
