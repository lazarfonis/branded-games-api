# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Active Work

This project is being brought into compliance with the "Softverski alati" course spec. **Read
[`IMPLEMENTATION_PLAN.md`](IMPLEMENTATION_PLAN.md) at the start of each session** — it is the living
progress tracker (requirement scoreboard, phased CRUD/test/doc tasks, and a decisions log). Update
its checkboxes as work lands.

## Build & Run

```bash
dotnet build                                    # Build all projects
dotnet run --project BrandedGames.Api           # Start dev server (https://localhost:7055)
dotnet run --project BrandedGames.Api --launch-profile http  # HTTP only (http://localhost:5084)
```

Swagger UI is available at `/swagger` when `showSwagger` is enabled in `appsettings.Development.json` (it is also the default launch URL).

### Database Migrations

```bash
# From solution root
dotnet ef migrations add <MigrationName> --project BrandedGames.Data -s BrandedGames.Api
dotnet ef database update --project BrandedGames.Data -s BrandedGames.Api
```

The database is PostgreSQL (Npgsql). `Database.Migrate()` runs in the `BrandedGamesDbContext` constructor, so pending migrations are applied automatically on startup. The `BrandedGamesDb` connection string lives in `appsettings.Development.json` (git-ignored).

## No Tests

There are no test projects in this solution.

## Solution Structure

```
BrandedGames.Api        → Controllers, middleware, auth handlers, MapperConfig.cs
BrandedGames.Core       → Managers (business logic + data access)
BrandedGames.Data       → BrandedGamesDbContext, EF migrations
BrandedGames.Entities   → Domain entities (implement IEntity), Identity models
BrandedGames.Common     → ViewModels, custom exceptions, ValidationHelper, enums
```

Dependency direction: `Api → Core → Data → Entities`, all layers → `Common`.

## Architecture

This project uses a **thin-controller → Manager** pattern (not the repository pattern):

- **Controllers** (`/Api/Controllers/`) delegate immediately to manager classes. No business logic.
- **Managers** (`/Core/*Manager.cs`) own all business logic and query `BrandedGamesDbContext` directly via constructor injection.
- There is no repository layer. Managers talk to EF Core directly.
- All managers inject `BrandedGamesDbContext` and `IMapper`.

### Key Classes

| Class | Location | Purpose |
|-------|----------|---------|
| `BrandedGamesDbContext` | `Data/` | Main EF context; applies migrations in its constructor and auto-sets `CreatedAt`/`ModifiedAt` via `PopulateEntityFields()` in `SaveChanges`/`SaveChangesAsync` |
| `BaseController` | `Api/Controllers/` | Provides `GetCurrentUserId()` helper |
| `ValidationHelper` | `Common/Helpers/` | `MustExist<T>()` throws `NotFoundException`; `MustNotExist<T>()` throws `ValidationException` |
| `MapperConfig` | `Api/Helpers/MapperConfig.cs` | All AutoMapper profiles — add mappings for new ViewModels here |
| `CloudinaryFileManager` | `Core/` | Handles file uploads to Cloudinary |

### Entities

All entities implement `IEntity` (provides `CreatedAt`, `ModifiedAt`). Identity models are in `Entities/Identity/` (`User` extends `IdentityUser<Guid>`, `Role` extends `IdentityRole<Guid>`).

### Authentication & Authorization

JWT Bearer auth configured in `Program.cs`. Four authorization policies are defined (names in `Api/Authentication/Policies.cs`):
- `NotConfirmedEmail`, `EmailConfirmed`, `RegisteredUser`, `AdministratorUser`

Custom handlers live in `Api/Authentication/`.

### ViewModels

Organized under `Common/Models/`. Convention: `{Name}Model`, `{Name}CreateModel`, `{Name}UpdateModel`.

## Adding New Features

When adding a new entity/endpoint:
1. Add entity to `BrandedGames.Entities/` (implement `IEntity`)
2. Add `DbSet<T>` to `BrandedGamesDbContext`
3. Add ViewModels to `BrandedGames.Common/Models/`
4. Add AutoMapper mappings to `Api/Helpers/MapperConfig.cs`
5. Add Manager class to `BrandedGames.Core/`
6. Register manager in `Program.cs` DI (`AddScoped`)
7. Add Controller to `BrandedGames.Api/Controllers/`
8. Run `dotnet ef migrations add` to create the migration

## Coding Standards

- C# with nullable reference types; prefer `async`/`await` for all I/O and database access.
- Keep controllers thin — no business logic, no direct `DbContext` access. Delegate to a manager.
- Use `ValidationHelper.MustExist`/`MustNotExist` for existence checks; throw the typed exceptions in `Common/Exceptions/` rather than returning ad-hoc error responses.
- Never hardcode secrets or connection strings — they belong in `appsettings.Development.json` (git-ignored).
- Use parameterized EF Core queries; never build SQL by string concatenation.
- Prefer `AsNoTracking()` for read-only queries and avoid N+1 access patterns (use `Include`/projection).
