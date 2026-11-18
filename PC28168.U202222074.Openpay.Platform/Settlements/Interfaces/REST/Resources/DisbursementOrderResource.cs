namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Resources;

/// <summary>
///     Response body that describes a stored disbursement order.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="Id">Database identity.</param>
/// <param name="OrderTrackingCode">Openpay reference as text.</param>
/// <param name="AffiliateStoreId">Affiliate store UUID as text.</param>
/// <param name="ScheduledFrom">Start date as text (yyyy-MM-dd).</param>
/// <param name="ScheduledTo">End date as text (yyyy-MM-dd).</param>
/// <param name="TransferCapAmount">Maximum amount to transfer.</param>
/// <param name="TransferCapCurrencyCode">Currency code of the maximum amount.</param>
/// <param name="Status">Status name.</param>
/// <param name="User">Operator user.</param>
/// <param name="Remarks">Remarks, if any.</param>
/// <param name="UpdatedAt">Last update of the record.</param>
public record DisbursementOrderResource(
    int Id,
    string OrderTrackingCode,
    string AffiliateStoreId,
    string ScheduledFrom,
    string ScheduledTo,
    decimal TransferCapAmount,
    string TransferCapCurrencyCode,
    string Status,
    string User,
    string? Remarks,
    DateTimeOffset? UpdatedAt);
