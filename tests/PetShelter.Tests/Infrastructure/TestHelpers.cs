using Microsoft.EntityFrameworkCore;
using PetShelter.Data;
using PetShelter.Data.Entities;
using PetShelter.Data.Enums;
using PetShelter.Data.Seeding;

namespace PetShelter.Tests.Infrastructure;

/// <summary>Часовник с фиксирана дата, за да не зависят тестовете от днешния ден.</summary>
public class FixedTimeProvider : TimeProvider
{
    private readonly DateTimeOffset now;

    public FixedTimeProvider(DateTimeOffset now)
    {
        this.now = now;
    }

    public override DateTimeOffset GetUtcNow() => now;
}

/// <summary>Общи помощни методи за тестовете с база в паметта.</summary>
public static class TestData
{
    public static readonly DateTimeOffset Now = new(2026, 12, 1, 12, 0, 0, TimeSpan.Zero);

    public static readonly DateOnly Today = DateOnly.FromDateTime(Now.UtcDateTime);

    public static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated(); // записва данните от HasData
        return context;
    }

    public static Animal Animal(
        string name,
        int speciesId = SeedData.DogSpeciesId,
        int? breedId = null,
        AnimalStatus status = AnimalStatus.Available,
        Gender gender = Gender.Male,
        AnimalSize size = AnimalSize.Medium,
        DateOnly? birthDate = null,
        int shelterId = 1,
        DateTime? createdOn = null,
        params int[] traitIds)
    {
        var animal = new Animal
        {
            Name = name,
            SpeciesId = speciesId,
            BreedId = breedId,
            ShelterId = shelterId,
            Status = status,
            Gender = gender,
            Size = size,
            BirthDate = birthDate,
            Description = "Много добро и спокойно животно, което обича хората.",
            CreatedOn = createdOn ?? Now.UtcDateTime,
        };

        foreach (int traitId in traitIds)
        {
            animal.Traits.Add(new AnimalTrait { PersonalityTraitId = traitId });
        }

        return animal;
    }
}
