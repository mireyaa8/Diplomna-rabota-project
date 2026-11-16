using Microsoft.EntityFrameworkCore;
using PetShelter.Data;
using PetShelter.Data.Entities;
using PetShelter.Data.Enums;
using PetShelter.Data.Seeding;

namespace PetShelter.Tests.Data;

public class DemoDataSeederTests
{
    private static readonly DateTime Now = new(2026, 11, 13, 12, 0, 0, DateTimeKind.Utc);

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task Seed_EmptyDatabase_AddsDemoAnimalsWithTraits()
    {
        await using var context = CreateContext();

        int added = await DemoDataSeeder.SeedAsync(context, Now);

        Assert.True(added >= 6);
        Assert.Equal(added, await context.Animals.CountAsync());
        Assert.True(await context.AnimalTraits.AnyAsync());
        Assert.True(await context.Animals.AnyAsync(a => a.SpeciesId == SeedData.CatSpeciesId));
        Assert.True(await context.Animals.AnyAsync(a => a.Status == AnimalStatus.Reserved));
    }

    [Fact]
    public async Task Seed_UsesOnlyExistingBreedsAndTraits()
    {
        await using var context = CreateContext();
        await DemoDataSeeder.SeedAsync(context, Now);

        var animals = await context.Animals.Include(a => a.Breed).Include(a => a.Traits).ToListAsync();

        // Породата трябва да е от същия вид като животното.
        Assert.All(animals.Where(a => a.Breed is not null), a => Assert.Equal(a.SpeciesId, a.Breed!.SpeciesId));
        var traitIds = SeedData.PersonalityTraits.Select(t => t.Id).ToHashSet();
        Assert.All(animals.SelectMany(a => a.Traits), t => Assert.Contains(t.PersonalityTraitId, traitIds));
    }

    [Fact]
    public async Task Seed_RunTwice_DoesNotDuplicate()
    {
        await using var context = CreateContext();

        int first = await DemoDataSeeder.SeedAsync(context, Now);
        int second = await DemoDataSeeder.SeedAsync(context, Now);

        Assert.Equal(0, second);
        Assert.Equal(first, await context.Animals.CountAsync());
    }

    [Fact]
    public async Task Seed_DatabaseWithAnimals_AddsNothing()
    {
        await using var context = CreateContext();
        context.Animals.Add(new Animal
        {
            Name = "Истинско",
            SpeciesId = SeedData.DogSpeciesId,
            ShelterId = 1,
            Description = "Животно, добавено от служител на приюта.",
        });
        await context.SaveChangesAsync();

        Assert.Equal(0, await DemoDataSeeder.SeedAsync(context, Now));
        Assert.Equal(1, await context.Animals.CountAsync());
    }
}
