# Implementation Plan & Progress Tracker

> **Purpose:** This is the working checklist for bringing BrandedGames API into compliance with the
> "Softverski alati" course specification. Claude reads this at the start of every session to know
> what is done and what is next. Keep statuses up to date as work lands.
>
> **Status legend:** `[ ]` todo · `[~]` in progress · `[x]` done

## Track: C#/.NET (substitution rules)

The spec is written for Java/Maven. We are on the approved alternative track, so:

| Spec (Java) | Our equivalent | Already satisfied? |
|---|---|---|
| Maven | .NET SDK + NuGet (`PackageReference`) | ✅ yes |
| JUnit (Jupiter) | **xUnit** | ❌ to build |
| Javadoc | XML doc comments (`/// <summary>`) | ❌ to write |

## Requirement scoreboard

| # | Requirement | Target | Status | Notes |
|---|---|---|---|---|
| 1 | Domain classes | ≥ 8 | ✅ Met | 8 game entities + Identity (see below) |
| 2 | System operations | ≥ 12 | ✅ Met (19) | Full CRUD implemented (Phase 1) |
| 3 | Class diagram artifact | submitted | ❌ | Produce before submission |
| 4 | System-operations list artifact | submitted | ❌ | Produce before submission |
| 5 | Git uses own email | — | ✅ Met | `lazarst.pn@gmail.com` |
| 6 | Visible history | — | ⚠️ Weak | Only 3 commits; grows as we work |
| 7 | Branch create **and merge** | mandatory | ❌ | Do all phases on feature branches, `--no-ff` merge |
| 8 | Tags | mandatory | ❌ | Tag `v1.0` at the end |
| 9 | Hosted (GitHub) | — | ✅ Met | `origin/main` exists |
| 10 | Build via NuGet | mandatory | ✅ Met | SDK-style projects |
| 11 | All domain classes tested | mandatory | ❌ | xUnit project |
| 12 | All system operations tested | mandatory | ❌ | xUnit project |
| 13 | All documented (XML docs) | mandatory | ✅ Met | Domain classes + all operations (Phase 2); infra intentionally scoped out |
| 14 | JSON functionality | mandatory | ✅ Met (weak) | API returns JSON; Cloudinary JSON call. Optional hardening below. |
| 15 | Other course tech | optional | ✅ Bonus | JWT, AutoMapper, NLog, Swagger, Docker |

## Current state snapshot (baseline)

- **Solution:** .NET 8, 5 projects — `Api → Core → Data → Entities`, all → `Common`. PostgreSQL/EF Core.
- **Domain classes (8+):** `GameForm`, `GameFeature`, `GameType`, `PlatformType`, `GameFormFeature`,
  `GameFormPlatformType`, `GameFormFile`, `User` (+ Identity: `Role`, `UserRole`, etc.).
- **System operations today (4):**
  - `GET /api/features` → `FeatureManager.GetFeatures()`
  - `GET /api/game-types` → `GameTypeManager.GetTypes()`
  - `GET /api/platform-types` → `PlatformTypeManager.GetPlatforms()`
  - `POST /api/customer-games` → `GameFormManager.Create()`
- **Tests:** none. **XML docs:** none (only 1 helper file). `GenerateDocumentationFile=true` only in `.Api`.

---

## Phase 1 — System operations: 4 → ≥12  (branch: `feature/crud-operations`)  ✅ DONE (committed + merged to `main` via `--no-ff`)

Strategy: **full CRUD on existing entities** (no new domain). **19 operations total** — clears the ≥12 bar.
"Existing" = already implemented, leave as-is. Build verified clean (0 errors).

### Feature (`GameFeature`)
- [x] `GetFeatures` — `GET /api/features` *(existing)*
- [x] `GetFeature(id)` — `GET /api/features/{id}`
- [x] `CreateFeature(FeatureCreateModel)` — `POST /api/features`
- [x] `UpdateFeature(id, FeatureUpdateModel)` — `PUT /api/features/{id}`
- [x] `DeleteFeature(id)` — `DELETE /api/features/{id}`

### GameType
- [x] `GetTypes` — `GET /api/game-types` *(existing; renamed action from mis-named `CreateGame`)*
- [x] `GetType(id)` — `GET /api/game-types/{id}`
- [x] `CreateType(GameTypeCreateModel)` — `POST /api/game-types`
- [x] `UpdateType(id, GameTypeUpdateModel)` — `PUT /api/game-types/{id}`
- [x] `DeleteType(id)` — `DELETE /api/game-types/{id}`

### PlatformType
- [x] `GetPlatforms` — `GET /api/platform-types` *(existing)*
- [x] `GetPlatform(id)` — `GET /api/platform-types/{id}`
- [x] `CreatePlatform(PlatformTypeCreateModel)` — `POST /api/platform-types`
- [x] `UpdatePlatform(id, PlatformTypeUpdateModel)` — `PUT /api/platform-types/{id}`
- [x] `DeletePlatform(id)` — `DELETE /api/platform-types/{id}`

### GameForm
- [x] `Create` — `POST /api/customer-games` *(existing)*
- [x] `GetGames()` — `GET /api/customer-games`
- [x] `GetGame(id)` — `GET /api/customer-games/{id}`
- [x] `DeleteGame(id)` — `DELETE /api/customer-games/{id}`
- Note: GameForm `Update` deliberately omitted (file upload + transaction complexity); not needed for the count.

