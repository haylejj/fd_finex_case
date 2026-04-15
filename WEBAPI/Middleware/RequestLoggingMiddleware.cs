using System.Diagnostics;

namespace WEBAPI.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    // burada basit bir þekilde her gelen isteðin methodunu, path'ini ve iþlenme süresini loglamak için bu middleware'i oluþturdum. Bu sayede uygulamanýn performansýný izleyebilir ve hangi endpointlerin daha uzun sürdüðünü görebilirim.
    // bunun yerine serilog kütüphanesinin daha geliþmiþ enricher'larýný kullanarak loglara daha fazla bilgi ekleyebilir ve loglama iþlemini daha merkezi bir þekilde yönetebilirim.
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await next(context);
        stopwatch.Stop();

        Console.WriteLine(
            "[Request] {0} {1} took {2}ms",
            context.Request.Method,
            context.Request.Path,
            stopwatch.ElapsedMilliseconds);
    }
}
