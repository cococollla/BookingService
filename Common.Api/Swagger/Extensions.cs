using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api.Swagger;

/// <summary>
/// Расширения для Swagger
/// </summary>
public static class Extentions
{
    /// <summary>
    /// Использовать готовую конфигурацию
    /// </summary>
    /// <remarks>
    /// Автоматически подбирает все загруженные сборки и загружает их XML документацию.
    /// Сборки с API должны содержать аттрибут:
    /// <PropertyGroup>
    ///     <GenerateDocumentationFile>true</GenerateDocumentationFile>
    /// </PropertyGroup> 
    /// </remarks>
    /// <param name="options">опции</param>
    public static void UseDefaultConfig(this SwaggerGenOptions options)
    {
        // TODO: проверить эту штуку.
        // https://github.com/Azure/azure-functions-openapi-extension/issues/133
        options.UseInlineDefinitionsForEnums();


        // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        foreach (var apiAssemblyType in AppDomain.CurrentDomain.GetAssemblies())
        {
            var assemblyXml = Path.Combine(AppContext.BaseDirectory, $"{apiAssemblyType.GetName().Name}.xml");
         
            if (File.Exists(assemblyXml) is false)
                continue;

            options.IncludeXmlComments(assemblyXml);
        }

        // У рекордов поля нужно называть начиная с большой, в api хочется видеть с маленькой, поэтому:
        options.DescribeAllParametersInCamelCase();

        // Настройка глобального ответа на ошибки
        options.OperationFilter<Add500ErrorResponse>();
        options.OperationFilter<Add400ErrorResponse>();
        options.OperationFilter<WrapResponseCodeDesctiption>();
        options.OperationFilter<AuthorizeCheckOperationFilter>();
        options.DocumentFilter<ForbidHttpResultDocumentFilter>();
        options.SchemaFilter<CustomValidationSchemaFilter>();
        options.SchemaFilter<RefNullableSchemaFilter>();

        // Что бы не было пересечений по именам моделей.
        // Переопределяем идентификаторы моделей на полное имя типа
        options.CustomSchemaIds(type => type
            .FullName!
            .Replace("+", ".")
        );
    }
}