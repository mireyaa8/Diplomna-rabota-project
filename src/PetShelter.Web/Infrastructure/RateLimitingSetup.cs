using System.Threading.RateLimiting;

namespace PetShelter.Web.Infrastructure;

/// <summary>Настройки от appsettings.json, раздел „RateLimiting“.</summary>
public class AccountRateLimitOptions
{
    /// <summary>Колко опита са позволени от един IP адрес в рамките на прозореца.</summary>
    public int PermitLimit { get; set; } = 10;

    /// <summary>Дължина на прозореца в секунди.</summary>
    public int WindowSeconds { get; set; } = 60;
}

/// <summary>
/// Ограничава опитите за вход, регистрация и смяна на забравена парола от един IP адрес.
/// Така отгатването на пароли с програма („brute force“) и масовото създаване на профили
/// стават много бавни. Обикновеното разглеждане на сайта не се ограничава.
/// </summary>
public static class RateLimitingSetup
{
    private static readonly string[] ProtectedPaths =
    [
        "/Identity/Account/Login",
        "/Identity/Account/Register",
        "/Identity/Account/ForgotPassword",
    ];

    public static IServiceCollection AddAccountRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("RateLimiting").Get<AccountRateLimitOptions>() ?? new AccountRateLimitOptions();

        services.AddRateLimiter(limiter =>
        {
            // 429 Too Many Requests → нашата страница за грешки (вж. HomeController.HandleStatusCode).
            limiter.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            limiter.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                if (!IsProtectedRequest(context.Request))
                {
                    return RateLimitPartition.GetNoLimiter("unlimited");
                }

                // Отделен брояч за всеки IP адрес.
                string ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = options.PermitLimit,
                    Window = TimeSpan.FromSeconds(options.WindowSeconds),
                    QueueLimit = 0,
                });
            });
        });

        return services;
    }

    /// <summary>Ограничават се само изпращанията на формите (POST), не отварянето на страниците.</summary>
    public static bool IsProtectedRequest(HttpRequest request)
        => HttpMethods.IsPost(request.Method)
            && ProtectedPaths.Any(path => request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase));
}
