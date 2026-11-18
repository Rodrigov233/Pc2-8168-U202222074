using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PC28168.U202222074.Openpay.Platform.Resources;
using PC28168.U202222074.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202222074.Openpay.Platform.Shared.Application.Patterns;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Converts the outcome of the create disbursement order use case into an HTTP response.
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class ActionResultFromCreateDisbursementOrderResultAssembler
{
    /// <summary>
    ///     Builds the HTTP response for the application result.
    /// </summary>
    /// <param name="result">Result returned by the command service.</param>
    /// <param name="controller">Controller that produces the response.</param>
    /// <param name="localizer">Localizer of the response messages.</param>
    /// <returns>201 Created, 409 Conflict or 500 Internal Server Error.</returns>
    public static ActionResult ToActionResultFromCreateDisbursementOrderResult(
        Result<DisbursementOrder, CreateDisbursementOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer)
    {
        if (result is Result<DisbursementOrder, CreateDisbursementOrderError>.Success created)
            return controller.StatusCode(StatusCodes.Status201Created,
                DisbursementOrderResourceFromEntityAssembler.ToResourceFromEntity(created.Value));

        var error = (result as Result<DisbursementOrder, CreateDisbursementOrderError>.Failure)?.Error;
        if (error == CreateDisbursementOrderError.OrderTrackingCodeAlreadyExists)
            return controller.Conflict(localizer["OrderTrackingCodeAlreadyExists"].Value);

        return controller.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: localizer["UnexpectedServerError"].Value,
            detail: localizer["DisbursementOrderNotSaved"].Value);
    }
}
