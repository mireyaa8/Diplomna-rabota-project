using PetShelter.Services.Helpers;

namespace PetShelter.Tests.Helpers;

public class GeoHelperTests
{
    // Център на София и център на Пловдив.
    private const double SofiaLat = 42.6977, SofiaLon = 23.3219;
    private const double PlovdivLat = 42.1354, PlovdivLon = 24.7453;

    [Fact]
    public void DistanceInMeters_SamePoint_ReturnsZero()
    {
        double distance = GeoHelper.DistanceInMeters(SofiaLat, SofiaLon, SofiaLat, SofiaLon);

        Assert.Equal(0, distance, precision: 6);
    }

    [Fact]
    public void DistanceInMeters_SofiaToPlovdiv_IsAbout132Kilometers()
    {
        double distance = GeoHelper.DistanceInMeters(SofiaLat, SofiaLon, PlovdivLat, PlovdivLon);

        // Очаквано: ~132 522 м по права линия.
        Assert.InRange(distance, 132_000, 133_000);
    }

    [Fact]
    public void DistanceInMeters_IsSymmetric()
    {
        double there = GeoHelper.DistanceInMeters(SofiaLat, SofiaLon, PlovdivLat, PlovdivLon);
        double back = GeoHelper.DistanceInMeters(PlovdivLat, PlovdivLon, SofiaLat, SofiaLon);

        Assert.Equal(there, back, precision: 6);
    }

    [Fact]
    public void DistanceInMeters_ShortDistance_IsAccurate()
    {
        // 0.004° географска дължина при ширина 42.7° ≈ 327 м.
        double distance = GeoHelper.DistanceInMeters(SofiaLat, SofiaLon, SofiaLat, SofiaLon + 0.004);

        Assert.InRange(distance, 320, 335);
    }

    [Theory]
    [InlineData(91, 0)]
    [InlineData(-91, 0)]
    [InlineData(0, 181)]
    [InlineData(0, -181)]
    public void DistanceInMeters_InvalidCoordinates_Throws(double lat, double lon)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GeoHelper.DistanceInMeters(lat, lon, 0, 0));
    }

    [Theory]
    [InlineData(-90, true)]
    [InlineData(90, true)]
    [InlineData(42.7, true)]
    [InlineData(90.1, false)]
    public void IsValidLatitude_ReturnsExpected(double latitude, bool expected)
    {
        Assert.Equal(expected, GeoHelper.IsValidLatitude(latitude));
    }
}
