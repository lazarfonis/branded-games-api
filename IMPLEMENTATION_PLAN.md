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
| 2 | System operations | ≥ 12 | ✅ Met (20) | Full CRUD implemented (Phase 1) |
| 3 | Class diagram artifact | submitted | ✅ Ready | `docs/class-diagram.md` (Mermaid, 8+ classes) — hand in |
| 4 | System-operations list artifact | submitted | ✅ Ready | `docs/system-operations.md` (20 operations) — hand in |
| 5 | Git uses own email | — | ✅ Met | `lazarst.pn@gmail.com` |
| 6 | Visible history | — | ✅ Met | Phased commits + 4 `--no-ff` merge commits pushed to `origin` |
| 7 | Branch create **and merge** | mandatory | ✅ Met | 4 feature branches, each merged to `main` via `--no-ff` |
| 8 | Tags | mandatory | ✅ Met | Annotated `v1.0` ("Course submission") pushed to `origin` |
| 9 | Hosted (GitHub) | — | ✅ Met | `origin/main` exists |
| 10 | Build via NuGet | mandatory | ✅ Met | SDK-style projects |
| 11 | All domain classes tested | mandatory | ✅ Met | `BrandedGames.Tests` (xUnit) — domain test class per entity |
| 12 | All system operations tested | mandatory | ✅ Met | `BrandedGames.Tests` (xUnit) — manager test class per operation |
| 13 | All documented (XML docs) | mandatory | ✅ Met | Domain classes + all operations (Phase 2); infra intentionally scoped out |
| 14 | JSON functionality | mandatory | ✅ Met (weak) | API returns JSON; Cloudinary JSON call. Optional hardening below. |
| 15 | Other course tech | optional | ✅ Bonus | JWT, AutoMapper, NLog, Swagger, Docker |

## Current state snapshot (baseline)

- **Solution:** .NET 8, 5 projects — `Api → Core → Data → Entities`, all → `Common`. SQL Server/EF Core.
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

Strategy: **full CRUD on existing entities** (no new domain). **20 operations total** — clears the ≥12 bar.
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

