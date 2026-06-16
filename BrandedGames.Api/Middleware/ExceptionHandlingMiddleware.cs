using BrandedGames.Common.Enums;
using BrandedGames.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace BrandedGames.Common.Middleware;

/// <summary>
/// Middleware that catches unhandled exceptions and writes a JSON error response,
/// mapping known exception types to the appropriate HTTP status code.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment env;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    /// <summary>Creates a new <see cref="ExceptionHandlingMiddleware"/>.</summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="env">The hosting environment.</param>
    /// <param name="logger">The logger.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.env = env;
        this.logger = logger;
    }

    /// <summary>Invokes the middleware for the current request.</summary>
    /// <param name="context">The HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        
        List<ExceptionDetail> exceptionDetails = new List<ExceptionDetail>();

        if (exception is ValidationException)
        {
            statusCode = HttpStatusCode.BadRequest;
            exceptionDetails = (exception as ValidationException).Details;
        }
        else if (exception is AuthenticationException)
        {
            statusCode = HttpStatusCode.Unauthorized;
            exceptionDetails = (exception as AuthenticationException).Details;
        }
        else if (exception is AuthorizationException)
        {
            statusCode = HttpStatusCode.Forbidden;
            exceptionDetails = (exception as AuthorizationException).Details;
        }
        else if (exception is NotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            exceptionDetails = (exception as NotFoundException).Details;
        }
        else
        {
            var isProduction = env.EnvironmentName.Equals("production", StringComparison.OrdinalIgnoreCase);
            statusCode = HttpStatusCode.InternalServerError;
            exceptionDetails.Add(new ExceptionDetail
            {
                ErrorCode = ErrorCode.InternalServerError,
                Params = new
                {
                    details = isProduction ? "A server error has occured." : exception.Message
                }
            });
        }

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        logger.LogError(exception, exception.Message);

        var serializerSettings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };
        serializerSettings.Converters.Add(new StringEnumConverter());

        var responseString = JsonConvert.SerializeObject(exceptionDetails, serializerSettings);

        await response.WriteAsync(responseString).ConfigureAwait(false);
    }
}
