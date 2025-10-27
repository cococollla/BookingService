using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Common.Api.Results;

// ReSharper disable InconsistentNaming - намеренно с маленькой буквы, не пересекается с методами контроллера и короткое название
/// <summary>
/// Расширения для контроллеров, для возврата результатов
/// </summary>
public static class ResultsExtensions
{
    /// <summary>
    /// 200
    /// </summary>
    /// <returns>200 результат вызова api</returns>
    public static Ok ok()
    {
        return  TypedResults.Ok();
    }
    
    /// <summary>
    /// 200
    /// </summary>
    /// <param name="data">данные</param>
    /// <typeparam name="T">тип полезных данных</typeparam>
    /// <returns>200 результат вызова api</returns>
    public static Ok<T> ok<T>(T data)
    {
        return  TypedResults.Ok(data);
    }

    /// <summary>
    /// Не авторизован
    /// </summary>
    /// <returns>401 результат вызова api</returns>
    public static  UnauthorizedHttpResult unauthorized()
    {
        return  TypedResults.Unauthorized();
    }
    
    
    /// <summary>
    /// 404
    /// </summary>
    /// <param name="error">текст оибки</param>
    /// <returns>404 результат вызова api</returns>
    public static TNotFound notFound(string error)
    {
        return TNotFound.Create(error);
    }
    
    /// <summary>
    /// 400
    /// </summary>
    /// <param name="msg">сообщение</param>
    /// <returns>400 результат вызова api</returns>
    public static TBadRequest badRequest(string msg)
    {
        return TBadRequest.Create(msg);
    }
    
    /// <summary>
    /// 400
    /// </summary>
    /// <param name="msgs">сообщения</param>
    /// <returns>400 результат вызова api</returns>
    public static TBadRequest badRequest(string[] msgs)
    {
        return TBadRequest.Create(msgs);
    }

    /// <summary>
    /// 409
    /// </summary>
    /// <param name="msg">сообщение</param>
    /// <returns>409 результат вызова api</returns>
    public static TConflict conflict(string msg)
    {
        return TConflict.Create(msg);
    }
    
    /// <summary>
    /// 409
    /// </summary>
    /// <param name="body">тело</param>
    /// <returns>409 результат вызова api</returns>
    public static GConflict<T> conflict<T>(T body)
    {
        return GConflict<T>.Create(body);
    }
    
    
    /// <summary>
    /// 400
    /// </summary>
    /// <param name="body">тело</param>
    /// <returns>400 результат вызова api</returns>
    public static GBadRequest<T> badRequest<T>(T body)
    {
        return GBadRequest<T>.Create(body);
    }
    
    
    /// <summary>
    /// 403
    /// </summary>
    /// <returns>403 результат вызова api</returns>
    public static ForbidHttpResult forbid()
    {
        return TypedResults.Forbid();
    }

}