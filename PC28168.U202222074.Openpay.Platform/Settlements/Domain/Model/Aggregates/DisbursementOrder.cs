using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202222074.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root for an order that moves funds from Openpay to an affiliate store.
/// </summary>
/// <remarks>
///     Guards the operator and remarks rules; the uniqueness of the tracking code is checked by the
///     application layer and by a unique index.
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public partial class DisbursementOrder
{
    /// <summary>Longest operator identifier allowed.</summary>
    public const int MaxUserLength = 20;

    /// <summary>Longest remarks text allowed.</summary>
    public const int MaxRemarksLength = 85;

    /// <summary>
    ///     Constructor reserved for Entity Framework Core materialization.
    /// </summary>
    protected DisbursementOrder()
    {
        OrderTrackingCode = null!;
        AffiliateStoreId = null!;
        ExecutionSchedule = null!;
        TransferCap = null!;
        User = null!;
    }

    /// <summary>
    ///     Registers a disbursement order from a <see cref="CreateDisbursementOrderCommand" />.
    /// </summary>
    /// <param name="command">Data of the new order.</param>
    /// <exception cref="ArgumentNullException">Raised when the command or a value object is missing.</exception>
    /// <exception cref="ArgumentException">Raised when the status, operator or remarks break a rule.</exception>
    public DisbursementOrder(CreateDisbursementOrderCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        OrderTrackingCode = command.OrderTrackingCode
                            ?? throw new ArgumentNullException(nameof(command.OrderTrackingCode));
        AffiliateStoreId = command.AffiliateStoreId
                           ?? throw new ArgumentNullException(nameof(command.AffiliateStoreId));
        ExecutionSchedule = command.ExecutionSchedule
                            ?? throw new ArgumentNullException(nameof(command.ExecutionSchedule));
        TransferCap = command.TransferCap
                      ?? throw new ArgumentNullException(nameof(command.TransferCap));
        Status = EnsureKnownStatus(command.Status ?? DisbursementStatus.Queued);
        User = EnsureValidUser(command.User);
        Remarks = NormalizeRemarks(command.Remarks);
    }

    /// <summary>Database identity of the order.</summary>
    public int Id { get; private set; }

    /// <summary>Openpay reference of the order (unique).</summary>
    public OpenpayReference OrderTrackingCode { get; private set; }

    /// <summary>Affiliate store that receives the funds.</summary>
    public StoreIdentifier AffiliateStoreId { get; private set; }

    /// <summary>Period in which the order may be executed.</summary>
    public DateInterval ExecutionSchedule { get; private set; }

    /// <summary>Maximum amount allowed for the transfer.</summary>
    public FinancialAmount TransferCap { get; private set; }

    /// <summary>Current lifecycle state.</summary>
    public DisbursementStatus Status { get; private set; }

    /// <summary>Operator that registered the order.</summary>
    public string User { get; private set; }

    /// <summary>Optional notes about the order.</summary>
    public string? Remarks { get; private set; }

    /// <summary>
    ///     Tells whether the execution period of the order is already over.
    /// </summary>
    /// <param name="referenceDate">Day used to evaluate the period.</param>
    /// <returns><c>true</c> when the execution schedule has elapsed.</returns>
    public bool IsScheduleElapsed(DateOnly referenceDate) => ExecutionSchedule.HasElapsed(referenceDate);

    /// <summary>
    ///     Rejects numeric values that are not part of <see cref="DisbursementStatus" />.
    /// </summary>
    /// <param name="status">State to check.</param>
    /// <returns>The same state when it is defined.</returns>
    private static DisbursementStatus EnsureKnownStatus(DisbursementStatus status) =>
        Enum.IsDefined(status)
            ? status
            : throw new ArgumentException("The disbursement status is not supported.", nameof(Status));

    /// <summary>
    ///     Applies the operator rule: required and at most 20 characters.
    /// </summary>
    /// <param name="user">Operator identifier.</param>
    /// <returns>The operator identifier without surrounding blanks.</returns>
    private static string EnsureValidUser(string? user)
    {
        var value = user?.Trim();
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("The operator user is mandatory.", nameof(User));
        if (value.Length > MaxUserLength)
            throw new ArgumentException($"The operator user exceeds {MaxUserLength} characters.", nameof(User));
        return value;
    }

    /// <summary>
    ///     Applies the remarks rule: optional and at most 85 characters.
    /// </summary>
    /// <param name="remarks">Remarks text.</param>
    /// <returns>The remarks without surrounding blanks, or null when there is no text.</returns>
    private static string? NormalizeRemarks(string? remarks)
    {
        var value = remarks?.Trim();
        if (string.IsNullOrEmpty(value)) return null;
        return value.Length <= MaxRemarksLength
            ? value
            : throw new ArgumentException($"The remarks exceed {MaxRemarksLength} characters.", nameof(Remarks));
    }
}
