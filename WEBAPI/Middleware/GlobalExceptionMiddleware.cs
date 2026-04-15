using System.Net;
using WEBAPI.Application.Common;

namespace WEBAPI.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    // uygulama genelinde beklenmeyen hatalarý yakalayarak merkezi bir þekilde yönetmek ve kullanýcýya anlamlý hata mesajlarý döndürmek için bu middleware'i oluþturdum.
    // response da hata durumunda standart bir formatta JSON döndürerek frontend'in hata durumlarýný daha kolay iþlemesini saðlýyorum.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            var result = ServiceResult.Failure(
                $"Beklenmeyen bir hata olustu. Detay: {ex.Message}",
                HttpStatusCode.InternalServerError);

            context.Response.StatusCode = (int)result.StatusCode;
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
