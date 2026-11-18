using Microsoft.EntityFrameworkCore;
using PC28168.U202222074.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U202222074.Openpay.Platform.Settlements.Application.Services;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U202222074.Openpay.Platform.Shared.Application.Patterns;
using PC28168.U202222074.Openpay.Platform.Shared.Domain.Repositories;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Application.Internal.CommandServices;

/// <summary>
///     Handles the commands that change disbursement orders.
/// </summary>
/// <remarks>
///     Rule 1 (unique order tracking code) is checked before building the aggregate and again by the
///     unique index when saving, in case two requests arrive at the same time.
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="repository">Disbursement order repository.</param>
/// <param name="unitOfWork">Unit of work that commits the changes.</param>
/// <param name="logger">Logger of this service.</param>
public class DisbursementOrderCommandService(
    IDisbursementOrderRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<DisbursementOrderCommandService> logger) : IDisbursementOrderCommandService
{
    private const int MySqlDuplicateEntryCode = 1062;

    /// <inheritdoc />
    public async Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(
        CreateDisbursementOrderCommand command, CancellationToken cancellationToken = default)
    {
        var current = await repository.FindByOrderTrackingCodeAsync(command.OrderTrackingCode, cancellationToken);
        if (current is not null)
            return Reject(CreateDisbursementOrderError.OrderTrackingCodeAlreadyExists, command);

        var order = new DisbursementOrder(command);

        try
        {
            await repository.AddAsync(order, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            var error = IsDuplicateEntry(exception)
                ? CreateDisbursementOrderError.OrderTrackingCodeAlreadyExists
                : CreateDisbursementOrderError.PersistenceFailure;
            logger.LogError(exception, "Saving disbursement order {OrderTrackingCode} failed with {Error}",
                command.OrderTrackingCode, error);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(error);
        }

        logger.LogInformation("Disbursement order {Id} registered with tracking code {OrderTrackingCode}",
            order.Id, order.OrderTrackingCode);
        return new Result<DisbursementOrder, CreateDisbursementOrderError>.Success(order);
    }

    /// <summary>
    ///     Builds a failure result and leaves a warning in the log.
    /// </summary>
    /// <param name="error">Reason of the rejection.</param>
    /// <param name="command">Rejected command.</param>
    /// <returns>The failure result.</returns>
    private Result<DisbursementOrder, CreateDisbursementOrderError> Reject(CreateDisbursementOrderError error,
        CreateDisbursementOrderCommand command)
    {
        logger.LogWarning("Disbursement order {OrderTrackingCode} rejected: {Error}", command.OrderTrackingCode,
            error);
        return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(error);
    }

    /// <summary>
    ///     Detects a MySQL duplicate entry error inside the exception chain.
    /// </summary>
    /// <param name="exception">Exception thrown while saving.</param>
    /// <returns><c>true</c> when the cause is a duplicate entry (MySQL error 1062).</returns>
    private static bool IsDuplicateEntry(Exception exception)
    {
        for (var inner = exception; inner is not null; inner = inner.InnerException)
        {
            if (inner.GetType().Name != "MySqlException") continue;
            if (inner.GetType().GetProperty("Number")?.GetValue(inner) is MySqlDuplicateEntryCode) return true;
        }

        return false;
    }
}
