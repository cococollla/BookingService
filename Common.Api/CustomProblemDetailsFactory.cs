using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.Api;

/// <summary>
/// Тело ответа на проблемы валидации
/// Возвращает только коллекцию ошибок
/// </summary>
public class CustomValidationProblemDetails : ValidationProblemDetails
{
    /// <summary>
    /// Коллекция ошибок
    /// </summary>
    [JsonPropertyName("errors")] 
    public new IEnumerable<string> Errors { get; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public CustomValidationProblemDetails(ModelStateDictionary modelState)
    {
        Title = null;
        Errors = from keyModelStatePair in modelState
            select keyModelStatePair.Value.Errors
            into errors
            from error in errors
            select error.ErrorMessage;
    }
}

/// <summary>
/// Кастомное тело ответа при проблемах
/// </summary>
public class CustomProblemDetails : ProblemDetails
{
    /// <summary>
    /// Текст ошибки
    /// </summary>
    [JsonPropertyName("error")] 
    public string Error { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public CustomProblemDetails()
    {
        Title = null;
        Error = "Internal Server Error";
    }
}

/// <summary>
/// Кастомная фабрика для создания кастомных ответов
/// </summary>
public class CustomProblemDetailsFactory : ProblemDetailsFactory
{
    /// <inheritdoc />
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        return new CustomProblemDetails();
    }

    /// <inheritdoc />
    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        var problemDetails = new CustomValidationProblemDetails(modelStateDictionary);

        httpContext.Response.StatusCode = 400;
        return problemDetails;
    }
}