using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PetShelter.Data;

namespace PetShelter.Tests.Integration;

/// <summary>
/// Стартира цялото уеб приложение в паметта (без браузър и без истински сървър),
/// за да могат тестовете да изпращат HTTP заявки и да проверяват отговорите.
/// Вместо SQL Server се използва база в паметта.
/// </summary>
public class PetShelterWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string databaseName = $"PetShelterTests_{Guid.NewGuid()}";

    /// <summary>Лимит за опитите за вход в тестовете. Наследниците могат да го сменят.</summary>
    protected virtual int RateLimitPermitLimit => 1000;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Program.cs не пуска миграциите в среда „Testing“.
        builder.UseEnvironment("Testing");
        builder.UseSetting("RateLimiting:PermitLimit", RateLimitPermitLimit.ToString());

        builder.ConfigureServices(services =>
        {
            // Махаме настройката за SQL Server и слагаме база в паметта.
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<DbContextOptions>();
            services.RemoveAll<IDbContextOptionsConfiguration<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Създаваме базата с началните данни (видове, породи, демо приют).
        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();

        return host;
    }

    /// <summary>Клиент, който НЕ следва пренасочванията — за да проверим къде пренасочва сайтът.</summary>
    public HttpClient CreateClientWithoutRedirects()
        => CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
}
