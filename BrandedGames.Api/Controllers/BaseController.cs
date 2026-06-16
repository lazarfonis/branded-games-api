using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrandedGames.Api.Controllers;

/// <summary>
/// Base controller providing helpers shared by all API controllers.
/// </summary>
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>Gets the identifier of the currently authenticated user, or null if unauthenticated.</summary>
    /// <returns>The current user's identifier, or null.</returns>
    protected Guid? GetCurrentUserId()
    {
        _ = Guid.TryParse(HttpContext.User.FindFirst(ClaimTypes.Name)?.Value, out Guid parsedId);
        return parsedId == Guid.Empty ? null : parsedId;
    }
}
