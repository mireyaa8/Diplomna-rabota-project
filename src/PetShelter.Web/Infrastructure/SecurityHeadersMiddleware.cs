namespace PetShelter.Web.Infrastructure;

/// <summary>
/// Добавя HTTP заглавки за сигурност към всеки отговор. Те казват на браузъра
/// какво му е позволено да прави със страницата и така намаляват щетите от XSS,
/// clickjacking и изтичане на информация.
/// </summary>
public class SecurityHeadersMiddleware
{
    // Откъде браузърът може да зарежда ресурси. Позволени са само нашият сайт и CDN-ът
    // за Bootstrap (cdn.jsdelivr.net). Вградени скриптове (<script>…</script>) не са позволени.
    private const string ContentSecurityPolicy =
        "default-src 'self'; " +
        "script-src 'self' https://cdn.jsdelivr.net; " +
        "style-src 'self' https://cdn.jsdelivr.net; " +
        "font-src 'self' https://cdn.jsdelivr.net; " +
        "img-src 'self' data: https://*.tile.openstreetmap.org; " +
        "connect-src 'self'; " +
        "frame-ancestors 'none'; " +
        "form-action 'self'; " +
        "base-uri 'self'; " +
        "object-src 'none'";

    private readonly RequestDelegate next;
    private readonly bool isDevelopment;
    private readonly string contentSecurityPolicy;

    public SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        this.next = next;
        isDevelopment = environment.IsDevelopment();

        // При разработка Visual Studio презарежда страницата през WebSocket (Hot Reload).
        contentSecurityPolicy = environment.IsDevelopment()
            ? ContentSecurityPolicy.Replace("connect-src 'self'", "connect-src 'self' ws: wss:")
            : ContentSecurityPolicy;
    }

    public Task InvokeAsync(HttpContext context)
    {
        // Заглавките се задават точно преди отговорът да тръгне към браузъра.
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;

            // Страницата с подробности за грешка при разработка има вградени стилове и скриптове,
            // затова при нея не слагаме CSP. В Production тази страница не се показва.
            bool isDeveloperErrorPage = isDevelopment && context.Response.StatusCode >= 500;
            if (!isDeveloperErrorPage)
            {
                headers["Content-Security-Policy"] = contentSecurityPolicy;
            }

            // Браузърът не „отгатва“ типа на файла (напр. не изпълнява качен .jpg като скрипт).
            headers["X-Content-Type-Options"] = "nosniff";

            // Сайтът не може да бъде вграден в чужда страница (защита от clickjacking).
            headers["X-Frame-Options"] = "DENY";

            // При преход към чужд сайт се изпраща само домейнът, не целият адрес.
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Камерата и микрофонът не са нужни. Местоположението ще трябва за сигналите (Етап 3).
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(self)";

            return Task.CompletedTask;
        });

        return next(context);
    }
}

public static class SecurityHeadersExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        => app.UseMiddleware<SecurityHeadersMiddleware>();
}
