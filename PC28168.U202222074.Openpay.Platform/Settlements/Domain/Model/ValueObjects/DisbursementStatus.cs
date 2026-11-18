namespace PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Lifecycle states of a disbursement order.
/// </summary>
/// <remarks>
///     Values begin at 10; a new order is <see cref="Queued" /> unless another state is given.
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public enum DisbursementStatus
{
    /// <summary>Waiting in line to be executed (default).</summary>
    Queued = 10,

    /// <summary>Executed successfully.</summary>
    Processed = 11,

    /// <summary>Cancelled before execution.</summary>
    Cancelled = 12,

    /// <summary>Execution failed.</summary>
    Failed = 13
}
