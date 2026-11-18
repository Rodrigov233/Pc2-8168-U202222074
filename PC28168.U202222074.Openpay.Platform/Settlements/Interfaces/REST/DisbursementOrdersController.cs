using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PC28168.U202222074.Openpay.Platform.Resources;
using PC28168.U202222074.Openpay.Platform.Settlements.Application.Services;
using PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Transform;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST;

/// <summary>
///     REST entry point for disbursement orders (/api/v1/disbursement-orders).
/// </summary>
/// <remarks>
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
/// <param name="commandService">Command service of disbursement orders.</param>
/// <param name="localizer">Localizer of the response messages.</param>
/// <param name="logger">Logger of this controller.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Disbursement Orders")]
public class DisbursementOrdersController(
    IDisbursementOrderCommandService commandService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<DisbursementOrdersController> logger) : ControllerBase
{
    /// <summary>
    ///     Registers a disbursement order.
    /// </summary>
    /// <param name="resource">Data of the order to register.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    ///     201 with the stored order; 400 when the request breaks a rule; 409 when the order tracking code
    ///     is already registered; 500 when the order cannot be stored.
    /// </returns>
    [HttpPost]
    [ProducesResponseType<DisbursementOrderResource>(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateDisbursementOrder([FromBody] CreateDisbursementOrderResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateDisbursementOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command, cancellationToken);
            return ActionResultFromCreateDisbursementOrderResultAssembler
                .ToActionResultFromCreateDisbursementOrderResult(result, this, localizer);
        }
        catch (ArgumentException violation)
        {
            logger.LogInformation("Disbursement order request rejected: {Reason}", violation.Message);
            return ActionResultFromBusinessRuleViolationAssembler
                .ToActionResultFromBusinessRuleViolation(violation, this, localizer);
        }
    }
}
