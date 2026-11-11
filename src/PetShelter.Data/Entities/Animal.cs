using System.ComponentModel.DataAnnotations;
using PetShelter.Data.Enums;
using static PetShelter.Data.Common.DataConstants.Animal;

namespace PetShelter.Data.Entities;

/// <summary>
/// Животно за осиновяване. Принадлежи на един приют, има вид, порода (по избор),
/// снимки и черти на характера.
/// </summary>
public class Animal
{
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    public int SpeciesId { get; set; }
    public Species Species { get; set; } = null!;

    /// <summary>Null, когато породата не е известна.</summary>
    public int? BreedId { get; set; }
    public Breed? Breed { get; set; }

    public int ShelterId { get; set; }
    public Shelter Shelter { get; set; } = null!;

    public Gender Gender { get; set; }

    public AnimalSize Size { get; set; }

    /// <summary>
    /// Дата на раждане. При бездомните животни често е приблизителна —
    /// пазим датата, а не възрастта, за да не „остарява“ записът.
    /// </summary>
    public DateOnly? BirthDate { get; set; }

    public bool IsBirthDateApproximate { get; set; } = true;

    [MaxLength(ColorMaxLength)]
    public string? Color { get; set; }

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Навици: режим, разходки, храна, поведение у дома.</summary>
    [MaxLength(HabitsMaxLength)]
    public string? Habits { get; set; }

    [MaxLength(HealthInfoMaxLength)]
    public string? HealthInfo { get; set; }

    public bool IsVaccinated { get; set; }

    public bool IsNeutered { get; set; }

    public AnimalStatus Status { get; set; } = AnimalStatus.Available;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }

    public ICollection<AnimalImage> Images { get; set; } = new List<AnimalImage>();
    public ICollection<AnimalTrait> Traits { get; set; } = new List<AnimalTrait>();
    public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
    public ICollection<FavoriteAnimal> FavoritedBy { get; set; } = new List<FavoriteAnimal>();
}
