using PetShelter.Data.Entities;
using PetShelter.Data.Enums;
using PetShelter.Data.Seeding;
using PetShelter.Services.Animals;
using PetShelter.Tests.Infrastructure;

namespace PetShelter.Tests.Animals;

public class AnimalServiceSearchTests
{
    private static AnimalService CreateService(PetShelter.Data.ApplicationDbContext context)
        => new(context, new FixedTimeProvider(TestData.Now));

    [Fact]
    public async Task Search_WithoutFilters_ReturnsAllExceptAdopted()
    {
        await using var context = TestData.CreateContext();
        context.Animals.AddRange(
            TestData.Animal("Рекс"),
            TestData.Animal("Бела", status: AnimalStatus.Reserved),
            TestData.Animal("Лъки", status: AnimalStatus.Adopted));
        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel());

        Assert.Equal(2, result.TotalCount);
        Assert.DoesNotContain(result.Items, a => a.Name == "Лъки");
    }

    [Fact]
    public async Task Search_BySpecies_ReturnsOnlyThatSpecies()
    {
        await using var context = TestData.CreateContext();
        context.Animals.AddRange(
            TestData.Animal("Рекс", speciesId: SeedData.DogSpeciesId),
            TestData.Animal("Мърка", speciesId: SeedData.CatSpeciesId));
        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel { SpeciesId = SeedData.CatSpeciesId });

        var animal = Assert.Single(result.Items);
        Assert.Equal("Мърка", animal.Name);
        Assert.Equal("Котка", animal.SpeciesName);
    }

    [Fact]
    public async Task Search_BySizeAndGender_CombinesFilters()
    {
        await using var context = TestData.CreateContext();
        context.Animals.AddRange(
            TestData.Animal("Голям мъжки", gender: Gender.Male, size: AnimalSize.Large),
            TestData.Animal("Голяма женска", gender: Gender.Female, size: AnimalSize.Large),
            TestData.Animal("Малък мъжки", gender: Gender.Male, size: AnimalSize.Small));
        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel
        {
            Gender = Gender.Male,
            Size = AnimalSize.Large,
        });

        Assert.Equal("Голям мъжки", Assert.Single(result.Items).Name);
    }

    [Theory]
    [InlineData(AgeGroup.Young, "Кутре")]
    [InlineData(AgeGroup.Adult, "Възрастен")]
    [InlineData(AgeGroup.Senior, "Дядо")]
    public async Task Search_ByAgeGroup_UsesBirthDate(AgeGroup ageGroup, string expectedName)
    {
        await using var context = TestData.CreateContext();
        context.Animals.AddRange(
            TestData.Animal("Кутре", birthDate: TestData.Today.AddMonths(-4)),
            TestData.Animal("Възрастен", birthDate: TestData.Today.AddYears(-3)),
            TestData.Animal("Дядо", birthDate: TestData.Today.AddYears(-10)),
            TestData.Animal("Без дата"));
        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel { Age = ageGroup });

        Assert.Equal(expectedName, Assert.Single(result.Items).Name);
    }

    [Fact]
    public async Task Search_ByTraits_RequiresAllSelectedTraits()
    {
        await using var context = TestData.CreateContext();
        context.Animals.AddRange(
            TestData.Animal("С деца и котки", traitIds: [1, 3]),
            TestData.Animal("Само с деца", traitIds: [1]),
            TestData.Animal("Без черти"));
        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel { TraitIds = [1, 3] });

        Assert.Equal("С деца и котки", Assert.Single(result.Items).Name);
    }

    [Fact]
    public async Task Search_BySearchTerm_MatchesName()
    {
        await using var context = TestData.CreateContext();
        context.Animals.AddRange(TestData.Animal("Рекс"), TestData.Animal("Бела"));
        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel { SearchTerm = "  Рекс " });

        Assert.Equal("Рекс", Assert.Single(result.Items).Name);
    }

    [Fact]
    public async Task Search_SortsByNewestByDefault()
    {
        await using var context = TestData.CreateContext();
        context.Animals.AddRange(
            TestData.Animal("Старо", createdOn: new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            TestData.Animal("Ново", createdOn: new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc)));
        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel());

        Assert.Equal(["Ново", "Старо"], result.Items.Select(a => a.Name));
    }

    [Fact]
    public async Task Search_ReturnsRequestedPage()
    {
        await using var context = TestData.CreateContext();
        for (int i = 1; i <= AnimalQueryModel.PageSize + 3; i++)
        {
            context.Animals.Add(TestData.Animal($"Животно {i:D2}"));
        }

        await context.SaveChangesAsync();

        var result = await CreateService(context).SearchAsync(new AnimalQueryModel
        {
            Sorting = AnimalSorting.Name,
            Page = 2,
        });

        Assert.Equal(AnimalQueryModel.PageSize + 3, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(3, result.Items.Count);
        Assert.True(result.HasPreviousPage);
        Assert.False(result.HasNextPage);
    }

    [Fact]
    public async Task Search_ReturnsMainImageAndAge()
    {
        await using var context = TestData.CreateContext();
        var animal = TestData.Animal("Рекс", birthDate: TestData.Today.AddYears(-2));
        animal.Images.Add(new AnimalImage { Url = "/uploads/second.jpg" });
        animal.Images.Add(new AnimalImage { Url = "/uploads/main.jpg", IsMain = true });
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        var card = Assert.Single((await CreateService(context).SearchAsync(new AnimalQueryModel())).Items);

        Assert.Equal("/uploads/main.jpg", card.MainImageUrl);
        Assert.Equal(2, card.AgeInYears);
        Assert.Equal("Демо приют – София", card.ShelterName);
    }

    [Fact]
    public async Task GetFilterOptions_ReturnsSeededLookups()
    {
        await using var context = TestData.CreateContext();

        var options = await CreateService(context).GetFilterOptionsAsync();

        Assert.Equal(SeedData.Species.Length, options.Species.Count);
        Assert.Equal(SeedData.Breeds.Length, options.Breeds.Count);
        Assert.All(options.Breeds, b => Assert.NotNull(b.ParentId));
        Assert.Equal(SeedData.PersonalityTraits.Length, options.Traits.Count);
        Assert.Single(options.Shelters);
    }
}
