using PetShelter.Services.Animals;

namespace PetShelter.Web.Models.Animals;

/// <summary>Всичко, което страницата на каталога показва.</summary>
public class AnimalCatalogViewModel
{
    public AnimalQueryModel Query { get; init; } = new();

    public PagedResult<AnimalCardModel> Result { get; init; } = null!;

    public AnimalFilterOptions Options { get; init; } = null!;
}
