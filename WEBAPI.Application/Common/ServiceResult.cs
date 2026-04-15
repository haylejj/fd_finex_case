using System.Net;
using System.Text.Json.Serialization;

namespace WEBAPI.Application.Common;

/// <summary>
/// Servis katmaný iþlemlerinin sonuçlarýný standart bir yapýda döndürmek için kullanýlan temel sýnýftýr.
/// Ýþlemin baþarýlý olup olmadýðýný, hata mesajlarýný ve ilgili HTTP durum kodunu içerir.
/// </summary>
public class ServiceResult
{
    public bool IsSuccess { get; protected set; }
    public List<string>? ErrorList { get; set; }

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }

    public string? UrlAsCreated { get; set; }

    public static ServiceResult Success(HttpStatusCode statusCode)
    {
        return new ServiceResult
        {
            StatusCode = statusCode,
            IsSuccess = true
        };
    }

    public static ServiceResult SuccessAsCreated(string? url = null)
    {
        return new ServiceResult
        {
            StatusCode = HttpStatusCode.Created,
            UrlAsCreated = url,
            IsSuccess = true
        };
    }

    public static ServiceResult Failure(List<string> errorList, HttpStatusCode statusCode)
    {
        return new ServiceResult
        {
            ErrorList = errorList,
            StatusCode = statusCode,
            IsSuccess = false
        };
    }

    public static ServiceResult Failure(string errorMessage, HttpStatusCode statusCode)
    {
        return new ServiceResult
        {
            ErrorList = [errorMessage],
            StatusCode = statusCode,
            IsSuccess = false
        };
    }
}
/// <summary>
/// Servis katmaný iþlemlerinin sonuçlarýný standart bir yapýda döndürmek için kullanýlan temel sýnýftýr.
/// Ýþlemin baþarýlý olup olmadýðýný, hata mesajlarýný ve ilgili HTTP durum kodunu içerir.
/// </summary>
public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public static ServiceResult<T> Success(T data, HttpStatusCode statusCode)
    {
        return new ServiceResult<T>
        {
            Data = data,
            StatusCode = statusCode,
            IsSuccess = true
        };
    }

    public static ServiceResult<T> SuccessAsCreated(T data, string? url = null)
    {
        return new ServiceResult<T>
        {
            Data = data,
            StatusCode = HttpStatusCode.Created,
            UrlAsCreated = url,
            IsSuccess = true
        };
    }

    public new static ServiceResult<T> Failure(List<string> errorList, HttpStatusCode statusCode)
    {
        return new ServiceResult<T>
        {
            ErrorList = errorList,
            StatusCode = statusCode,
            IsSuccess = false
        };
    }

    public new static ServiceResult<T> Failure(string errorMessage, HttpStatusCode statusCode)
    {
        return new ServiceResult<T>
        {
            ErrorList = [errorMessage],
            StatusCode = statusCode,
            IsSuccess = false
        };
    }
}
