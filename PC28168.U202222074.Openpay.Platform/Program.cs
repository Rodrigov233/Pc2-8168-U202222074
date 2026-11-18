using PC28168.U202222074.Openpay.Platform.Settlements.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using PC28168.U202222074.Openpay.Platform.Settlements.Interfaces.REST.Transform;
using PC28168.U202222074.Openpay.Platform.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Shared bounded context: controllers, localization, error handling and persistence
builder.AddLocalizedControllers()
    .AddLocalizedProblemDetails()
    .AddSharedPersistence();

// Settlements bounded context
builder.AddSettlementsContextServices();

// OpenAPI document with localized texts
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer(LocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(LocalizedOpenApiAssembler.TransformSchemaAsync);
});

var app = builder.Build();

app.ApplyDatabaseMigrations();

// Centralized exception handling (RFC 7807) and Accept-Language based localization
app.UseExceptionHandler();
app.UseAcceptLanguageLocalization();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();

app.Run();
