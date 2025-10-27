using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

namespace Common.Api.Swagger;

/// <summary>
/// Простой фильтр что бы добавлять 403 ответ
/// </summary>
public sealed class ForbidHttpResultDocumentFilter : IDocumentFilter
{
    private Type[] _results =
    [
        typeof(Results<,,>),
        typeof(Results<,,,>),
        typeof(Results<,,,>),
        typeof(Results<,,,,>)
    ];

    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var apiDescription in context.ApiDescriptions)
        {
            if (apiDescription.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
                continue;

            var returnType = actionDescriptor.MethodInfo.ReturnType;

            if (!returnType.IsGenericType ||
                returnType.GetGenericTypeDefinition() != typeof(Task<>) ||
                !returnType.GetGenericArguments()
                           .Any(a => a.IsGenericType && _results.Any(r => r == a.GetGenericTypeDefinition())))
                continue;

            var results = returnType.GetGenericArguments().First();

            if (!results.IsGenericType ||
                !results.GetGenericArguments().Contains(typeof(ForbidHttpResult))) continue;

            if (apiDescription.SupportedResponseTypes.All(r => r.StatusCode != 403))
            {
                apiDescription.SupportedResponseTypes.Add(new ApiResponseType
                {
                    StatusCode = 403,
                    Type = typeof(void),
                    ApiResponseFormats = new List<ApiResponseFormat>
                    {
                        new() { MediaType = "application/json" }
                    }
                });
            }
        }
    }
}