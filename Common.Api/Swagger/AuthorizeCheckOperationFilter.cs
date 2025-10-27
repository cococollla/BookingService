using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace Common.Api.Swagger;


/// <summary>
/// Фильтр, добавляющий к endpoints с <see cref="AuthorizeAttribute"/> описание требования авторизации и требуемых ролей
/// </summary>
public class AuthorizeCheckOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Проверяем наличие атрибута [Authorize]
        var authorizeAttributes = context.MethodInfo
                                         .GetCustomAttributes(true)
                                         .OfType<AuthorizeAttribute>()
                                         .ToList();

        if (!authorizeAttributes.Any())
            return;

        // Добавляем информацию о требуемых ролях
        var roles = authorizeAttributes
                    .Where(a => !string.IsNullOrEmpty(a.Roles))
                    .Select(a => a.Roles)
                    .ToList();

        // operation.Summary += " \ud83d\udd11";  

        operation.Description += "\n\n\ud83d\udd11";
        
        if (roles.Count != 0)
            operation.Description += $"\ud83d\udc64: {string.Join(", ", roles)}";

        // Добавляем Security Requirement
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            }
        };
    }
}