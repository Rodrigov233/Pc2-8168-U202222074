using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U202222074.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Translates the request resource into the domain command, building every value object.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class CreateDisbursementOrderCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a <see cref="CreateDisbursementOrderCommand" /> from the validated request.
    /// </summary>
    /// <param name="resource">Request already checked by data annotations.</param>
    /// <returns>The command with its value objects.</returns>
    /// <exception cref="ArgumentException">Raised when a value object rejects its data or the status is unknown.</exception>
    public static CreateDisbursementOrderCommand ToCommandFromResource(CreateDisbursementOrderResource resource)
    {
        var schedule = new DateInterval(resource.ScheduledFrom!.Value, resource.ScheduledTo!.Value);
        var transferCap = new FinancialAmount(resource.TransferCapAmount!.Value, resource.TransferCapCurrencyCode!);

        return new CreateDisbursementOrderCommand(
            new OpenpayReference(resource.OrderTrackingCode!.Value),
            new StoreIdentifier(resource.AffiliateStoreId!.Value),
            schedule,
            transferCap,
            ParseStatus(resource.Status),
            resource.User!,
            resource.Remarks);
    }

    /// <summary>
    ///     Reads the status name of the request, ignoring letter case.
    /// </summary>
    /// <param name="statusName">Status name or null.</param>
    /// <returns>The status, or null when the request does not send one.</returns>
    /// <exception cref="ArgumentException">Raised when the name is not a known status.</exception>
    private static DisbursementStatus? ParseStatus(string? statusName)
    {
        if (string.IsNullOrWhiteSpace(statusName)) return null;

        var known = Enum.GetNames<DisbursementStatus>()
            .FirstOrDefault(name => name.Equals(statusName.Trim(), StringComparison.OrdinalIgnoreCase));

        return known is null
            ? throw new ArgumentException($"Unknown disbursement status '{statusName}'.", "Status")
            : Enum.Parse<DisbursementStatus>(known);
    }
}
