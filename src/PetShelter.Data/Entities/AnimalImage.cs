using System.ComponentModel.DataAnnotations;
using static PetShelter.Data.Common.DataConstants.AnimalImage;

namespace PetShelter.Data.Entities;

/// <summary>Снимка на животно. В базата пазим само пътя до файла, не самата снимка.</summary>
public class AnimalImage
{
    public int Id { get; set; }

    [Required]
    [MaxLength(UrlMaxLength)]
    public string Url { get; set; } = string.Empty;

    /// <summary>Основната снимка се показва в каталога.</summary>
    public bool IsMain { get; set; }

    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
}
