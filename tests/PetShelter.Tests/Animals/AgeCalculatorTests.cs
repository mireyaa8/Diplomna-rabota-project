using PetShelter.Services.Animals;

namespace PetShelter.Tests.Animals;

public class AgeCalculatorTests
{
    private static readonly DateOnly Today = new(2026, 12, 1);

    [Fact]
    public void GetAgeInYears_BirthdayAlreadyPassed_ReturnsFullYears()
        => Assert.Equal(3, AgeCalculator.GetAgeInYears(new DateOnly(2023, 6, 15), Today));

    [Fact]
    public void GetAgeInYears_BirthdayNotYetThisYear_SubtractsOne()
        => Assert.Equal(2, AgeCalculator.GetAgeInYears(new DateOnly(2023, 12, 20), Today));

    [Fact]
    public void GetAgeInYears_YoungerThanOneYear_ReturnsZero()
        => Assert.Equal(0, AgeCalculator.GetAgeInYears(new DateOnly(2026, 8, 1), Today));

    [Fact]
    public void GetAgeInYears_NoBirthDate_ReturnsNull()
        => Assert.Null(AgeCalculator.GetAgeInYears(null, Today));

    [Fact]
    public void GetAgeInYears_BirthDateInFuture_ReturnsNull()
        => Assert.Null(AgeCalculator.GetAgeInYears(new DateOnly(2027, 1, 1), Today));
}
