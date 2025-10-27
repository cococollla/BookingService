using System.ComponentModel.DataAnnotations;

namespace Common.Api.Results.Swagger;

/// <summary>
/// Валидатор для строк
/// </summary>
public static class StringModelValidator
{
    /// <summary>
    /// Строка не null, не пустая и не состоит только из пробелов
    /// </summary>
    public static ValidationResult ValidateNullOrWhiteSpace(string value, ValidationContext context)
    {
        return string.IsNullOrWhiteSpace(value) ? new ValidationResult($"The field {context.MemberName} cannot consist of spaces only.") : ValidationResult.Success!;
    }
    
    /// <summary>
    /// Строка не состоит только из пробелов
    /// </summary>
    public static ValidationResult ValidateWhiteSpace(string value, ValidationContext context)
    {
        
        if (string.IsNullOrEmpty(value))
            return ValidationResult.Success!;
        
        return string.IsNullOrWhiteSpace(value) ? new ValidationResult($"The field {context.MemberName} cannot consist of spaces only but can be empty.") : ValidationResult.Success!;
    }
}