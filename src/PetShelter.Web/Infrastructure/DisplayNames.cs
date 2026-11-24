using PetShelter.Data.Enums;
using PetShelter.Services.Animals;

namespace PetShelter.Web.Infrastructure;

/// <summary>
/// Български надписи за изброимите типове. В базата се пазят числа,
/// а потребителят вижда тези текстове.
/// </summary>
public static class DisplayNames
{
    public static string ToDisplay(this Gender gender) => gender switch
    {
        Gender.Male => "Мъжки",
        Gender.Female => "Женски",
        _ => "Неизвестен",
    };

    public static string ToDisplay(this AnimalSize size) => size switch
    {
        AnimalSize.Small => "Малък",
        AnimalSize.Medium => "Среден",
        AnimalSize.Large => "Голям",
        AnimalSize.ExtraLarge => "Много голям",
        _ => size.ToString(),
    };

    public static string ToDisplay(this AnimalStatus status) => status switch
    {
        AnimalStatus.Available => "Търси дом",
        AnimalStatus.Reserved => "Резервиран",
        AnimalStatus.Adopted => "Осиновен",
        _ => status.ToString(),
    };

    public static string ToDisplay(this AgeGroup age) => age switch
    {
        AgeGroup.Young => "До 1 година",
        AgeGroup.Adult => "От 1 до 7 години",
        AgeGroup.Senior => "Над 7 години",
        _ => age.ToString(),
    };

    public static string ToDisplay(this AnimalSorting sorting) => sorting switch
    {
        AnimalSorting.Oldest => "Най-стари обяви",
        AnimalSorting.Name => "По име",
        _ => "Най-нови обяви",
    };

    /// <summary>„1 година“, „3 години“, „под 1 година“ или „неизвестна“.</summary>
    public static string AgeText(int? years) => years switch
    {
        null => "Възраст: неизвестна",
        0 => "Под 1 година",
        1 => "1 година",
        _ => $"{years} години",
    };
}
