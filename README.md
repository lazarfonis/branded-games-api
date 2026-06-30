# Branded Games API

A .NET 8 Web API for configuring **branded games**. Customers (influencers, brands, individuals)
submit *game forms* describing a game built on a chosen game type, with selected features, target
platforms and uploaded media assets. The API exposes full CRUD over game types, features, platform
types and game forms, with JWT-based authentication and file uploads to Cloudinary.

## Tech stack

- **.NET 8** Web API (ASP.NET Core)
- **Entity Framework Core 8** with **SQL Server**
- **ASP.NET Core Identity** + JWT Bearer authentication
- **AutoMapper** for entity ↔ view-model mapping
- **Cloudinary** for media storage
- **NLog** for logging, **Swashbuckle/Swagger** for API docs
- **xUnit** test suite (`BrandedGames.Tests`)

## Solution structure

```
BrandedGames.Api        → Controllers, middleware, auth handlers
BrandedGames.Core       → Managers (business logic + data access), MapperConfig
BrandedGames.Data       → BrandedGamesDbContext, EF migrations
BrandedGames.Entities   → Domain entities (implement IEntity), Identity models
BrandedGames.Common     → ViewModels, custom exceptions, ValidationHelper, enums
BrandedGames.Tests      → xUnit unit tests
```

Dependency direction: `Api → Core → Data → Entities`, all layers → `Common`. The app uses a
thin-controller → Manager pattern: controllers delegate to managers, which own the business logic
and query `BrandedGamesDbContext` directly (no repository layer).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A SQL Server instance (e.g. SQL Server Express or LocalDB)
- A `BrandedGames.Api/appsettings.Development.json` file (git-ignored) with the local
  configuration — at minimum a `BrandedGamesDb` connection string, plus Cloudinary and JWT settings.

## Build & run

```bash
dotnet build                                                  # Build all projects
dotnet run --project BrandedGames.Api                         # HTTPS dev server (https://localhost:7055)
dotnet run --project BrandedGames.Api --launch-profile http   # HTTP only (http://localhost:5084)
```

The database is created/migrated automatically on startup — `Database.Migrate()` runs in the
`BrandedGamesDbContext` constructor, so pending migrations are applied on the first request. No
manual database restore is required.

Swagger UI is served at `/swagger` when `showSwagger` is enabled in
`appsettings.Development.json` (it is also the default launch URL).

## Database migrations

```bash
# From the solution root
dotnet ef migrations add <MigrationName> --project BrandedGames.Data -s BrandedGames.Api
dotnet ef database update --project BrandedGames.Data -s BrandedGames.Api
```

## Tests

```bash
dotnet test                                  # Run the full xUnit suite
dotnet test --filter FullyQualifiedName~GameFormManagerTests   # Run a subset
```

Tests use the EF Core in-memory provider (see `BrandedGames.Tests/Infrastructure/TestDbContextFactory.cs`),
so no real database is needed.

## Authentication & authorization

JWT Bearer authentication is configured in `Program.cs`. Authorization policies are defined in
`BrandedGames.Api/Authentication/Policies.cs`: `NotConfirmedEmail`, `EmailConfirmed`,
`RegisteredUser`, and `AdministratorUser`.

## Documentation

- [`docs/class-diagram.md`](docs/class-diagram.md) — domain class diagram (Mermaid).
- [`docs/system-operations.md`](docs/system-operations.md) — list of system operations (HTTP verb,
  route, request/response model, backing manager method).
