using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Common.Api.Results;

/// <inheritdoc />
public record TConflict : UniversalWrap<Conflict<TErrorBody>, TErrorBody>
{
    /// <inheritdoc />
    private TConflict(Conflict<TErrorBody> result)
        : base(result)
    {
    }
    
    internal static TConflict Create(string error)
    {
        return new TConflict(TypedResults.Conflict(new TErrorBody
        {
            Error = error  
        }));
    }
}
