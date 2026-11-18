using System.Globalization;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Resources;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Flattens a <see cref="DisbursementOrder" /> aggregate into the response resource.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class DisbursementOrderResourceFromEntityAssembler
{
    /// <summary>
    ///     Builds the response resource of a stored order.
    /// </summary>
    /// <param name="entity">Stored aggregate.</param>
    /// <returns>The resource with value objects as primitive values.</returns>
    public static DisbursementOrderResource ToResourceFromEntity(DisbursementOrder entity)
    {
        var schedule = entity.ExecutionSchedule;
        return new DisbursementOrderResource(
            Id: entity.Id,
            OrderTrackingCode: entity.OrderTrackingCode.ToString(),
            AffiliateStoreId: entity.AffiliateStoreId.ToString(),
            ScheduledFrom: schedule.ScheduledFrom.ToString("O", CultureInfo.InvariantCulture),
            ScheduledTo: schedule.ScheduledTo.ToString("O", CultureInfo.InvariantCulture),
            TransferCapAmount: entity.TransferCap.Amount,
            TransferCapCurrencyCode: entity.TransferCap.CurrencyCode,
            Status: entity.Status.ToString(),
            User: entity.User,
            Remarks: entity.Remarks,
            UpdatedAt: entity.UpdatedAt);
    }
}
