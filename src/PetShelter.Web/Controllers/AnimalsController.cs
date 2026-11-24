using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Services.Animals;
using PetShelter.Web.Models.Animals;

namespace PetShelter.Web.Controllers;

public class AnimalsController : Controller
{
    private readonly IAnimalService animalService;

    public AnimalsController(IAnimalService animalService)
    {
        this.animalService = animalService;
    }

    /// <summary>Каталог с филтри. Достъпен и за гости.</summary>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] AnimalQueryModel query, CancellationToken cancellationToken)
    {
        var model = new AnimalCatalogViewModel
        {
            Query = query,
            Result = await animalService.SearchAsync(query, cancellationToken),
            Options = await animalService.GetFilterOptionsAsync(cancellationToken),
        };

        return View(model);
    }
}
