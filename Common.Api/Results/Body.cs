namespace Common.Api.Results;

/// <summary>
/// Типичное тело успешного ответа
/// </summary>
/// <param name="Data">полезная нагрузка</param>
/// <typeparam name="T">тип полезной нагрузки</typeparam>
public record TSuccessBody<T>(T Data);

/// <summary>
/// Типичное тело ответа при ошибке
/// </summary>
public record TErrorBody
{
    /// <summary>текст ошибки</summary>
    public required string Error { get; init; }
}

/// <summary>
/// Типичное тело ответа при 400
/// </summary>
public record TErrorsBody
{
    /// <summary>ошибки</summary>
    public required string[] Errors { get; init; }
}
