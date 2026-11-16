using System.Net;
using Microsoft.AspNetCore.Http;
using PetShelter.Web.Infrastructure;

namespace PetShelter.Tests.Integration;

/// <summary>Фабрика с лимит от 3 опита, за да се стигне бързо до отказа.</summary>
public class LowRateLimitWebApplicationFactory : PetShelterWebApplicationFactory
{
    public const int Limit = 3;

    protected override int RateLimitPermitLimit => Limit;
}

public class RateLimitingTests : IClassFixture<LowRateLimitWebApplicationFactory>
{
    private readonly LowRateLimitWebApplicationFactory factory;

    public RateLimitingTests(LowRateLimitWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task LoginPost_OverLimit_Returns429Page()
    {
        var client = factory.CreateClient();
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Input.Email"] = "someone@example.com",
            ["Input.Password"] = "wrong-password",
        });

        // Първите опити не са блокирани от ограничението (връщат 400 заради липсващия CSRF токен).
        for (int i = 0; i < LowRateLimitWebApplicationFactory.Limit; i++)
        {
            var allowed = await client.PostAsync("/Identity/Account/Login", form);
            Assert.NotEqual(HttpStatusCode.TooManyRequests, allowed.StatusCode);
        }

        var blocked = await client.PostAsync("/Identity/Account/Login", form);
        string html = await blocked.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.TooManyRequests, blocked.StatusCode);
        Assert.Contains("Твърде много опити", html);

        // Отварянето на страницата (GET) не се ограничава.
        var page = await client.GetAsync("/Identity/Account/Login");
        Assert.Equal(HttpStatusCode.OK, page.StatusCode);
    }

    [Theory]
    [InlineData("POST", "/Identity/Account/Login", true)]
    [InlineData("POST", "/identity/account/register", true)]
    [InlineData("POST", "/Identity/Account/ForgotPassword", true)]
    [InlineData("GET", "/Identity/Account/Login", false)]
    [InlineData("POST", "/Home/Index", false)]
    public void IsProtectedRequest_OnlyAccountFormPosts(string method, string path, bool expected)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;

        Assert.Equal(expected, RateLimitingSetup.IsProtectedRequest(context.Request));
    }
}
