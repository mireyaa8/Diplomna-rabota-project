using PetShelter.Data.Enums;

namespace PetShelter.Services.Animals;

/// <summary>Възрастова група за филтъра в каталога.</summary>
public enum AgeGroup
{
    /// <summary>До 1 година.</summary>
    Young = 1,

    /// <summary>От 1 до 7 години.</summary>
    Adult = 2,

    /// <summary>Над 7 години.</summary>
    Senior = 3,
}

/// <summary>Подреждане на резултатите в каталога.</summary>
public enum AnimalSorting
{
    Newest = 0,
    Oldest = 1,
    Name = 2,
}

/// <summary>
/// Филтрите, които потребителят избира в каталога. Всички са по избор:
/// празен филтър означава „без ограничение“.
/// </summary>
public class AnimalQueryModel
{
    public const int PageSize = 12;

    public string? SearchTerm { get; set; }

    public int? SpeciesId { get; set; }

    public int? BreedId { get; set; }

    public int? ShelterId { get; set; }

    public Gender? Gender { get; set; }

    public AnimalSize? Size { get; set; }

    public AgeGroup? Age { get; set; }

    /// <summary>Животното трябва да има всички избрани черти.</summary>
    public List<int> TraitIds { get; set; } = new();

    public AnimalSorting Sorting { get; set; } = AnimalSorting.Newest;

    public int Page { get; set; } = 1;
}

/// <summary>Данните за една карта в каталога.</summary>
public record AnimalCardModel(
    int Id,
    string Name,
    string SpeciesName,
    string? BreedName,
    Gender Gender,
    AnimalSize Size,
    int? AgeInYears,
    string? MainImageUrl,
    string ShelterName,
    string City,
    AnimalStatus Status);

/// <summary>Една страница от резултатите.</summary>
public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;
}

/// <summary>Елемент от падащо меню. ParentId е видът при породите.</summary>
public record SelectOption(int Id, string Name, int? ParentId = null);

/// <summary>Стойностите за падащите менюта и отметките във филтрите.</summary>
public record AnimalFilterOptions(
    IReadOnlyList<SelectOption> Species,
    IReadOnlyList<SelectOption> Breeds,
    IReadOnlyList<SelectOption> Shelters,
    IReadOnlyList<SelectOption> Traits);
