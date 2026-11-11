using System.ComponentModel.DataAnnotations;
using static PetShelter.Data.Common.DataConstants.Shelter;

namespace PetShelter.Data.Entities;

/// <summary>
/// Приют за животни. Координатите се използват за избор на най-близкия приют при сигнал.
/// </summary>
public class Shelter
{
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(AddressMaxLength)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(CityMaxLength)]
    public string City { get; set; } = string.Empty;

    /// <summary>Географска ширина (от -90 до 90).</summary>
    public double Latitude { get; set; }

    /// <summary>Географска дължина (от -180 до 180).</summary>
    public double Longitude { get; set; }

    [MaxLength(PhoneMaxLength)]
    public string? Phone { get; set; }

    [MaxLength(EmailMaxLength)]
    public string? Email { get; set; }

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>Меко изтриване: записът остава, но не се показва.</summary>
    public bool IsDeleted { get; set; }

    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    public ICollection<ApplicationUser> Staff { get; set; } = new List<ApplicationUser>();
    public ICollection<StrayReport> AssignedReports { get; set; } = new List<StrayReport>();
}
