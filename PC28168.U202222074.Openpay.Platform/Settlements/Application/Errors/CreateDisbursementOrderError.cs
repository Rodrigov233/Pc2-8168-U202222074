namespace PC28168.U202222074.Openpay.Platform.Settlements.Application.Errors;

/// <summary>
///     Reasons why registering a disbursement order can fail in the application layer.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public enum CreateDisbursementOrderError
{
    /// <summary>Another order already uses the same order tracking code.</summary>
    OrderTrackingCodeAlreadyExists,

    /// <summary>The order could not be stored.</summary>
    PersistenceFailure
}
