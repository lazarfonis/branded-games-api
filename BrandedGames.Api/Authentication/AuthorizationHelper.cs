using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BrandedGames.Api.Authentication;

/// <summary>
/// Helper for reading the authenticated user's identifier from an authorization context.
/// </summary>
public static class AuthorizationHelper
{
    /// <summary>Attempts to read and parse the current user's identifier from the authorization context.</summary>
    /// <param name="context">The authorization handler context.</param>
    /// <param name="userId">When this method returns, contains the parsed user identifier.</param>
    /// <returns><c>true</c> if a valid user identifier was found; otherwise <c>false</c>.</returns>
    public static bool TryParseUserId(AuthorizationHandlerContext context, out Guid userId)
    {
        var nameClaim = context.User.FindFirst(ClaimTypes.Name);

        if (nameClaim == null)
        {
            userId = default;
            return false;
        }

        _ = Guid.TryParse(nameClaim.Value, out userId);

        if (userId == Guid.Empty)
        {
            return false;
        }

        return true;
    }
}
