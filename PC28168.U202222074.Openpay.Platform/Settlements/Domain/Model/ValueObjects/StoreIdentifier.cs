namespace PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Reference to the affiliate store that receives the funds; it comes from another bounded context.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="Value">Non-empty UUID.</param>
/// <exception cref="ArgumentException">Raised when <paramref name="Value" /> is <see cref="Guid.Empty" />.</exception>
public sealed record StoreIdentifier(Guid Value)
{
    /// <summary>
    ///     Non-empty UUID held by this reference.
    /// </summary>
    public Guid Value { get; } = Value == Guid.Empty
        ? throw new ArgumentException("StoreIdentifier requires a non-empty UUID.", nameof(Value))
        : Value;

    /// <inheritdoc />
    public override string ToString() => Value.ToString("D");
}
