using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Common.Api.Results;

/// <inheritdoc />
public record GBadRequest<TBody> : UniversalWrap<BadRequest<TBody>, TBody>
{
    /// <inheritdoc />
    private GBadRequest(BadRequest<TBody> result)
        : base(result)
    {
    }

    internal static GBadRequest<TBody> Create(TBody body)
    {
        return new GBadRequest<TBody>(TypedResults.BadRequest(body));
    }
    
}