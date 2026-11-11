using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static PetShelter.Data.Common.DataConstants.User;

namespace PetShelter.Data.Entities;

/// <summary>
/// Потребител на системата. Наследява IdentityUser (имейл, парола, телефон и др.)
/// и добавя име, фамилия и приют — за служителите на приюти.
/// </summary>
public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(NameMaxLength)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(NameMaxLength)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>Приютът, в който работи служителят. Null за обикновени потребители.</summary>
    public int? ShelterId { get; set; }
    public Shelter? Shelter { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
    public ICollection<StrayReport> StrayReports { get; set; } = new List<StrayReport>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<FavoriteAnimal> FavoriteAnimals { get; set; } = new List<FavoriteAnimal>();
}
