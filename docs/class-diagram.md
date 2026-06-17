# Domain Class Diagram

> Course requirement #3 — domain model with **≥ 8 classes**. This repository has **8 game-domain
> classes** plus the ASP.NET Core Identity model (`User`, `Role`, `UserRole`, …).
>
> The diagram below is written in [Mermaid](https://mermaid.js.org/) and renders directly on GitHub
> and in most IDE markdown previews. To export an image/PDF for hand-in, open the rendered preview
> and use *Export*, or paste the fenced block into <https://mermaid.live>.

## Game-domain classes (8)

`GameForm`, `GameFeature`, `GameType`, `PlatformType`, `GameFormFeature`, `GameFormPlatformType`,
`GameFormFile`, `User`.

`GameFormFeature` and `GameFormPlatformType` are join entities that resolve the two many-to-many
relationships (a game form has many features and targets many platforms).

```mermaid
classDiagram
    direction LR

    class IEntity {
        <<interface>>
        +DateTime CreatedAt
        +DateTime ModifiedAt
    }

    class CustomerType {
        <<enumeration>>
        Influencer
        InfluencerPromotingABrand
        Brand
        IndividualUser
        Other
    }

    class FileType {
        <<enumeration>>
        Image
        Music
    }

    class GameForm {
        +Guid Id
        +Guid GameTypeId
        +CustomerType CustomerType
        +int Price
        +string Items
        +bool RewardTopPlayers
        +string Rewards
        +string RewardPlacements
    }

    class GameType {
        +Guid Id
        +string Name
        +string IconName
    }

    class GameFeature {
        +Guid Id
        +string Name
        +string IconName
        +int Price
        +string Description
    }

    class PlatformType {
        +Guid Id
        +string Name
        +string IconName
        +string Description
    }

    class GameFormFeature {
        +Guid GameFeatureId
        +Guid GameFormId
    }

    class GameFormPlatformType {
        +Guid GameFormId
        +Guid PlatformTypeId
    }

    class GameFormFile {
        +Guid Id
        +Guid GameFormId
        +string FileName
        +string FileOriginalName
        +string FileUrl
        +string ThumbUrl
        +FileType FileType
        +DateTime CreatedAt
        +DateTime ModifiedAt
    }

    class User {
        +Guid Id
        +string FirstName
        +string LastName
        +string Email
        +string FileUrl
        +string EmailVerificationCode
        +bool Active
        +DateTime CreatedAt
        +DateTime ModifiedAt
    }

    class Role {
        +Guid Id
        +string Name
    }

    class UserRole {
        +Guid UserId
        +Guid RoleId
    }

    %% Relationships
    GameType "1" o-- "0..*" GameForm : GameForms
    GameForm "1" *-- "0..*" GameFormFeature : Features
    GameFeature "1" *-- "0..*" GameFormFeature : Features
    GameForm "1" *-- "0..*" GameFormPlatformType : GameFormPlatformTypes
    PlatformType "1" *-- "0..*" GameFormPlatformType : GameFormPlatformTypes
    GameForm "1" *-- "0..*" GameFormFile : Files

    GameForm ..> CustomerType : uses
    GameFormFile ..> FileType : uses

    User "1" *-- "0..*" UserRole : UserRoles
    Role "1" *-- "0..*" UserRole : assignments
    User ..|> IEntity : implements
```

## Relationship summary

| From | To | Cardinality | Navigation |
|---|---|---|---|
| `GameType` | `GameForm` | 1 → many | `GameType.GameForms` / `GameForm.GameType` |
| `GameForm` ↔ `GameFeature` | via `GameFormFeature` | many ↔ many | `GameForm.Features` / `GameFeature.Features` |
| `GameForm` ↔ `PlatformType` | via `GameFormPlatformType` | many ↔ many | `GameForm.GameFormPlatformTypes` / `PlatformType.GameFormPlatformTypes` |
| `GameForm` | `GameFormFile` | 1 → many | `GameForm.Files` |
| `User` ↔ `Role` | via `UserRole` | many ↔ many | `User.UserRoles` |
| `User` | `IEntity` | implements | audit timestamps `CreatedAt` / `ModifiedAt` |

> Notes
> - `User` extends `IdentityUser<Guid>` and `Role` extends `IdentityRole<Guid>` from ASP.NET Core
>   Identity; only the project-specific members are shown above for clarity.
> - Nullable members (rendered without the `?` suffix so the Mermaid source parses cleanly on GitHub):
>   `GameForm.Price`, `GameFormFile.GameFormId`, and every `ModifiedAt` are nullable in code.
