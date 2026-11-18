# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-11-18

### Added

- Shared bounded context with audit interceptor, generic repository, unit of work, Result pattern,
  snake_case and kebab-case conventions.
- `DateInterval` and `FinancialAmount` value objects in the Shared bounded context.
- `DisbursementOrder` aggregate in the Settlements bounded context with `OpenpayReference`, `StoreIdentifier`
  value objects and `DisbursementStatus` enumeration.
- `POST /api/v1/disbursement-orders` endpoint with 201, 400, 409 and 500 responses.
- Localization of responses and OpenAPI metadata in English and Spanish.
- Initial migration for the `disbursement_orders` table.
