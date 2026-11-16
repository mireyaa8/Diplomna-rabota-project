using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PetShelter.Tests.Integration;

/// <summary>
/// Тестов „вход“: заявката се смята за влязъл потребител, ако има заглавка X-Test-Role.
/// Така тестовете проверяват правата по роли, без да минават през формата за вход.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Test";
    public const string RoleHeader = "X-Test-Role";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(RoleHeader, out var role))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Name, "test@petshelter.test"),
            new Claim(ClaimTypes.Role, role.ToString()),
        };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName)));
    }
}

/// <summary>
/// Фабрика с тестов вход: заявките със заглавка X-Test-Role минават през TestAuthHandler,
/// а всички останали — през обикновения вход с бисквитка (Identity).
/// </summary>
public class AuthenticatedWebApplicationFactory : PetShelterWebApplicationFactory
{
    private const string SelectorScheme = "TestOrIdentity";

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { })
                .AddPolicyScheme(SelectorScheme, SelectorScheme, options =>
                {
                    options.ForwardDefaultSelector = context =>
                        context.Request.Headers.ContainsKey(TestAuthHandler.RoleHeader)
                            ? TestAuthHandler.SchemeName
                            : IdentityConstants.ApplicationScheme;
                });

            // PostConfigure се изпълнява след настройките на Identity и ги замества.
            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = SelectorScheme;
                options.DefaultChallengeScheme = SelectorScheme;
                options.DefaultForbidScheme = SelectorScheme;
            });
        });
    }

    /// <summary>Клиент, „влязъл“ с дадената роля.</summary>
    public HttpClient CreateClientWithRole(string role)
    {
        var client = CreateClientWithoutRedirects();
        client.DefaultRequestHeaders.Add(TestAuthHandler.RoleHeader, role);
        return client;
    }
}
