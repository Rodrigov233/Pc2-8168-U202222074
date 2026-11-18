namespace PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Reference that identifies a disbursement order across Openpay; it is issued by another bounded context.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="Value">Non-empty UUID.</param>
/// <exception cref="ArgumentException">Raised when <paramref name="Value" /> is <see cref="Guid.Empty" />.</exception>
public sealed record OpenpayReference(Guid Value)
{
    /// <summary>
    ///     Non-empty UUID held by this reference.
    /// </summary>
    public Guid Value { get; } = Value == Guid.Empty
        ? throw new ArgumentException("OpenpayReference requires a non-empty UUID.", nameof(Value))
        : Value;

    /// <inheritdoc />
    public override string ToString() => Value.ToString("D");
}
