namespace PetShelter.Services.Helpers;

/// <summary>
/// Географски изчисления. Използва се за избор на най-близкия приют
/// и за проверка дали два сигнала са от едно и също място.
/// </summary>
public static class GeoHelper
{
    /// <summary>Среден радиус на Земята в метри.</summary>
    public const double EarthRadiusMeters = 6_371_000;

    /// <summary>
    /// Разстояние между две точки по повърхността на Земята (формула на Хаверсинус).
    /// </summary>
    /// <returns>Разстояние в метри.</returns>
    public static double DistanceInMeters(double lat1, double lon1, double lat2, double lon2)
    {
        ValidateCoordinates(lat1, lon1);
        ValidateCoordinates(lat2, lon2);

        double phi1 = ToRadians(lat1);
        double phi2 = ToRadians(lat2);
        double deltaPhi = ToRadians(lat2 - lat1);
        double deltaLambda = ToRadians(lon2 - lon1);

        double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2)
                 + Math.Cos(phi1) * Math.Cos(phi2)
                 * Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);

        double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

        return EarthRadiusMeters * c;
    }

    public static bool IsValidLatitude(double latitude) => latitude is >= -90 and <= 90;

    public static bool IsValidLongitude(double longitude) => longitude is >= -180 and <= 180;

    private static void ValidateCoordinates(double latitude, double longitude)
    {
        if (!IsValidLatitude(latitude))
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Географската ширина трябва да е между -90 и 90.");
        }

        if (!IsValidLongitude(longitude))
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Географската дължина трябва да е между -180 и 180.");
        }
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
