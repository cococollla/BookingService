using Common.Api.Results;
using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Common.Api.Swagger;

/// <summary>
/// Добавляет 500 ко всем endpoints.
/// </summary>
public class Add500ErrorResponse: IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("500"))
            return;
        
        if(!context.SchemaRepository.Schemas.ContainsKey(nameof(TErrorBody)))
            context.SchemaGenerator.GenerateSchema(typeof(TErrorBody), context.SchemaRepository);
        
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal Server Error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = nameof(TErrorBody)
                        }
                    }
                }
            }
        });
    }
}