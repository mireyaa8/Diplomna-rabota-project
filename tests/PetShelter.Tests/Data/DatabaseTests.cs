using Microsoft.EntityFrameworkCore;
using PetShelter.Data;
using PetShelter.Data.Entities;
using PetShelter.Data.Enums;
using PetShelter.Data.Seeding;
using PetShelter.Services.Admin;

namespace PetShelter.Tests.Data;

/// <summary>
/// Тестове с база в паметта (EF Core InMemory): проверяват началните данни,
/// филтъра за меко изтриване и услугата за статистика.
/// </summary>
public class DatabaseTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated(); // записва данните от HasData
        return context;
    }

    private static Animal NewAnimal(string name, AnimalStatus status, bool isDeleted = false) => new()
    {
        Name = name,
        SpeciesId = SeedData.DogSpeciesId,
        ShelterId = 1,
        Description = "Много добро и спокойно куче, което обича хората.",
        Status = status,
        IsDeleted = isDeleted,
    };

    [Fact]
    public async Task SeedData_ContainsSpeciesBreedsTraitsAndDemoShelter()
    {
        await using var context = CreateContext();

        Assert.Equal(SeedData.Species.Length, await context.Species.CountAsync());
        Assert.Equal(SeedData.Breeds.Length, await context.Breeds.CountAsync());
        Assert.Equal(SeedData.PersonalityTraits.Length, await context.PersonalityTraits.CountAsync());
        Assert.Equal(1, await context.Shelters.CountAsync());
    }

    [Fact]
    public async Task Animals_SoftDeleted_AreHiddenByQueryFilter()
    {
        await using var context = CreateContext();
        context.Animals.AddRange(
            NewAnimal("Рекс", AnimalStatus.Available),
            NewAnimal("Шаро", AnimalStatus.Available, isDeleted: true));
        await context.SaveChangesAsync();

        Assert.Equal(1, await context.Animals.CountAsync());
        Assert.Equal(2, await context.Animals.IgnoreQueryFilters().CountAsync());
    }

    [Fact]
    public async Task AdminDashboardService_ReturnsCorrectCounts()
    {
        await using var context = CreateContext();
        context.Animals.AddRange(
            NewAnimal("Рекс", AnimalStatus.Available),
            NewAnimal("Бела", AnimalStatus.Available),
            NewAnimal("Лъки", AnimalStatus.Adopted),
            NewAnimal("Скрит", AnimalStatus.Available, isDeleted: true));
        await context.SaveChangesAsync();

        var service = new AdminDashboardService(context);

        var statistics = await service.GetStatisticsAsync();

        Assert.Equal(1, statistics.Shelters);
        Assert.Equal(2, statistics.AvailableAnimals);
        Assert.Equal(1, statistics.AdoptedAnimals);
        Assert.Equal(0, statistics.PendingAdoptionRequests);
        Assert.Equal(0, statistics.OpenStrayReports);
    }
}
