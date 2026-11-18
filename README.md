# Openpay Platform - Settlements RESTful API

RESTful API that supports the settlement and funds transfer operations of the Openpay Peru ecosystem.
It manages **Disbursement Orders** in the `Settlements` bounded context.

## Features

- `POST /api/v1/disbursement-orders`: registers a disbursement order and returns `201 Created` with the
  generated `id` and `updatedAt`.
- Business rules:
  - The `orderTrackingCode` is unique (`409 Conflict` when it already exists).
  - `scheduledTo` must be later than `scheduledFrom` (`DateInterval`).
  - `transferCapAmount` must be zero or greater and `transferCapCurrencyCode` cannot be empty (`FinancialAmount`).
  - `status` is `Queued` (10, default), `Processed` (11), `Cancelled` (12) or `Failed` (13).
  - `user` is required (max. 20 characters) and `remarks` is optional (max. 85 characters).
- Validation and error messages localized in English (`en`, `en-US`) and Spanish (`es`, `es-PE`) through the
  `Accept-Language` header.
- Audit timestamps (`created_at`, `updated_at`) managed by an EF Core interceptor.
- OpenAPI documentation with Scalar at `/scalar`.

## Architecture

- Domain-Driven Design with the `Settlements` and `Shared` bounded contexts.
- Layered architecture: Domain, Application, Infrastructure and Interfaces.
- CQRS commands, Result pattern, resources and assemblers.
- `Shared` contains the reusable `DateInterval` and `FinancialAmount` value objects, the generic repository,
  unit of work, naming conventions and audit interceptor.

## Technologies

- C# 14, .NET 10, ASP.NET Core
- Entity Framework Core 10 with MySQL (`MySql.EntityFrameworkCore`)
- Microsoft.AspNetCore.OpenApi and Scalar

## Database

MySQL schema `openpay_settlements`, table `disbursement_orders`. Migrations are applied on startup.
Update the connection string in `appsettings.Development.json` if needed.

## Run

```bash
cd PC28168.U202222074.Openpay.Platform
dotnet run --launch-profile http
```

Then open `http://localhost:5128/scalar`.

## Author

- **Rodrigo Velasquez Velasquez** - U202222074 - NRC 8168
