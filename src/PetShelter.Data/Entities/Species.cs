using System.ComponentModel.DataAnnotations;
using static PetShelter.Data.Common.DataConstants.Species;

namespace PetShelter.Data.Entities;

/// <summary>Вид животно: куче, котка и т.н.</summary>
public class Species
{
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Breed> Breeds { get; set; } = new List<Breed>();
    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
