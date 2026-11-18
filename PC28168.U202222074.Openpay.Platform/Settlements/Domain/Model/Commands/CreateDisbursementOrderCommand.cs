using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202222074.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Commands;

/// <summary>
///     Intention to register a new disbursement order.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="OrderTrackingCode">Openpay reference of the order.</param>
/// <param name="AffiliateStoreId">Affiliate store that receives the funds.</param>
/// <param name="ExecutionSchedule">Period in which the order may be executed.</param>
/// <param name="TransferCap">Maximum amount allowed for the transfer.</param>
/// <param name="Status">Initial state; null means <see cref="DisbursementStatus.Queued" />.</param>
/// <param name="User">Operator that registers the order.</param>
/// <param name="Remarks">Optional notes.</param>
public record CreateDisbursementOrderCommand(
    OpenpayReference OrderTrackingCode,
    StoreIdentifier AffiliateStoreId,
    DateInterval ExecutionSchedule,
    FinancialAmount TransferCap,
    DisbursementStatus? Status,
    string User,
    string? Remarks);