Reference: standard XML doc-comment style (controller-level
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

### Known remaining — DECISION: won't fix
- `NU1903`: **AutoMapper 13.0.1** has a known high-severity advisory
  (https://github.com/advisories/GHSA-rvv3-g6hj-g44x). **Do NOT upgrade AutoMapper or the .NET
  version.** Newer AutoMapper releases moved to a commercial/paid license, which we are deliberately
  avoiding. The advisory is accepted as-is; pin versions and do not "fix" this in any future phase.

### Phase 2 done
- [x] Committed on `feature/xml-docs`, merged to `main` with `--no-ff`, pushed both branches

---

## Phase 3 — xUnit test project  (branch: `feature/unit-tests`)  ✅ DONE (committed + merged to `main` via `--no-ff`)

**52 tests, all green.** `dotnet test` passes; full-solution build clean of new warnings (only the
accepted NU1903 AutoMapper advisory remains).

- [x] Create `BrandedGames.Tests` (xUnit), add to `BrandedGames.sln`
- [x] NuGet: `xunit` 2.9.2, `xunit.runner.visualstudio` 2.8.2, `Microsoft.NET.Test.Sdk` 17.11.1,
      `Microsoft.EntityFrameworkCore.InMemory` 8.0.11, `AutoMapper` 13.0.1 (pinned per policy), `Moq` 4.20.72
- [x] Test infra: `TestDbContextFactory` (in-memory `BrandedGamesDbContext`, unique store per test,
      `TransactionIgnoredWarning` suppressed) + `TestMapper` (`IMapper` built from `MapperConfig`)
- [x] **Refactor for testability:** extracted `ICloudinaryFileManager` (Core); `CloudinaryFileManager`
      implements it; `GameFormManager` depends on the interface; DI registers
      `AddScoped<ICloudinaryFileManager, CloudinaryFileManager>()`. Tests inject a Moq fake — no network.
- [x] **Supporting refactors:** moved `MapperConfig` from `Api/Helpers` to `Core` (so the test project
      reuses it without referencing the web project — Api still auto-discovers it via assembly scan);
      guarded `Database.Migrate()` with `if (Database.IsRelational())` so the in-memory provider can
      construct the context.
- [x] **Domain class tests** — one test class per entity (8 game entities + `User`): construction,
      property get/set round-trip, collection defaults initialized (covers requirement #11)
- [x] **System operation tests** — one test class per manager (`Feature`, `GameType`, `PlatformType`,
      `GameForm`); each operation gets happy-path + not-found/validation-path tests (covers requirement #12)
- [x] `dotnet test` green — **52 passed, 0 failed**

---

## Phase 4 — Submission artifacts  (branch: `feature/submission-artifacts`)  ✅ DONE (committed + merged to `main` via `--no-ff`)

- [x] Domain **class diagram** (≥8 classes) → `docs/class-diagram.md` — Mermaid `classDiagram` of the
      8 game-domain classes + Identity (`User`/`Role`/`UserRole`) + `IEntity` and the two enums, with a
      relationship summary table. Renders on GitHub; export to image/PDF from the preview for hand-in.
- [x] **System-operations list** (≥12) → `docs/system-operations.md` — all **20** operations grouped by
      resource (HTTP verb, route, request/response model, backing manager method).
- [ ] (Optional) JSON hardening for a stronger defense: e.g. a JSON file export/import operation
      or an explicit JSON-consuming endpoint — **not done** (req #14 already Met; left as optional).

---

## Phase 5 — Git finalization  ✅ DONE

- [x] Each phase done on its own feature branch (`feature/crud-operations`, `feature/xml-docs`,
      `feature/unit-tests`, `feature/submission-artifacts`)
- [x] Merge each with `git merge --no-ff` so branch topology is visible in history (4 merge commits)
- [x] Tag a release: `git tag -a v1.0 -m "Course submission"` — pushed to `origin`
- [x] Verify on GitHub: `git ls-remote` confirms all 4 branches + `main` + tag `v1.0` (dereferences to
      `main` HEAD); the 4 `--no-ff` merge commits are visible in pushed history

---

## Decisions log

- **2026-06-16** — Track: C#/.NET. Test framework: **xUnit**. Ops strategy: **full CRUD on existing
  entities** (target ~15 operations). This session: written plan + this tracker only (no code changes yet).
- **2026-06-16** — Phase 1 (19 system operations) and Phase 2 (XML docs, zero-warning build) both
  implemented, committed on feature branches, merged to `main` via `--no-ff`, and pushed.
- **2026-06-16** — **Dependency policy:** keep AutoMapper (13.0.1) and the .NET version pinned. Newer
  AutoMapper is commercially licensed, so we will not upgrade. The `NU1903` advisory is accepted; do
  not attempt to resolve it.
- **2026-06-17** — Phase 3 (xUnit test project, 52 tests green) implemented on `feature/unit-tests`,
  merged to `main` via `--no-ff`. Testability refactors: extracted `ICloudinaryFileManager`; relocated
  `MapperConfig` from `Api/Helpers` to `Core`; guarded `Database.Migrate()` with `Database.IsRelational()`.
  Requirements #11 and #12 now Met.
- **2026-06-17** — Phase 4 (submission artifacts) implemented on `feature/submission-artifacts`, merged
  to `main` via `--no-ff`. Added `docs/class-diagram.md` (Mermaid, 8+ classes) and
  `docs/system-operations.md` (19 operations). Requirements #3 and #4 now Ready. Optional JSON hardening
  deliberately skipped (req #14 already Met). Diagram uses Mermaid in markdown (renders on GitHub) — no
  local diagram CLI available; export to image/PDF from the preview at hand-in time if the prof wants a file.
- **2026-06-17** — Phase 5 (git finalization) done. Pushed `main`, `feature/unit-tests`,
  `feature/submission-artifacts`, and annotated tag `v1.0` ("Course submission") to `origin`
  (`github.com:lazarfonis/branded-games-api`). Requirements #6, #7, #8 now Met. **All phases complete.**

## Resume here (next session)
- **All phases (1–5) are complete.** The codebase is course-compliant: 19 system operations, XML docs
  (zero-warning required surface), 52 xUnit tests, class-diagram + operations-list artifacts in `docs/`,
  4 `--no-ff`-merged feature branches and tag `v1.0` on GitHub.
- Only optional follow-ups remain (see below): JSON hardening.
- For the hand-in itself: export `docs/class-diagram.md` to an image/PDF if the professor wants a file.

## Open items / risks

- ~~Branches/tags not pushed~~ — **resolved (Phase 5):** `main`, all feature branches, and tag `v1.0`
  are on `origin`.
- Database provider switched back to SQL Server (`Microsoft.EntityFrameworkCore.SqlServer`); the
  Npgsql packages were removed and the `Initial` migration regenerated for SQL Server.
- ~~README.md is still IdealWedding-specific~~ — **resolved:** README rewritten for Branded Games
  (tech stack, build/run, migrations, tests, auth).
- The IDE can rewrite `BrandedGames.sln` and drop CLI-added projects — verify the `.sln` diff before
  committing after using `dotnet sln add`.
