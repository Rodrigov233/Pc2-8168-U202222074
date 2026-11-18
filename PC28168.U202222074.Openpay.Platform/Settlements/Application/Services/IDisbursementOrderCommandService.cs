using PC28168.U202222074.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202222074.Openpay.Platform.Shared.Application.Patterns;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Application.Services;

/// <summary>
///     Write-side use cases of the disbursement orders (CQRS command side).
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public interface IDisbursementOrderCommandService
{
    /// <summary>
    ///     Registers a new disbursement order.
    /// </summary>
    /// <param name="command">Data of the order to register.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    ///     <see cref="Result{TValue,TError}.Success" /> with the stored order, or
    ///     <see cref="Result{TValue,TError}.Failure" /> with the reason it was rejected.
    /// </returns>
    /// <exception cref="ArgumentException">Raised by the aggregate when a business rule is broken.</exception>
    Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(CreateDisbursementOrderCommand command,
        CancellationToken cancellationToken = default);
}
