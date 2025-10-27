using System.Collections.Generic;
using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Common.Api.Results.Swagger;

/// <summary>
/// Добавляет 400 ко всем endpoints.
/// </summary>
public class Add400ErrorResponse : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("400"))
            return;

        if(!context.SchemaRepository.Schemas.ContainsKey(nameof(TErrorsBody)))
            context.SchemaGenerator.GenerateSchema(typeof(TErrorsBody), context.SchemaRepository);
        
        operation.Responses.Add("400", new OpenApiResponse
        {
            Description = "Bad Request",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = nameof(TErrorsBody)
                        }
                    }
                }
            }
        });
    }
}