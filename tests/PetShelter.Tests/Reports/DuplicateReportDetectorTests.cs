using PetShelter.Services.Reports;

namespace PetShelter.Tests.Reports;

public class DuplicateReportDetectorTests
{
    private const double Lat = 42.6977, Lon = 23.3219;
    private static readonly DateTime Now = new(2026, 11, 6, 12, 0, 0, DateTimeKind.Utc);

    private readonly DuplicateReportDetector detector = new(new DuplicateDetectionOptions
    {
        MaxDistanceMeters = 500,
        TimeWindowHours = 72,
        MaxHammingDistance = 10,
    });

    private static ReportFingerprint Report(
        int id,
        double lonOffset = 0,
        double hoursAgo = 0,
        long? hash = 0b1111_0000L,
        string? species = null,
        string? breed = null,
        string? color = null)
        => new(id, Lat, Lon + lonOffset, Now.AddHours(-hoursAgo), hash, species, breed, color);

    [Fact]
    public void FindPossibleDuplicate_NearRecentAndSimilarImage_ReturnsExistingId()
    {
        var candidate = Report(10);
        var existing = Report(1, lonOffset: 0.002, hoursAgo: 5, hash: 0b1111_0001L); // ~160 м, 1 бит разлика

        Assert.Equal(1, detector.FindPossibleDuplicate(candidate, [existing]));
    }

    [Fact]
    public void FindPossibleDuplicate_TooFarAway_ReturnsNull()
    {
        var candidate = Report(10);
        var existing = Report(1, lonOffset: 0.02); // ~1.6 км

        Assert.Null(detector.FindPossibleDuplicate(candidate, [existing]));
    }

    [Fact]
    public void FindPossibleDuplicate_TooOld_ReturnsNull()
    {
        var candidate = Report(10);
        var existing = Report(1, hoursAgo: 100);

        Assert.Null(detector.FindPossibleDuplicate(candidate, [existing]));
    }

    [Fact]
    public void FindPossibleDuplicate_DifferentImagesButSameAiDescription_ReturnsExistingId()
    {
        var candidate = Report(10, hash: 0L, species: "Куче", breed: "Смесена порода", color: "черен");
        var existing = Report(1, hash: -1L, species: "куче", breed: "Смесена порода ", color: "Черен");

        Assert.Equal(1, detector.FindPossibleDuplicate(candidate, [existing]));
    }

    [Fact]
    public void FindPossibleDuplicate_DifferentImagesAndDifferentAnimal_ReturnsNull()
    {
        var candidate = Report(10, hash: 0L, species: "Куче", breed: "Хъски", color: "сив");
        var existing = Report(1, hash: -1L, species: "Котка", breed: "Смесена порода", color: "бял");

        Assert.Null(detector.FindPossibleDuplicate(candidate, [existing]));
    }

    [Fact]
    public void FindPossibleDuplicate_MissingHashAndAiData_ReturnsNull()
    {
        var candidate = Report(10, hash: null);
        var existing = Report(1, hash: null);

        Assert.Null(detector.FindPossibleDuplicate(candidate, [existing]));
    }

    [Fact]
    public void FindPossibleDuplicate_SeveralMatches_ReturnsClosest()
    {
        var candidate = Report(10);
        var farther = Report(1, lonOffset: 0.005);
        var closer = Report(2, lonOffset: 0.001);

        Assert.Equal(2, detector.FindPossibleDuplicate(candidate, [farther, closer]));
    }

    [Fact]
    public void FindPossibleDuplicate_IgnoresItself()
    {
        var candidate = Report(10);

        Assert.Null(detector.FindPossibleDuplicate(candidate, [candidate]));
    }
}
