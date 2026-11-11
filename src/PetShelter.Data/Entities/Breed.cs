using System.ComponentModel.DataAnnotations;
using static PetShelter.Data.Common.DataConstants.Breed;

namespace PetShelter.Data.Entities;

/// <summary>Порода. Всяка порода принадлежи на точно един вид.</summary>
public class Breed
{
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    public int SpeciesId { get; set; }
    public Species Species { get; set; } = null!;

    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
