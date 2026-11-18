# Technical Stories

## TS001 - Create disbursement order

**As** a developer, **I want** to register a disbursement order through the RESTful API
**so that** the settlement process can transfer funds to an affiliate store.

### Scenario 1: Disbursement order created
**Given** the endpoint `/api/v1/disbursement-orders` is available
**When** a POST request is sent with valid data and a new `orderTrackingCode`
**Then** the response status is 201 Created
**And** the body includes the order with its generated `id` and `updatedAt`.

### Scenario 2: Default status
**Given** the request does not include `status`
**When** the order is created
**Then** the order status is `Queued`.

### Scenario 3: Duplicated order tracking code
**Given** an order with the same `orderTrackingCode` already exists
**When** a POST request is sent
**Then** the response status is 409 Conflict with a localized message.

### Scenario 4: Invalid execution schedule
**Given** `scheduledTo` is less than or equal to `scheduledFrom`
**When** a POST request is sent
**Then** the response status is 400 Bad Request with a localized message.

### Scenario 5: Invalid transfer cap
**Given** `transferCapAmount` is negative or `transferCapCurrencyCode` is empty
**When** a POST request is sent
**Then** the response status is 400 Bad Request with a localized message.

### Scenario 6: Invalid user or remarks
**Given** `user` is empty or longer than 20 characters, or `remarks` is longer than 85 characters
**When** a POST request is sent
**Then** the response status is 400 Bad Request with a localized message.

### Scenario 7: Localized messages
**Given** the request has the header `Accept-Language: es-PE`
**When** an error occurs
**Then** the message is returned in Spanish; otherwise it is returned in English.
