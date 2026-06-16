using Azure.Core;
using BrandedGames.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;

namespace BrandedGames.Common.Middleware;

/// <summary>
/// Middleware that refreshes a valid JWT on each response, returning the new token
/// in a <c>refreshed-token</c> header unless refresh is explicitly skipped.
/// </summary>
public class SessionManagementMiddleware
{
    private readonly RequestDelegate next;
    private readonly JwtHelper authHelper;

    /// <summary>Creates a new <see cref="SessionManagementMiddleware"/>.</summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="configuration">Application configuration used to build the JWT helper.</param>
    public SessionManagementMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        this.next = next;
        authHelper = new JwtHelper(configuration);
    }

    /// <summary>Invokes the middleware for the current request.</summary>
    /// <param name="context">The HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Request.Headers.TryGetValue(HeaderNames.Authorization, out StringValues authorizationHeader);
            var authorizationHeaderString = authorizationHeader.ToString();

            if (!string.IsNullOrWhiteSpace(authorizationHeaderString))
            {
                var skipTokenRefreshExists = context.Request.Query.ContainsKey("skipTokenRefresh");

                if (!skipTokenRefreshExists)
                {
                    var tokenValue = authorizationHeaderString.Replace("Bearer", string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();

                    var isTokenValid = authHelper.VerifyToken(tokenValue);

                    if (isTokenValid)
                    {
                        var newToken = authHelper.RegenerateJwtToken(tokenValue);
                        context.Response.Headers.Append("refreshed-token", newToken);
                    }
                }
            }

            return Task.CompletedTask;
        });

        await next(context);
    }
}
