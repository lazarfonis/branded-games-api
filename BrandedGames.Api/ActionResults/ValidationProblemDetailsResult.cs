using BrandedGames.Common.Enums;
using BrandedGames.Common.Exceptions;
using BrandedGames.Common.Extensions;
using BrandedGames.Common.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BrandedGames.Api.ActionResults;

/// <summary>
/// Action result that turns invalid model state into a <see cref="ValidationException"/>
/// so it is rendered as a consistent JSON validation error response.
/// </summary>
public class ValidationProblemDetailsResult : IActionResult
{
    /// <summary>Executes the result, throwing a validation exception when model state is invalid.</summary>
    /// <param name="context">The action context.</param>
    public Task ExecuteResultAsync(ActionContext context)
    {
        if (context.ModelState.IsValid)
        {
            return Task.CompletedTask;
        }

        var invalidModelStates = context.ModelState
            .ToDictionary(ms => ms.Key)
            .Where(ms => ms.Value.Value.ValidationState == ModelValidationState.Invalid);

        var validationResults = new List<ValidationResult>();

        foreach (var modelState in invalidModelStates)
        {
            ModelErrorCollection errors = modelState.Value.Value.Errors;

            var validationResult = new ValidationResult
            {
                Property = modelState.Key.ToCamelCase(),
                Errors = errors
                    .Select(e => string.IsNullOrEmpty(e.ErrorMessage)
                        ? e.Exception.Message
                        : e.ErrorMessage)
                    .ToList()
            };

            validationResult.Errors = validationResult.Errors.Distinct().ToList();
            validationResults.Add(validationResult);
        }

        throw new ValidationException(validationResults.Select(result => new ExceptionDetail
        {
            ErrorCode = ErrorCode.RequestInvalid,
            Params = result
        }).ToList());
    }
}
