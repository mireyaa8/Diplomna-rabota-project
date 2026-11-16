using Microsoft.EntityFrameworkCore;
using PetShelter.Data.Entities;
using PetShelter.Data.Enums;

namespace PetShelter.Data.Seeding;

/// <summary>
/// Демо животни за разработка и представяне на проекта.
/// За разлика от SeedData, те НЕ са в миграцията: добавят се само когато
/// настройката SeedDemoData е включена (в appsettings.Development.json),
/// за да не попаднат в истинска база.
/// </summary>
public static class DemoDataSeeder
{
    /// <summary>Добавя демо животните, ако в базата още няма нито едно животно. Връща броя добавени.</summary>
    public static async Task<int> SeedAsync(ApplicationDbContext dbContext, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        // IgnoreQueryFilters: броим и меко изтритите, за да не добавяме демо данни отново.
        if (await dbContext.Animals.IgnoreQueryFilters().AnyAsync(cancellationToken))
        {
            return 0;
        }

        var animals = CreateAnimals(utcNow);
        dbContext.Animals.AddRange(animals);
        await dbContext.SaveChangesAsync(cancellationToken);

        return animals.Count;
    }

    private static List<Animal> CreateAnimals(DateTime utcNow)
    {
        var today = DateOnly.FromDateTime(utcNow);
        int shelterId = SeedData.Shelters[0].Id;

        // Всяко следващо животно е „добавено“ един ден по-рано — така подредбата по дата е видима.
        int order = 0;
        Animal Create(
            string name,
            int speciesId,
            int? breedId,
            Gender gender,
            AnimalSize size,
            DateOnly? birthDate,
            string color,
            string description,
            string? habits,
            bool vaccinated,
            bool neutered,
            AnimalStatus status,
            params int[] traitIds)
        {
            var animal = new Animal
            {
                Name = name,
                SpeciesId = speciesId,
                BreedId = breedId,
                ShelterId = shelterId,
                Gender = gender,
                Size = size,
                BirthDate = birthDate,
                IsBirthDateApproximate = true,
                Color = color,
                Description = description,
                Habits = habits,
                IsVaccinated = vaccinated,
                IsNeutered = neutered,
                Status = status,
                CreatedOn = utcNow.AddDays(-order++),
            };

            foreach (int traitId in traitIds)
            {
                animal.Traits.Add(new AnimalTrait { PersonalityTraitId = traitId });
            }

            return animal;
        }

        return
        [
            Create("Рекс", SeedData.DogSpeciesId, 2, Gender.Male, AnimalSize.Large, today.AddYears(-4),
                "бял с кафяви петна",
                "Спокоен и предан пазач, намерен край Витоша. Свиква бързо с новите хора.",
                "Обича сутрешните разходки и да лежи на двора.",
                vaccinated: true, neutered: true, AnimalStatus.Available, 1, 4, 7, 8),

            Create("Бела", SeedData.DogSpeciesId, 1, Gender.Female, AnimalSize.Medium, today.AddYears(-2),
                "черна",
                "Весела и игрива кучка, която обожава топки и деца.",
                "Яде два пъти на ден, приучена е на тоалетна навън.",
                vaccinated: true, neutered: true, AnimalStatus.Available, 1, 2, 5, 11),

            Create("Лъки", SeedData.DogSpeciesId, 4, Gender.Male, AnimalSize.Large, today.AddMonths(-8),
                "златист",
                "Енергично подрастващо куче, което тепърва учи командите.",
                null,
                vaccinated: true, neutered: false, AnimalStatus.Available, 5, 6, 7),

            Create("Шаро", SeedData.DogSpeciesId, 1, Gender.Male, AnimalSize.Small, today.AddYears(-9),
                "сив",
                "Възрастно и много гальовно куче, търси спокоен дом за старините си.",
                "Предпочита кратки разходки и топло легло.",
                vaccinated: true, neutered: true, AnimalStatus.Reserved, 4, 9, 12, 14),

            Create("Мърка", SeedData.CatSpeciesId, 9, Gender.Female, AnimalSize.Small, today.AddYears(-3),
                "тигрова",
                "Нежна котка, която обича да спи на слънце и да мърка в скута.",
                "Приучена на котешка тоалетна.",
                vaccinated: true, neutered: true, AnimalStatus.Available, 4, 9, 11, 12),

            Create("Снежко", SeedData.CatSpeciesId, 19, Gender.Male, AnimalSize.Medium, today.AddMonths(-5),
                "бял",
                "Любопитно коте, което изследва всеки ъгъл.",
                null,
                vaccinated: false, neutered: false, AnimalStatus.Available, 3, 5),

            Create("Пухчо", SeedData.RabbitSpeciesId, 23, Gender.Unknown, AnimalSize.Small, null,
                "сив",
                "Малко и плахо зайче, нужно му е време да свикне с ръцете.",
                "Яде сено, моркови и листа от глухарче.",
                vaccinated: false, neutered: false, AnimalStatus.Available, 9, 13),

            Create("Кики", SeedData.BirdSpeciesId, 25, Gender.Female, AnimalSize.Small, today.AddYears(-1),
                "зелено-жълта",
                "Вълнисто папагалче, което обича да чурулика сутрин.",
                null,
                vaccinated: false, neutered: false, AnimalStatus.Available, 5, 14),
        ];
    }
}
