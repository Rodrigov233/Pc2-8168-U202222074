using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PC28168.U202222074.Openpay.Platform.Resources;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Converts a business rule violation raised by the domain into a localized 400 Bad Request.
/// </summary>
/// <remarks>
///     Domain exceptions keep English text for logs; the client receives the localized message that
///     matches the violated rule (identified by the parameter name of the exception).
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class ActionResultFromBusinessRuleViolationAssembler
{
    private static readonly Dictionary<string, string> MessageKeyByRule = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Value"] = "IdentifierMustNotBeEmpty",
        ["ScheduledTo"] = "ScheduledToMustBeAfterScheduledFrom",
        ["Amount"] = "TransferCapAmountMustNotBeNegative",
        ["CurrencyCode"] = "TransferCapCurrencyCodeRequired",
        ["Status"] = "StatusNotSupported",
        ["User"] = "UserRequiredWithMaxLength",
        ["Remarks"] = "RemarksTooLong"
    };

    /// <summary>
    ///     Builds the 400 response for a business rule violation.
    /// </summary>
    /// <param name="violation">Exception raised by a value object or the aggregate.</param>
    /// <param name="controller">Controller that produces the response.</param>
    /// <param name="localizer">Localizer of the response messages.</param>
    /// <returns>A 400 Bad Request with the localized explanation.</returns>
    public static ActionResult ToActionResultFromBusinessRuleViolation(
        ArgumentException violation,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer)
    {
        var key = violation.ParamName is not null && MessageKeyByRule.TryGetValue(violation.ParamName, out var found)
            ? found
            : "DisbursementOrderRequestInvalid";
        return controller.BadRequest(localizer[key].Value);
    }
}
