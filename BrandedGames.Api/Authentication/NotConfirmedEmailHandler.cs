using BrandedGames.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrandedGames.Api.Authentication;

/// <summary>
/// Authorization handler that grants access only to users whose email is not yet confirmed.
/// </summary>
public class NotConfirmedEmailHandler : AuthorizationHandler<NotConfirmedEmailRequirement>
{
    private readonly BrandedGamesDbContext db;

    /// <summary>Creates a new <see cref="NotConfirmedEmailHandler"/>.</summary>
    /// <param name="db">The database context.</param>
    public NotConfirmedEmailHandler(BrandedGamesDbContext db)
    {
        this.db = db;
    }

    /// <summary>Evaluates the not-confirmed-email requirement for the current request.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <param name="requirement">The requirement being evaluated.</param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NotConfirmedEmailRequirement requirement)
    {
        var emailNotConfirmed = await CheckEmailNotConfirmed(context);

        if (emailNotConfirmed)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    /// <summary>Checks whether the current user's email is not yet confirmed.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <returns><c>true</c> if the user exists and their email is not confirmed; otherwise <c>false</c>.</returns>
    public async Task<bool> CheckEmailNotConfirmed(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        var userInfo = await db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { Id = u.Id, EmailConfirmed = u.EmailConfirmed })
            .FirstOrDefaultAsync();

        return userInfo != null && !userInfo.EmailConfirmed;
    }
}

/// <summary>Authorization requirement representing an unconfirmed email.</summary>
public class NotConfirmedEmailRequirement : IAuthorizationRequirement
{
}
