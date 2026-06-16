using BrandedGames.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrandedGames.Api.Authentication;

/// <summary>
/// Authorization handler that grants access only to users whose email is confirmed.
/// </summary>
public class EmailConfirmedHandler : AuthorizationHandler<EmailConfirmedRequirement>
{
    private readonly BrandedGamesDbContext db;

    /// <summary>Creates a new <see cref="EmailConfirmedHandler"/>.</summary>
    /// <param name="db">The database context.</param>
    public EmailConfirmedHandler(BrandedGamesDbContext db)
    {
        this.db = db;
    }

    /// <summary>Evaluates the email-confirmed requirement for the current request.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <param name="requirement">The requirement being evaluated.</param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailConfirmedRequirement requirement)
    {
        var hasEmailConfirmed = await CheckEmailConfirmed(context);

        if (hasEmailConfirmed)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    /// <summary>Checks whether the current user's email is confirmed.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <returns><c>true</c> if the user's email is confirmed; otherwise <c>false</c>.</returns>
    public async Task<bool> CheckEmailConfirmed(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        var userInfo = await db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { Id = u.Id, EmailConfirmed = u.EmailConfirmed })
            .FirstOrDefaultAsync();

        return userInfo != null && userInfo.EmailConfirmed;
    }
}

/// <summary>Authorization requirement representing a confirmed email.</summary>
public class EmailConfirmedRequirement : IAuthorizationRequirement
{
}