### Per-operation checklist (apply the project's "Adding New Features" steps)
- [x] ViewModels in `Common/Models/` — added Create/Update for Feature, GameType, PlatformType; read models `GameFormModel` + `GameFormFileModel`
- [x] AutoMapper mappings in `Api/Helpers/MapperConfig.cs` (read + write maps, incl. nested GameForm projection)
- [x] Manager methods in `Core/*Manager.cs` (`ValidationHelper.MustExist`, async)
- [x] Controller actions with `[Http*]` + `[ProducesResponseType]`
- [x] No migration needed (CRUD on existing schema)

### Phase 1 still to do
- [x] Commit on `feature/crud-operations`, merge to `main` with `--no-ff`, push both branches
- [ ] Functional verification deferred to Phase 3 tests (local run needs Postgres + git-ignored `appsettings.Development.json`)

---

## Phase 2 — XML documentation  (branch: `feature/xml-docs`)  ✅ DONE (committed + merged to `main` via `--no-ff`)

Reference: matched the doc style in `/Users/lazar/Projects/ideal-wedding-api` (controller-level
`<summary>`/`<param>`/`<returns>`, doc-gen for Swagger). Build verified: 0 errors.

- [x] Enabled `<GenerateDocumentationFile>true</GenerateDocumentationFile>` on `Entities` + `Core` (Api already had it).
      Deliberately **not** on `Common`/`Data` — they hold DTOs/enums/helpers/DbContext, which are neither
      domain classes nor system operations.
- [x] `/// <summary>` on all 8 domain classes + properties, plus Identity subclasses + `IEntity` → **Entities: 0 CS1591**
- [x] `/// <summary>` + `<param>` + `<returns>` on every manager method (system operations) → **Core: 0 CS1591**
- [x] `/// <summary>` on every controller action (+ class/ctor) → **controllers: 0 CS1591**
- [x] XML doc files generated: `BrandedGames.Entities.xml`, `BrandedGames.Core.xml`, `BrandedGames.Api.xml`
- [x] Build clean of doc warnings on the **required** surface (domain classes + system operations)

### Infrastructure docs (optional add-on, completed)
- [x] Documented all remaining Api infrastructure: auth handlers + requirements, middleware,
  `MapperConfig`, `BaseController`, `Policies`, `AuthorizationHelper`, `SecurityRequirementsOperationFilter`,
  `ValidationProblemDetailsResult` → **0 CS1591 across the whole solution**.
- [x] Cleared pre-existing dead-code warnings: removed unused `userId` in `MapperConfig`; dropped unused
  `ex` in `GameFormManager.Create` catch → **0 CS compiler warnings**.

### Known remaining (out of scope for Phase 2)
- `NU1903`: **AutoMapper 13.0.1** has a known high-severity advisory
  (https://github.com/advisories/GHSA-rvv3-g6hj-g44x). Fixing means a package bump — handle as a
  separate dependency-update task, not in the docs phase.

### Phase 2 done
- [x] Committed on `feature/xml-docs`, merged to `main` with `--no-ff`, pushed both branches

---

## Phase 3 — xUnit test project  (branch: `feature/unit-tests`)

- [ ] Create `BrandedGames.Tests` (xUnit), add to `BrandedGames.sln`
- [ ] NuGet: `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`,
      `Microsoft.EntityFrameworkCore.InMemory` (or SQLite in-memory), `AutoMapper`, `Moq`
- [ ] Test infra: in-memory `BrandedGamesDbContext` factory + `IMapper` built from `MapperConfig`
- [ ] **Refactor for testability:** extract `ICloudinaryFileManager` so `GameFormManager` can be tested
      without hitting the Cloudinary network (inject a fake/Moq)
- [ ] **Domain class tests** — one test class per entity; verify construction, property get/set,
      collection defaults initialized (covers requirement #11)
- [ ] **System operation tests** — one test class per manager; each operation gets happy-path +
      not-found/validation-path tests (covers requirement #12)
- [ ] `dotnet test` green

---

## Phase 4 — Submission artifacts

- [ ] Domain **class diagram** (≥8 classes) — export image/PDF, place in repo or hand-in
- [ ] **System-operations list** (≥12) — derive from Phase 1 final endpoint list
- [ ] (Optional) JSON hardening for a stronger defense: e.g. a JSON file export/import operation
      or an explicit JSON-consuming endpoint

---

## Phase 5 — Git finalization

- [ ] Each phase done on its own feature branch
- [ ] Merge each with `git merge --no-ff` so branch topology is visible in history
- [ ] Tag a release: `git tag -a v1.0 -m "Course submission"` and push tags
- [ ] Verify on GitHub: visible history, at least one merged branch, at least one tag

---

## Decisions log

- **2026-06-16** — Track: C#/.NET. Test framework: **xUnit**. Ops strategy: **full CRUD on existing
  entities** (target ~15 operations). This session: written plan + this tracker only (no code changes yet).

## Open items / risks

- `GameFormManager` depends on the concrete `CloudinaryFileManager` (network). Needs an interface
  extraction before it is unit-testable — tracked in Phase 3.
- Leftover `Microsoft.EntityFrameworkCore.SqlServer` package reference after the Postgres switch
  (harmless, can be cleaned up).
- README.md is still IdealWedding-specific and contradicts this repo (out of scope unless asked).
