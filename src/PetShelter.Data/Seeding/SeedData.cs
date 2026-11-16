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
    public const int RabbitSpeciesId = 3;
    public const int BirdSpeciesId = 4;

    public static readonly Species[] Species =
    [
        new() { Id = DogSpeciesId, Name = "Куче" },
        new() { Id = CatSpeciesId, Name = "Котка" },
        new() { Id = RabbitSpeciesId, Name = "Заек" },
        new() { Id = BirdSpeciesId, Name = "Птица" },
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

        // Добавени в миграцията AddMoreLookupData. Новите записи получават следващите номера,
        // старите не се променят — иначе миграцията би изтрила и създала наново съществуващи данни.
        new() { Id = 12, SpeciesId = DogSpeciesId, Name = "Български овчарски пес" },
        new() { Id = 13, SpeciesId = DogSpeciesId, Name = "Йоркширски териер" },
        new() { Id = 14, SpeciesId = DogSpeciesId, Name = "Чихуахуа" },
        new() { Id = 15, SpeciesId = DogSpeciesId, Name = "Бордър коли" },
        new() { Id = 16, SpeciesId = DogSpeciesId, Name = "Американски стафордширски териер" },
        new() { Id = 17, SpeciesId = DogSpeciesId, Name = "Френски булдог" },
        new() { Id = 18, SpeciesId = CatSpeciesId, Name = "Сиамска" },
        new() { Id = 19, SpeciesId = CatSpeciesId, Name = "Мейн кун" },
        new() { Id = 20, SpeciesId = CatSpeciesId, Name = "Шотландска клепоуха" },
        new() { Id = 21, SpeciesId = CatSpeciesId, Name = "Руска синя" },
        new() { Id = 22, SpeciesId = RabbitSpeciesId, Name = "Смесена порода" },
        new() { Id = 23, SpeciesId = RabbitSpeciesId, Name = "Холандско джудже" },
        new() { Id = 24, SpeciesId = RabbitSpeciesId, Name = "Клепоух заек" },
        new() { Id = 25, SpeciesId = BirdSpeciesId, Name = "Вълнисто папагалче" },
        new() { Id = 26, SpeciesId = BirdSpeciesId, Name = "Корела" },
        new() { Id = 27, SpeciesId = BirdSpeciesId, Name = "Канарче" },
    ];

    public static readonly PersonalityTrait[] PersonalityTraits =
    [
        new() { Id = 1, Name = "Разбира се с деца", Description = "Спокоен и търпелив в присъствието на деца." },
        new() { Id = 2, Name = "Разбира се с други кучета", Description = "Може да живее с друго куче в дома." },
        new() { Id = 3, Name = "Разбира се с котки", Description = "Не гони и не напада котки." },
        new() { Id = 4, Name = "Спокоен", Description = "Предпочита тишината и почивката." },
        new() { Id = 5, Name = "Игрив", Description = "Обича игри и играчки." },
        new() { Id = 6, Name = "Енергичен", Description = "Нуждае се от много движение всеки ден." },
        new() { Id = 7, Name = "Обича разходки", Description = "Радва се на дълги разходки навън." },
        new() { Id = 8, Name = "Приучен на каишка", Description = "Върви спокойно на каишка." },
        new() { Id = 9, Name = "Подходящ за апартамент", Description = "Чувства се добре и в по-малко пространство." },
        new() { Id = 10, Name = "Нуждае се от опитен стопанин", Description = "Има нужда от последователно възпитание." },
        new() { Id = 11, Name = "Приучен на тоалетна", Description = "Ходи до тоалетна навън или в котешка тоалетна." },
        new() { Id = 12, Name = "Гальовен", Description = "Търси ласка и близост с хората." },
        new() { Id = 13, Name = "Плах", Description = "Нужно му е време да свикне с нови хора." },
        new() { Id = 14, Name = "Може да остава сам", Description = "Спокойно изчаква, докато стопаните са на работа." },
        new() { Id = 15, Name = "Нуждае се от специални грижи", Description = "Има здравословен проблем, който изисква внимание." },
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
