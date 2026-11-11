using System.ComponentModel.DataAnnotations;
using PetShelter.Data.Enums;
using static PetShelter.Data.Common.DataConstants.StrayReport;

namespace PetShelter.Data.Entities;

/// <summary>
/// Сигнал за бездомно животно: снимка, местоположение, резултат от AI анализа
/// и връзка към възможен повторен сигнал.
/// </summary>
public class StrayReport
{
    public int Id { get; set; }

    [Required]
    public string ReporterId { get; set; } = string.Empty;
    public ApplicationUser Reporter { get; set; } = null!;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    [MaxLength(AddressMaxLength)]
    public string? Address { get; set; }

    [Required]
    [MaxLength(ImageUrlMaxLength)]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// 64-битов перцептивен хеш (difference hash) на снимката.
    /// Близки хешове означават визуално сходни снимки.
    /// </summary>
    public long? ImageHash { get; set; }

    public StrayReportStatus Status { get; set; } = StrayReportStatus.New;

    /// <summary>Най-близкият приют, който е получил сигнала.</summary>
    public int? AssignedShelterId { get; set; }
    public Shelter? AssignedShelter { get; set; }

    // --- Резултат от AI анализа ---

    public bool? AiIsAnimal { get; set; }

    [MaxLength(AiFieldMaxLength)]
    public string? AiSpecies { get; set; }

    [MaxLength(AiFieldMaxLength)]
    public string? AiBreed { get; set; }

    [MaxLength(AiFieldMaxLength)]
    public string? AiColor { get; set; }

    [MaxLength(AiDescriptionMaxLength)]
    public string? AiDescription { get; set; }

    /// <summary>Увереност на модела от 0 до 1.</summary>
    public double? AiConfidence { get; set; }

    // --- Откриване на повторни сигнали ---

    /// <summary>Сигналът, на който този вероятно е дубликат. Служителят решава.</summary>
    public int? PossibleDuplicateOfId { get; set; }
    public StrayReport? PossibleDuplicateOf { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedOn { get; set; }
}
