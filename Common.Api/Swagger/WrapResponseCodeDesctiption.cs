using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Common.Api.Swagger;

/// <summary>
/// Оборачивает описание ответов в стандартное описание кодов.
/// </summary>
public class WrapResponseCodeDesctiption: IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.TryGetValue("200", out var apiResponse)
            && !apiResponse.Description.Equals("Success"))
        {
            apiResponse.Description = $"Success - {apiResponse.Description}";
        }
        
        if (operation.Responses.TryGetValue("400", out apiResponse)
            && !apiResponse.Description.Equals("Bad Request"))
        {
            apiResponse.Description = $"Bad Request - {apiResponse.Description}";
        }
        
        if (operation.Responses.TryGetValue("404", out apiResponse)
            && !apiResponse.Description.Equals("Not Found"))
        {
            apiResponse.Description = $"Not Found - {apiResponse.Description}";
        }
        
        if (operation.Responses.TryGetValue("409", out apiResponse)
            && !apiResponse.Description.Equals("Conflict" ))
        {
            apiResponse.Description = $"Conflict - {apiResponse.Description}";
        }
        
        if (operation.Responses.TryGetValue("500", out apiResponse)
            && !apiResponse.Description.Equals("Internal Server Error"))
        {
            apiResponse.Description = $"Internal Server Error - {apiResponse.Description}";
        }
    }
}