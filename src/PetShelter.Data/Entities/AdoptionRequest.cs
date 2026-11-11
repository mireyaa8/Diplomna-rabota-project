using System.ComponentModel.DataAnnotations;
using PetShelter.Data.Enums;
using static PetShelter.Data.Common.DataConstants.AdoptionRequest;

namespace PetShelter.Data.Entities;

/// <summary>Заявка от потребител за осиновяване на конкретно животно.</summary>
public class AdoptionRequest
{
    public int Id { get; set; }

    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    /// <summary>Защо кандидатът иска да осинови животното, какви са условията у дома.</summary>
    [Required]
    [MaxLength(MessageMaxLength)]
    public string Message { get; set; } = string.Empty;

    public AdoptionRequestStatus Status { get; set; } = AdoptionRequestStatus.Pending;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>Кога служител на приюта е одобрил или отказал заявката.</summary>
    public DateTime? ReviewedOn { get; set; }
}
