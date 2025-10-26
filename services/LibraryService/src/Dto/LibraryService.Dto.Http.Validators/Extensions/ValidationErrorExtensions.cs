using FluentValidation;
using LibraryService.Dto.Http;

namespace PersonService.Server.Extensions;

public static class ValidationErrorExtensions
{
    public static ValidationErrorResponse ToValidationErrorResponse(this ValidationException ex, 
        string? customMessage = null)
    {
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => ToCamelCase(g.Key),
                g => string.Join("; ", g.Select(e => e.ErrorMessage))
            );

        return new ValidationErrorResponse(customMessage ?? "Произошла ошибка валидации", errors);
    }
    
    private static string ToCamelCase(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || char.IsLower(propertyName[0]))
            return propertyName;
            
        return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
    }
}