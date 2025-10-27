using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Common.Api.Results;

/// <summary>
/// 404 типичный NotFound HTTP результат
/// </summary>
/// <remarks>
/// TNotFound сокращение от TypicalNotFound
/// </remarks>
// ReSharper disable once InconsistentNaming - TNotFound сокращение от TypicalNotFound
public record TNotFound : UniversalWrap<NotFound<TErrorBody>, TErrorBody>
{
    /// <inheritdoc />
    private TNotFound(NotFound<TErrorBody> result)
        : base(result)
    {
    }

    internal static TNotFound Create(string error)
    {
        return new TNotFound(TypedResults.NotFound(new TErrorBody
        {
            Error = error
        }));
    }
}

