using BrandedGames.Common.Enums;
using BrandedGames.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrandedGames.Api.Authentication;

/// <summary>
/// Authorization handler that grants access only to users with the administrator role.
/// </summary>
public class AdministratorUserHandler : AuthorizationHandler<AdministratorUserRequirement>
{
    private readonly BrandedGamesDbContext db;

    /// <summary>Creates a new <see cref="AdministratorUserHandler"/>.</summary>
    /// <param name="db">The database context.</param>
    public AdministratorUserHandler(BrandedGamesDbContext db)
    {
        this.db = db;
    }

    /// <summary>Evaluates the administrator requirement for the current request.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <param name="requirement">The requirement being evaluated.</param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorUserRequirement requirement)
    {
        var isUserAdministrator = await CheckIfUserIsAdministrator(context);

        if (isUserAdministrator)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    /// <summary>Checks whether the current user has a confirmed email and the administrator role.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <returns><c>true</c> if the user is an administrator; otherwise <c>false</c>.</returns>
    public async Task<bool> CheckIfUserIsAdministrator(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        var userInfo = await db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { Id = u.Id, EmailConfirmed = u.EmailConfirmed })
            .FirstOrDefaultAsync();

        if (userInfo == null || !userInfo.EmailConfirmed)
        {
            return false;
        }

        var isAdmin = await db.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == UserRoleConstants.Administrator);

        return isAdmin;
    }
}

/// <summary>Authorization requirement representing administrator access.</summary>
public class AdministratorUserRequirement : IAuthorizationRequirement
{
}
