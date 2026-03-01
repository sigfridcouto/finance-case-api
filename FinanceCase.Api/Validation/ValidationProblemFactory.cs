using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace FinanceCase.Api.Validation;

public static class ValidationProblemFactory
{
    public static ValidationProblemDetails From(ValidationResult result)
    {
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return new ValidationProblemDetails(errors)
        {
            Title = "Validation error",
            Status = StatusCodes.Status400BadRequest
        };
    }
}