using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Common.Api.Results;

/// <inheritdoc />
public record TBadRequest : UniversalWrap<BadRequest<TErrorsBody>, TErrorsBody>
{
    /// <inheritdoc />
    private TBadRequest(BadRequest<TErrorsBody> result)
        : base(result)
    {
    }

    internal static TBadRequest Create(string error)
    {
        return new TBadRequest(TypedResults.BadRequest(new TErrorsBody
        {
            Errors = [error]
        }));
    }

    internal static TBadRequest Create(string[] errors)
    {
        return new TBadRequest(TypedResults.BadRequest(new TErrorsBody
        {
            Errors = errors
        }));
    }
}