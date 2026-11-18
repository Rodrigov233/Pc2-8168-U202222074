namespace PC28168.U202222074.Openpay.Platform.Shared.Domain.Model.ValueObjects;

/// <summary>
///     Immutable money value made of a non-negative amount and a currency code.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="Amount">Money amount, zero or positive.</param>
/// <param name="CurrencyCode">Currency code, with text.</param>
/// <exception cref="ArgumentOutOfRangeException">Raised when <paramref name="Amount" /> is below zero.</exception>
/// <exception cref="ArgumentNullException">Raised when <paramref name="CurrencyCode" /> is null.</exception>
/// <exception cref="ArgumentException">Raised when <paramref name="CurrencyCode" /> is empty or blank.</exception>
public sealed record FinancialAmount(decimal Amount, string CurrencyCode)
{
    /// <summary>
    ///     Money amount, never below zero.
    /// </summary>
    public decimal Amount { get; } = Amount >= decimal.Zero
        ? Amount
        : throw new ArgumentOutOfRangeException(nameof(Amount), Amount,
            "The financial amount cannot be negative.");

    /// <summary>
    ///     Currency code of the amount.
    /// </summary>
    public string CurrencyCode { get; } = RequireCurrencyCode(CurrencyCode);

    /// <summary>
    ///     Builds a new amount by multiplying this one by a factor, keeping the currency.
    /// </summary>
    /// <param name="factor">Multiplier to apply.</param>
    /// <returns>The resulting <see cref="FinancialAmount" /> in the same currency.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised when the result is negative.</exception>
    public FinancialAmount MultiplyBy(decimal factor) => new(Amount * factor, CurrencyCode);

    /// <summary>
    ///     Checks that the currency code has text.
    /// </summary>
    /// <param name="currencyCode">Currency code to check.</param>
    /// <returns>The currency code without surrounding blanks.</returns>
    private static string RequireCurrencyCode(string currencyCode)
    {
        ArgumentNullException.ThrowIfNull(currencyCode, nameof(CurrencyCode));
        return string.IsNullOrWhiteSpace(currencyCode)
            ? throw new ArgumentException("The currency code cannot be blank.", nameof(CurrencyCode))
            : currencyCode.Trim();
    }
}
