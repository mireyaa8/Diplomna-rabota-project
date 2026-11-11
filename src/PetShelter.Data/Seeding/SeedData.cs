using PetShelter.Data.Entities;

namespace PetShelter.Data.Seeding;

/// <summary>
/// Начални данни, които миграцията записва в базата (HasData).
/// Всички стойности са фиксирани — EF Core изисква това, за да не генерира
/// нова миграция при всяко стартиране.
/// </summary>
public static class SeedData
{
    public const int DogSpeciesId = 1;
    public const int CatSpeciesId = 2;

    public static readonly Species[] Species =
    [
        new() { Id = DogSpeciesId, Name = "Куче" },
        new() { Id = CatSpeciesId, Name = "Котка" },
    ];

    public static readonly Breed[] Breeds =
    [
        new() { Id = 1, SpeciesId = DogSpeciesId, Name = "Смесена порода" },
        new() { Id = 2, SpeciesId = DogSpeciesId, Name = "Каракачанско куче" },
        new() { Id = 3, SpeciesId = DogSpeciesId, Name = "Немска овчарка" },
        new() { Id = 4, SpeciesId = DogSpeciesId, Name = "Лабрадор ретривър" },
        new() { Id = 5, SpeciesId = DogSpeciesId, Name = "Златен ретривър" },
        new() { Id = 6, SpeciesId = DogSpeciesId, Name = "Сибирско хъски" },
        new() { Id = 7, SpeciesId = DogSpeciesId, Name = "Бигъл" },
        new() { Id = 8, SpeciesId = DogSpeciesId, Name = "Пудел" },
        new() { Id = 9, SpeciesId = CatSpeciesId, Name = "Смесена порода" },
        new() { Id = 10, SpeciesId = CatSpeciesId, Name = "Британска късокосместа" },
        new() { Id = 11, SpeciesId = CatSpeciesId, Name = "Персийска" },
    ];

    public static readonly PersonalityTrait[] PersonalityTraits =
    [
        new() { Id = 1, Name = "Разбира се с деца" },
        new() { Id = 2, Name = "Разбира се с други кучета" },
        new() { Id = 3, Name = "Разбира се с котки" },
        new() { Id = 4, Name = "Спокоен" },
        new() { Id = 5, Name = "Игрив" },
        new() { Id = 6, Name = "Енергичен" },
        new() { Id = 7, Name = "Обича разходки" },
        new() { Id = 8, Name = "Приучен на каишка" },
        new() { Id = 9, Name = "Подходящ за апартамент" },
        new() { Id = 10, Name = "Нуждае се от опитен стопанин" },
    ];

    /// <summary>Тестов приют за разработка. Не е реален приют.</summary>
    public static readonly Shelter[] Shelters =
    [
        new()
        {
            Id = 1,
            Name = "Демо приют – София",
            Address = "Тестов адрес 1",
            City = "София",
            Latitude = 42.6977,
            Longitude = 23.3219,
            Description = "Тестов приют за разработка и демонстрация на приложението.",
            CreatedOn = new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc),
        },
    ];
}
