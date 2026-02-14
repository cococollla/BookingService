using System.Reflection;
using System.Runtime.CompilerServices;
using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Common.Api.Swagger;

/// <summary>
/// Расширение для swagger.
/// Обрабатывает nullable ref types (string?, object?)
/// Если тип параметра или поля  nullable ref types тогда проставляется Nullable = true
/// Если тип просто ref то проставляется Nullable = false
/// </summary>
public class RefNullableSchemaFilter : ISchemaFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Если контекст описывает параметр
        if (context.ParameterInfo != null)
        {
            if (context.ParameterInfo.ParameterType.IsValueType)
                return;

            schema.Nullable = IsNullableType(context.ParameterInfo);
            
            return;
        }
        
        // Если контекст описывает члена объекта
        if (context.MemberInfo == null)
            return;
        
        // Только для свойств
        if (context.MemberInfo is not PropertyInfo propertyInfo)
            return;
        
        // Откидвываем не ссылочные типы.
        if (propertyInfo.PropertyType.IsValueType)
            return;

        // Тут знаем что поле ссылочное либо nullable лдибо нет. 
        // Метод IsNonNullableReferenceType законный в отличие от метода ниже IsNullableType.
        schema.Nullable = !propertyInfo.IsNonNullableReferenceType();
       
    }
    
    // TODO: эта штука сомнительная возможно отвалится когда-то
    // - почему не законно https://learn.microsoft.com/ru-ru/dotnet/api/system.runtime.compilerservices.nullableattribute?view=net-8.0
    // - возможно в net9 не нужна будет или появится метод для ParameterInfo
    // - не понимаю почему нет медода как для PropertyInfo
    private static bool IsNullableType(ParameterInfo parameterInfo)
    {
        var nullableAttribute = parameterInfo.GetCustomAttribute<NullableAttribute>();

        if (nullableAttribute == null)
            return false;
        
        return nullableAttribute.NullableFlags[0] == 2;
    }
}