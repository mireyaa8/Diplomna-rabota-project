using PetShelter.Data.Seeding;

namespace PetShelter.Tests.Data;

/// <summary>
/// Проверява началните данни, преди да стигнат до миграция: грешка тук би
/// означавала счупена миграция или дублирани записи в базата.
/// </summary>
public class SeedDataTests
{
    [Fact]
    public void Species_HaveUniqueIdsAndNames()
    {
        Assert.Equal(SeedData.Species.Length, SeedData.Species.Select(s => s.Id).Distinct().Count());
        Assert.Equal(SeedData.Species.Length, SeedData.Species.Select(s => s.Name).Distinct().Count());
    }

    [Fact]
    public void Breeds_BelongToExistingSpecies()
    {
        var speciesIds = SeedData.Species.Select(s => s.Id).ToHashSet();

        Assert.All(SeedData.Breeds, b => Assert.Contains(b.SpeciesId, speciesIds));
    }

    [Fact]
    public void Breeds_HaveUniqueIdsAndUniqueNamesWithinSpecies()
    {
        Assert.Equal(SeedData.Breeds.Length, SeedData.Breeds.Select(b => b.Id).Distinct().Count());
        Assert.Equal(SeedData.Breeds.Length, SeedData.Breeds.Select(b => (b.SpeciesId, b.Name)).Distinct().Count());
    }

    [Fact]
    public void EverySpecies_HasAtLeastOneBreed()
    {
        Assert.All(SeedData.Species, s => Assert.Contains(SeedData.Breeds, b => b.SpeciesId == s.Id));
    }

    [Fact]
    public void PersonalityTraits_HaveUniqueNamesAndDescriptions()
    {
        Assert.Equal(SeedData.PersonalityTraits.Length, SeedData.PersonalityTraits.Select(t => t.Name).Distinct().Count());
        Assert.All(SeedData.PersonalityTraits, t => Assert.False(string.IsNullOrWhiteSpace(t.Description)));
    }
}
