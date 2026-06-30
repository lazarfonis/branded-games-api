# System Operations

> Course requirement #4 — list of system operations with a target of **≥ 12**. This API exposes
> **20 system operations** across four resources. Each operation is a thin controller action that
> delegates to a manager method (the business-logic layer); see the per-row "Manager method".
>
> Base path: `/api`. All payloads are JSON except game-form creation, which is `multipart/form-data`
> (it carries uploaded files).

## Count by resource

| Resource | Controller | Operations |
|---|---|---|
| Feature | `FeatureController` (`api/features`) | 5 |
| Game type | `GameTypeController` (`api/game-types`) | 5 |
| Platform type | `PlatformTypeController` (`api/platform-types`) | 5 |
| Game form | `GameFormController` (`api/customer-games`) | 5 |
| **Total** | | **20** |

## Feature — `GameFeature`

| # | Operation | HTTP | Route | Request body | Success | Manager method |
|---|---|---|---|---|---|---|
| 1 | Get all features | GET | `/api/features` | — | 200 `FeatureModel[]` | `FeatureManager.GetFeatures()` |
| 2 | Get feature by id | GET | `/api/features/{id}` | — | 200 `FeatureModel` | `FeatureManager.GetFeature(id)` |
| 3 | Create feature | POST | `/api/features` | `FeatureCreateModel` | 200 `FeatureModel` | `FeatureManager.CreateFeature(model)` |
| 4 | Update feature | PUT | `/api/features/{id}` | `FeatureUpdateModel` | 200 `FeatureModel` | `FeatureManager.UpdateFeature(id, model)` |
| 5 | Delete feature | DELETE | `/api/features/{id}` | — | 204 | `FeatureManager.DeleteFeature(id)` |

## Game type — `GameType`

| # | Operation | HTTP | Route | Request body | Success | Manager method |
|---|---|---|---|---|---|---|
| 6 | Get all game types | GET | `/api/game-types` | — | 200 `GameTypeModel[]` | `GameTypeManager.GetTypes()` |
| 7 | Get game type by id | GET | `/api/game-types/{id}` | — | 200 `GameTypeModel` | `GameTypeManager.GetType(id)` |
| 8 | Create game type | POST | `/api/game-types` | `GameTypeCreateModel` | 200 `GameTypeModel` | `GameTypeManager.CreateType(model)` |
| 9 | Update game type | PUT | `/api/game-types/{id}` | `GameTypeUpdateModel` | 200 `GameTypeModel` | `GameTypeManager.UpdateType(id, model)` |
| 10 | Delete game type | DELETE | `/api/game-types/{id}` | — | 204 | `GameTypeManager.DeleteType(id)` |

## Platform type — `PlatformType`

| # | Operation | HTTP | Route | Request body | Success | Manager method |
|---|---|---|---|---|---|---|
| 11 | Get all platform types | GET | `/api/platform-types` | — | 200 `PlatformTypeModel[]` | `PlatformTypeManager.GetPlatforms()` |
| 12 | Get platform type by id | GET | `/api/platform-types/{id}` | — | 200 `PlatformTypeModel` | `PlatformTypeManager.GetPlatform(id)` |
| 13 | Create platform type | POST | `/api/platform-types` | `PlatformTypeCreateModel` | 200 `PlatformTypeModel` | `PlatformTypeManager.CreatePlatform(model)` |
| 14 | Update platform type | PUT | `/api/platform-types/{id}` | `PlatformTypeUpdateModel` | 200 `PlatformTypeModel` | `PlatformTypeManager.UpdatePlatform(id, model)` |
| 15 | Delete platform type | DELETE | `/api/platform-types/{id}` | — | 204 | `PlatformTypeManager.DeletePlatform(id)` |

## Game form — `GameForm`

| # | Operation | HTTP | Route | Request body | Success | Manager method |
|---|---|---|---|---|---|---|
| 16 | Get all game forms | GET | `/api/customer-games` | — | 200 `GameFormModel[]` | `GameFormManager.GetGames()` |
| 17 | Get my game forms | GET | `/api/customer-games/mine` | — | 200 `GameFormModel[]` | `GameFormManager.GetMyGames(userId)` |
| 18 | Get game form by id | GET | `/api/customer-games/{id}` | — | 200 `GameFormModel` | `GameFormManager.GetGame(id)` |
| 19 | Create game form | POST | `/api/customer-games` | `GameFormCreateModel` (`multipart/form-data`, incl. files) | 204 | `GameFormManager.Create(model, userId)` |
| 20 | Delete game form | DELETE | `/api/customer-games/{id}` | — | 204 | `GameFormManager.DeleteGame(id)` |

> Notes
> - Operation 17 (`/mine`) is restricted to authenticated users (`Policies.RegisteredUser`) and
>   returns only the game forms submitted by the current user.
> - Operation 19 creates a game form together with its selected features, target platforms and
>   uploaded files inside a single database transaction; files are pushed to the storage provider via
>   `ICloudinaryFileManager`.
> - Missing entities surface as `404 Not Found` through `ValidationHelper.MustExist<T>()`; validation
>   failures surface as `400 Bad Request`. Both are translated to JSON problem responses by the
>   exception-handling middleware.
> - These operations are exercised by the `BrandedGames.Tests` xUnit suite (course
>   requirement #12).
