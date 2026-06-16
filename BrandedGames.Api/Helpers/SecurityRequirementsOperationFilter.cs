// Referenced from: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/tree/master/src/Swashbuckle.AspNetCore.Filters

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BrandedGames.Api.Helpers;

/// <summary>
/// Swagger operation filter that adds security requirements (and 401/403 responses)
/// to operations guarded by <see cref="AuthorizeAttribute"/>.
/// </summary>
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> filter;

    /// <summary>
    /// Constructor for SecurityRequirementsOperationFilter
    /// </summary>
    /// <param name="includeUnauthorizedAndForbiddenResponses">If true (default), then 401 and 403 responses will be added to every operation</param>
    /// <param name="securitySchemaName">Name of the security schema. Default value is <c>"oauth2"</c></param>
    public SecurityRequirementsOperationFilter(bool includeUnauthorizedAndForbiddenResponses = true, string securitySchemaName = "oauth2")
    {
        Func<IEnumerable<AuthorizeAttribute>, IEnumerable<string>> policySelector = authAttributes =>
            authAttributes
                .Where(a => !string.IsNullOrEmpty(a.Policy))
                .Select(a => a.Policy);

        filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(policySelector, includeUnauthorizedAndForbiddenResponses, securitySchemaName);
    }

    /// <summary>Applies the security requirements to the given operation.</summary>
    /// <param name="operation">The OpenAPI operation being built.</param>
    /// <param name="context">The operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        filter.Apply(operation, context);
    }
}

/// <summary>
/// Generic Swagger operation filter that adds security requirements for operations
/// guarded by an authorization attribute of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The authorization attribute type to look for.</typeparam>
public class SecurityRequirementsOperationFilter<T> : IOperationFilter where T : Attribute
{
    // inspired by https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/test/WebSites/OAuth2Integration/ResourceServer/Swagger/SecurityRequirementsOperationFilter.cs

    private readonly bool includeUnauthorizedAndForbiddenResponses;
    private readonly string securitySchemaName;
    private readonly Func<IEnumerable<T>, IEnumerable<string>> policySelector;

    /// <summary>
    /// Constructor for SecurityRequirementsOperationFilter
    /// </summary>
    /// <param name="policySelector">Used to select the authorization policy from the attribute e.g. (a => a.Policy)</param>
    /// <param name="includeUnauthorizedAndForbiddenResponses">If true (default), then 401 and 403 responses will be added to every operation</param>
    /// <param name="securitySchemaName">Name of the security schema. Default value is <c>"oauth2"</c></param>
    public SecurityRequirementsOperationFilter(
        Func<IEnumerable<T>, IEnumerable<string>> policySelector,
        bool includeUnauthorizedAndForbiddenResponses = true,
        string securitySchemaName = "oauth2")
    {
        this.policySelector = policySelector;
        this.includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
        this.securitySchemaName = securitySchemaName;
    }

    /// <summary>Applies the security requirements to the given operation.</summary>
    /// <param name="operation">The OpenAPI operation being built.</param>
    /// <param name="context">The operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        var actionAttributes = context.GetControllerAndActionAttributes<T>();

        if (!actionAttributes.Any())
        {
            return;
        }

        if (includeUnauthorizedAndForbiddenResponses)
        {
            if (!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            }

            if (!operation.Responses.ContainsKey("403"))
            {
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }
        }

        var policies = policySelector(actionAttributes) ?? Enumerable.Empty<string>();

        operation.Security.Add(new OpenApiSecurityRequirement
            {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = securitySchemaName } }, policies.ToList() }
            });
    }
}

internal static class OperationFilterContextExtensions
{
    public static IEnumerable<T> GetControllerAndActionAttributes<T>(this OperationFilterContext context) where T : Attribute
    {
        var controllerAttributes = context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes<T>();
        var actionAttributes = context.MethodInfo.GetCustomAttributes<T>();

        var result = new List<T>(controllerAttributes);
        result.AddRange(actionAttributes);
        return result;
    }
}
