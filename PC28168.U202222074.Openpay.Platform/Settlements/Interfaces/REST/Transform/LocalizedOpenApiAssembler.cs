using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
using PC28168.U202222074.Openpay.Platform.Resources;
using PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Resources;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Fills the OpenAPI document of the Settlements endpoints with texts from the shared resources.
/// </summary>
/// <remarks>
///     Resource keys follow a naming convention, so a new operation only needs its texts in the .resx files:
///     {Action}Summary, {Action}Description, {Action}Response{StatusCode}, {Resource}Schema and
///     {Property}PropertyDescription.
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public static class LocalizedOpenApiAssembler
{
    private static readonly HashSet<Type> DocumentedResources =
        [typeof(CreateDisbursementOrderResource), typeof(DisbursementOrderResource)];

    /// <summary>
    ///     Sets the summary, description and response texts of the Settlements operations.
    /// </summary>
    /// <param name="operation">Operation being documented.</param>
    /// <param name="context">Transformer context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A completed task.</returns>
    public static Task TransformOperationAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor
            {
                ControllerTypeInfo.Namespace: { } controllerNamespace
            } action || !controllerNamespace.Contains(".Settlements."))
            return Task.CompletedTask;

        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<SharedResource>>();
        var name = action.ActionName;
        operation.OperationId = name;
        operation.Summary = Text(localizer, $"{name}Summary") ?? operation.Summary;
        operation.Description = Text(localizer, $"{name}Description") ?? operation.Description;

        foreach (var (statusCode, response) in operation.Responses ?? [])
            if (response is OpenApiResponse openApiResponse &&
                Text(localizer, $"{name}Response{statusCode}") is { } description)
                openApiResponse.Description = description;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Sets the description of the Settlements resource schemas and of their properties.
    /// </summary>
    /// <param name="schema">Schema being documented.</param>
    /// <param name="context">Transformer context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A completed task.</returns>
    public static Task TransformSchemaAsync(OpenApiSchema schema,
        OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<SharedResource>>();

        if (context.JsonPropertyInfo is { } property &&
            DocumentedResources.Contains(property.DeclaringType) &&
            Text(localizer, $"{char.ToUpperInvariant(property.Name[0])}{property.Name[1..]}PropertyDescription")
                is { } propertyText)
            schema.Description = propertyText;
        else if (DocumentedResources.Contains(context.JsonTypeInfo.Type) &&
                 Text(localizer, $"{context.JsonTypeInfo.Type.Name}Schema") is { } schemaText)
            schema.Description = schemaText;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Reads a localized text, returning null when the key is missing.
    /// </summary>
    /// <param name="localizer">Shared resources localizer.</param>
    /// <param name="key">Resource key.</param>
    /// <returns>The localized text, or null.</returns>
    private static string? Text(IStringLocalizer localizer, string key)
    {
        var text = localizer[key];
        return text.ResourceNotFound ? null : text.Value;
    }
}
