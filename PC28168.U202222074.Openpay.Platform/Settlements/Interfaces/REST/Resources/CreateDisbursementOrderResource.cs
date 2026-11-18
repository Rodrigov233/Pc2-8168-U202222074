using System.ComponentModel.DataAnnotations;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Resources;

/// <summary>
///     Request body to register a disbursement order. Value objects arrive as separate primitive fields.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public record CreateDisbursementOrderResource
{
    /// <summary>Openpay reference (UUID) of the order.</summary>
    [Required(ErrorMessage = "OrderTrackingCodeRequired")]
    public Guid? OrderTrackingCode { get; init; }

    /// <summary>UUID of the affiliate store.</summary>
    [Required(ErrorMessage = "AffiliateStoreIdRequired")]
    public Guid? AffiliateStoreId { get; init; }

    /// <summary>Start date of the execution schedule.</summary>
    [Required(ErrorMessage = "ScheduledFromRequired")]
    public DateOnly? ScheduledFrom { get; init; }

    /// <summary>End date of the execution schedule.</summary>
    [Required(ErrorMessage = "ScheduledToRequired")]
    public DateOnly? ScheduledTo { get; init; }

    /// <summary>Maximum amount to transfer.</summary>
    [Required(ErrorMessage = "TransferCapAmountRequired")]
    public decimal? TransferCapAmount { get; init; }

    /// <summary>Currency code of the maximum amount.</summary>
    [Required(ErrorMessage = "TransferCapCurrencyCodeRequired")]
    [MaxLength(10, ErrorMessage = "TransferCapCurrencyCodeTooLong")]
    public string? TransferCapCurrencyCode { get; init; }

    /// <summary>Initial status name (Queued, Processed, Cancelled or Failed). Queued when omitted.</summary>
    public string? Status { get; init; }

    /// <summary>Operator user that registers the order.</summary>
    [Required(ErrorMessage = "UserRequired")]
    public string? User { get; init; }

    /// <summary>Optional remarks.</summary>
    public string? Remarks { get; init; }
}
