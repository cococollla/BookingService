using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Xml.XPath;
using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Common.Api.Results;

/// <summary>
/// Фильтр для добавления описания параметров с аттрибутом <see cref="CustomValidationAttribute"/> из XML документации.
/// Важно что бы для проекта в котором лежит реализация кастомного аттрибута была настройка
/// <PropertyGroup>
///     <GenerateDocumentationFile>true</GenerateDocumentationFile>
/// </PropertyGroup>
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class CustomValidationSchemaFilter : ISchemaFilter
{
    private readonly Dictionary<string, XPathDocument> _cache = new();
    
    /// <summary>
    /// Конструктор
    /// </summary>
    public CustomValidationSchemaFilter()
    {
    }

    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Проверяем, является ли тип модели классом (исключаем примитивные типы и string)
        // Если контекст описывает параметр
        if (context.ParameterInfo != null)
        {
            var customValidationAttributes = context.ParameterInfo.GetCustomAttributes<CustomValidationAttribute>();

            foreach (var attribute in customValidationAttributes)
            {
                var description = FormatDescription(attribute);
                
                if(description is null)
                    continue;
                
                AddToParameter(schema, description);    
            }
            
            return;
        }
        
        // Получаем все свойства модели
        var properties = context.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            // Проверяем, есть ли атрибут CustomValidation на свойстве
            var customValidationAttributes = property.GetCustomAttributes<CustomValidationAttribute>();

            foreach (var attribute in customValidationAttributes)
            {
                // Добавляем описание кастомной валидации к полю
                var description = FormatDescription(attribute);
                
                if(description is null)
                    continue;
                
                AddToProperty(schema, property.Name, description);
            }
        }
    }

    /// <summary>
    /// Открыть документацию
    /// </summary>
    private XPathDocument OpenXmlDocumentation(Type type)
    {
        var assembly = type.Assembly.GetName().Name!;
        
        if (_cache.TryGetValue(assembly, out var xmlDoc)) 
            return xmlDoc;
        
        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly}.xml");
        var newXmlDoc = new XPathDocument(xmlPath);
            
        _cache.Add(assembly, newXmlDoc);

        return newXmlDoc;
        
    }

    /// <summary>
    /// Формат описания валидации
    /// </summary>
    private string? FormatDescription(CustomValidationAttribute attribute)
    {
        var xmlDoc = OpenXmlDocumentation(attribute.ValidatorType);
        
        var methodInfo = attribute.ValidatorType.GetMethod(attribute.Method, BindingFlags.Public | BindingFlags.Static);
        if (methodInfo == null) 
            return null;
        
        var methodName = $"M:{attribute.ValidatorType.FullName}.{attribute.Method}(System.String,System.ComponentModel.DataAnnotations.ValidationContext)";
        var methodNode = xmlDoc.CreateNavigator().SelectSingleNode($"/doc/members/member[@name='{methodName}']/summary");

        if (methodNode == null) 
            return null;

        return $"\n\n`{methodNode.Value.Trim()}`";
        
    }

    /// <summary>
    /// Добавить описание к параметру
    /// </summary>
    private void AddToParameter(OpenApiSchema schema, string description)
    {
        schema.Description += description;
    }
    
    /// <summary>
    /// Добавить описание к свойству
    /// </summary>
    private void AddToProperty(OpenApiSchema schema, string propertyName, string description)
    {
        var camelCaseName = ToCamelCaseName(propertyName);
        
        if (!schema.Properties.TryGetValue(camelCaseName, value: out var propertySchema))
            return;

        propertySchema.Description += description;
    }

    /// <summary>
    /// Имя свойства в CamelCase
    /// </summary>
    private string ToCamelCaseName(string name) => char.ToLowerInvariant(name[0]) + name[1..];
   
}