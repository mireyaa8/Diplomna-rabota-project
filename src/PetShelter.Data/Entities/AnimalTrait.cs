namespace PetShelter.Data.Entities;

/// <summary>
/// Свързваща таблица „много към много“ между животни и черти на характера.
/// Първичният ключ е съставен: (AnimalId, PersonalityTraitId).
/// </summary>
public class AnimalTrait
{
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    public int PersonalityTraitId { get; set; }
    public PersonalityTrait PersonalityTrait { get; set; } = null!;
}
