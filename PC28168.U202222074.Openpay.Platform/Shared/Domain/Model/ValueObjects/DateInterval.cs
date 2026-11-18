namespace PC28168.U202222074.Openpay.Platform.Shared.Domain.Model.ValueObjects;

/// <summary>
///     Immutable period of calendar days bounded by a start date and an end date.
/// </summary>
/// <remarks>
///     The end date (ScheduledTo) has to be after the start date (ScheduledFrom).
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="ScheduledFrom">Start date of the period.</param>
/// <param name="ScheduledTo">End date of the period, after <paramref name="ScheduledFrom" />.</param>
/// <exception cref="ArgumentException">Raised when ScheduledTo is on or before ScheduledFrom.</exception>
public sealed record DateInterval(DateOnly ScheduledFrom, DateOnly ScheduledTo)
{
    /// <summary>
    ///     Start date of the period.
    /// </summary>
    public DateOnly ScheduledFrom { get; } = ScheduledFrom;

    /// <summary>
    ///     End date of the period. Validated against <see cref="ScheduledFrom" /> when the interval is built.
    /// </summary>
    public DateOnly ScheduledTo { get; } = ScheduledTo > ScheduledFrom
        ? ScheduledTo
        : throw new ArgumentException(
            "The date interval is invalid: ScheduledTo must be after ScheduledFrom.", nameof(ScheduledTo));

    /// <summary>
    ///     Indicates whether the period is already over for a given day.
    /// </summary>
    /// <param name="referenceDate">Day used to evaluate the period.</param>
    /// <returns><c>true</c> when <paramref name="referenceDate" /> comes after <see cref="ScheduledTo" />; otherwise <c>false</c>.</returns>
    public bool HasElapsed(DateOnly referenceDate) => referenceDate.CompareTo(ScheduledTo) > 0;
}
