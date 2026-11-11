using PetShelter.Services.Helpers;

namespace PetShelter.Services.Reports;

/// <summary>
/// Данните на един сигнал, нужни за сравнението. Отделен тип, за да не зависи
/// алгоритъмът от базата данни и да може да се тества лесно.
/// </summary>
public record ReportFingerprint(
    int Id,
    double Latitude,
    double Longitude,
    DateTime CreatedOn,
    long? ImageHash,
    string? AiSpecies,
    string? AiBreed,
    string? AiColor);

/// <summary>Прагове за откриване на дубликати. Настройват се от appsettings.json.</summary>
public class DuplicateDetectionOptions
{
    public double MaxDistanceMeters { get; set; } = 500;

    public int TimeWindowHours { get; set; } = 72;

    public int MaxHammingDistance { get; set; } = 10;
}

public interface IDuplicateReportDetector
{
    /// <summary>
    /// Връща Id на най-близкия съществуващ сигнал, който вероятно описва същото животно,
    /// или null, ако няма такъв.
    /// </summary>
    int? FindPossibleDuplicate(ReportFingerprint candidate, IEnumerable<ReportFingerprint> existingReports);
}

/// <summary>
/// Сигналът е възможен дубликат, когато е:
/// 1) на не повече от MaxDistanceMeters от друг сигнал,
/// 2) в рамките на TimeWindowHours от него, и
/// 3) снимките са визуално сходни ИЛИ AI е разпознал един и същ вид, порода и цвят.
/// </summary>
public class DuplicateReportDetector : IDuplicateReportDetector
{
    private readonly DuplicateDetectionOptions options;

    public DuplicateReportDetector(DuplicateDetectionOptions options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public int? FindPossibleDuplicate(ReportFingerprint candidate, IEnumerable<ReportFingerprint> existingReports)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(existingReports);

        var timeWindow = TimeSpan.FromHours(options.TimeWindowHours);

        return existingReports
            .Where(existing => existing.Id != candidate.Id)
            .Where(existing => (candidate.CreatedOn - existing.CreatedOn).Duration() <= timeWindow)
            .Select(existing => new
            {
                existing.Id,
                Distance = GeoHelper.DistanceInMeters(
                    candidate.Latitude, candidate.Longitude, existing.Latitude, existing.Longitude),
                Existing = existing,
            })
            .Where(x => x.Distance <= options.MaxDistanceMeters)
            .Where(x => ImagesAreSimilar(candidate, x.Existing) || AiDescriptionsMatch(candidate, x.Existing))
            .OrderBy(x => x.Distance)
            .Select(x => (int?)x.Id)
            .FirstOrDefault();
    }

    private bool ImagesAreSimilar(ReportFingerprint first, ReportFingerprint second)
        => first.ImageHash.HasValue
           && second.ImageHash.HasValue
           && ImageHashHelper.HammingDistance(first.ImageHash.Value, second.ImageHash.Value) <= options.MaxHammingDistance;

    private static bool AiDescriptionsMatch(ReportFingerprint first, ReportFingerprint second)
        => SameNonEmpty(first.AiSpecies, second.AiSpecies)
           && SameNonEmpty(first.AiBreed, second.AiBreed)
           && SameNonEmpty(first.AiColor, second.AiColor);

    private static bool SameNonEmpty(string? first, string? second)
        => !string.IsNullOrWhiteSpace(first)
           && !string.IsNullOrWhiteSpace(second)
           && string.Equals(first.Trim(), second.Trim(), StringComparison.OrdinalIgnoreCase);
}
