using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;

namespace Common.Api.Results;

/// <summary>
/// Универсальная обертка для <see cref="IResult"/>
/// </summary>
/// <typeparam name="TResult">тип результата</typeparam>
/// <typeparam name="TValue">тип значения</typeparam>
public record UniversalWrap<TResult, TValue> : IResult
    , IEndpointMetadataProvider
    , IStatusCodeHttpResult
    , IValueHttpResult
    , IValueHttpResult<TValue>
    where TResult : IEndpointMetadataProvider
    , IResult
    , IStatusCodeHttpResult
    , IValueHttpResult<TValue>
{
    private readonly TResult _result;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="result">результат</param>
    protected UniversalWrap(TResult result)
    {
        _result = result;
    }

    /// <inheritdoc />
    public Task ExecuteAsync(HttpContext httpContext)
    {
        return _result.ExecuteAsync(httpContext);
    }

    /// <inheritdoc />
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        TResult.PopulateMetadata(method, builder);
    }

    /// <inheritdoc />
    public int? StatusCode => _result.StatusCode;

    object? IValueHttpResult.Value => _result.Value;

    /// <inheritdoc />
    public TValue? Value => _result.Value;
}