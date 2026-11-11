using System.ComponentModel.DataAnnotations;
using static PetShelter.Data.Common.DataConstants.PersonalityTrait;

namespace PetShelter.Data.Entities;

/// <summary>Черта на характера, например „Разбира се с деца“ или „Енергичен“.</summary>
public class PersonalityTrait
{
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    public ICollection<AnimalTrait> Animals { get; set; } = new List<AnimalTrait>();
}
