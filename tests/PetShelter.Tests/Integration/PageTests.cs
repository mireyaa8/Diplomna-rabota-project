using System.Net;

namespace PetShelter.Tests.Integration;

/// <summary>
/// Интеграционни тестове: пускат се истински HTTP заявки към приложението
/// и се проверява какво връща то — статус код, пренасочване и съдържание.
/// </summary>
public class PageTests : IClassFixture<PetShelterWebApplicationFactory>
{
    private readonly PetShelterWebApplicationFactory factory;

    public PageTests(PetShelterWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/Privacy")]
    [InlineData("/Identity/Account/Login")]
    [InlineData("/Identity/Account/Register")]
    public async Task PublicPages_ReturnSuccess(string url)
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task HomePage_ShowsBulgarianContent()
    {
        var client = factory.CreateClient();

        string html = await client.GetStringAsync("/");

        Assert.Contains("Всяко животно заслужава дом", html);
        Assert.Contains("<html lang=\"bg\">", html);
    }

    [Fact]
    public async Task UnknownPage_ReturnsCustom404Page()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/no-such-page");
        string html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Contains("Тази страница е избягала", html);
    }

    [Fact]
    public async Task AdminDashboard_AnonymousUser_IsRedirectedToLogin()
    {
        var client = factory.CreateClientWithoutRedirects();

        var response = await client.GetAsync("/Admin/Dashboard");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task StaticFiles_AreServed()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/css/site.css");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
