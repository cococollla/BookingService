using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Common.Api.Results;

/// <inheritdoc />
public record GConflict<T> : UniversalWrap<Conflict<T>, T>
{
    /// <inheritdoc />
    private GConflict(Conflict<T> result)
        : base(result)
    {
    }
    
    internal static GConflict<T> Create(T body)
    {
        return new GConflict<T>(TypedResults.Conflict(body));
    }
}
