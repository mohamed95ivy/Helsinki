Overview
- Currency conversion API built with Clean Architecture. Includes EF Core (code-first), HttpClient to Open Exchange Rates, in-memory caching, Swagger, ProblemDetails, and rate limiting.


Requirements
- .NET SDK 9 or later
- SQLite (bundled)
- Open Exchange Rates API key


Setup and Run
- Configure the OpenExchangeRates__ApiKey via environment or configuration.
- Start the API from the Api project; database migrations are applied automatically at startup.
- Start backend and frontend using docker-compose. Set the docker-compose project as Startup project and run.


Endpoints
- POST /api/conversion — perform and persist a conversion
- GET /api/conversion/history — paged history (skip, take, sort, userId)
- GET /api/currencies — list available currency codes
- GET /api/rates/{base} — exchange rates for the base currency


Configuration
- Connection string defaults to SQLite file database.
- Enable CORS for the Angular origin if required (for example, http://localhost:4200).


Architecture
- Domain (entities), Application (interfaces and services), Infrastructure (EF Core, provider, DI), API (controllers, DTOs, middleware). Patterns: Repository, Service, Factory. SOLID and DRY principles followed.


Testing
- Unit tests with xUnit, Moq, and AutoFixture for ConversionController. Only 3 requested. Similaraly, unit tests should be implemented per project.


Assumptions and Notes
- UserId defaults to candidate when omitted.
- On free plan, provider returns USD base; cross-rates are computed.


Submission Checklist
- Builds and runs
- Swagger available
- Automatic migrations applied
- Unit tests included
- README present