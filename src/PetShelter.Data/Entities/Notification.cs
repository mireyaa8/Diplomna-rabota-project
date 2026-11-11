using System.ComponentModel.DataAnnotations;
using static PetShelter.Data.Common.DataConstants.Notification;

namespace PetShelter.Data.Entities;

/// <summary>Известие в системата (например „Нов сигнал близо до вашия приют“).</summary>
public class Notification
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    [Required]
    [MaxLength(MessageMaxLength)]
    public string Message { get; set; } = string.Empty;

    /// <summary>Относителен адрес, към който води известието, например /StrayReports/Details/5.</summary>
    [MaxLength(LinkMaxLength)]
    public string? Link { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
