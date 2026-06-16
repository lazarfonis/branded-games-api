using BrandedGames.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrandedGames.Api.Authentication;

/// <summary>
/// Authorization handler that grants access only to existing, registered users.
/// </summary>
public class RegisteredUserHandler : AuthorizationHandler<RegisteredUserRequirement>
{
    private readonly BrandedGamesDbContext db;

    /// <summary>Creates a new <see cref="RegisteredUserHandler"/>.</summary>
    /// <param name="db">The database context.</param>
    public RegisteredUserHandler(BrandedGamesDbContext db)
    {
        this.db = db;
    }

    /// <summary>Evaluates the registered-user requirement for the current request.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <param name="requirement">The requirement being evaluated.</param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegisteredUserRequirement requirement)
    {
        var isUserRegistered = await CheckIfUserIsRegistered(context);

        if (isUserRegistered)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    /// <summary>Checks whether the current user exists in the database.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <returns><c>true</c> if the user exists; otherwise <c>false</c>.</returns>
    public async Task<bool> CheckIfUserIsRegistered(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        return await db.Users.AnyAsync(u => u.Id == userId);
    }
}

/// <summary>Authorization requirement representing a registered user.</summary>
public class RegisteredUserRequirement : IAuthorizationRequirement
{
}
