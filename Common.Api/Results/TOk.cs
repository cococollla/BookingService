using System;
using Microsoft.AspNetCore.Http;

namespace Common.Api.Results;

/// <summary>
/// Успешный результат
/// </summary>
/// <remarks>
/// Пока тело не оборачиваем в какой-то общий контейнер, поэтому не используем этот тип
/// </remarks>
/// <typeparam name="T">тип тела</typeparam>
// TODO: удалить если так и не нужно будет
[Obsolete("пока не используется, думаю не нужно")]
public record TOk<T>: UniversalWrap<Microsoft.AspNetCore.Http.HttpResults.Ok<TSuccessBody<T>>, TSuccessBody<T>>
{
    /// <inheritdoc />
    private TOk(Microsoft.AspNetCore.Http.HttpResults.Ok<TSuccessBody<T>> result)
        : base(result)
    {
    }

    internal static TOk<T> Create(T data)
    {
        return new TOk<T>(TypedResults.Ok(new TSuccessBody<T>(data)));
    }
}