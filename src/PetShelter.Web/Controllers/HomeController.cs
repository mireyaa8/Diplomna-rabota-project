using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Web.Models;

namespace PetShelter.Web.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    /// <summary>Собствена страница за необработени грешки (500).</summary>
    [IgnoreAntiforgeryToken]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    /// <summary>
    /// Извиква се от UseStatusCodePagesWithReExecute за отговори 4xx/5xx без съдържание.
    /// </summary>
    [Route("Home/StatusCode/{code:int}")]
    [IgnoreAntiforgeryToken] // грешката може да дойде и от POST заявка
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult HandleStatusCode(int code)
    {
        // Запазваме истинския код, дори когато сме стигнали тук чрез пренасочване.
        Response.StatusCode = code;

        return code switch
        {
            404 => View("NotFound"),
            403 => View("Forbidden"),
            _ => View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }),
        };
    }
}
