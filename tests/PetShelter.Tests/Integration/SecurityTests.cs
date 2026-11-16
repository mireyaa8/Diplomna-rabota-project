using System.Net;
using PetShelter.Data.Common;

namespace PetShelter.Tests.Integration;

/// <summary>Права по роли и HTTP заглавки за сигурност.</summary>
public class SecurityTests : IClassFixture<AuthenticatedWebApplicationFactory>
{
    private readonly AuthenticatedWebApplicationFactory factory;

    public SecurityTests(AuthenticatedWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task AdminDashboard_Administrator_ReturnsDashboard()
    {
        var client = factory.CreateClientWithRole(RoleNames.Administrator);

        var response = await client.GetAsync("/Admin/Dashboard");
        string html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Админ панел", html);
    }

    [Theory]
    [InlineData(RoleNames.User)]
    [InlineData(RoleNames.ShelterStaff)]
    public async Task AdminDashboard_OtherRoles_ReturnCustom403Page(string role)
    {
        var client = factory.CreateClientWithRole(role);

        var response = await client.GetAsync("/Admin/Dashboard");
        string html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Contains("Нямате достъп", html);
    }

    [Fact]
    public async Task AdminDashboard_AnonymousUser_IsStillRedirectedToLogin()
    {
        var client = factory.CreateClientWithoutRedirects();

        var response = await client.GetAsync("/Admin/Dashboard");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/no-such-page")]
    public async Task Responses_IncludeSecurityHeaders(string url)
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Equal("strict-origin-when-cross-origin", response.Headers.GetValues("Referrer-Policy").Single());

        string csp = response.Headers.GetValues("Content-Security-Policy").Single();
        Assert.Contains("default-src 'self'", csp);
        Assert.Contains("frame-ancestors 'none'", csp);
        Assert.DoesNotContain("unsafe-inline", csp);
    }
}
