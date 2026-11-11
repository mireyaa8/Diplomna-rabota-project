namespace PetShelter.Data.Common;

/// <summary>
/// Ограничения за дължината на текстовите полета.
/// Използват се едновременно в Entities (за базата) и във View Models (за валидацията),
/// за да няма разминаване между двете.
/// </summary>
public static class DataConstants
{
    public static class User
    {
        public const int NameMaxLength = 50;
    }

    public static class Shelter
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;
        public const int AddressMaxLength = 200;
        public const int CityMaxLength = 60;
        public const int PhoneMaxLength = 20;
        public const int EmailMaxLength = 100;
        public const int DescriptionMaxLength = 2000;
    }

    public static class Species
    {
        public const int NameMaxLength = 40;
    }

    public static class Breed
    {
        public const int NameMaxLength = 60;
    }

    public static class Animal
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;
        public const int ColorMaxLength = 50;
        public const int DescriptionMinLength = 20;
        public const int DescriptionMaxLength = 4000;
        public const int HabitsMaxLength = 2000;
        public const int HealthInfoMaxLength = 1000;
    }

    public static class AnimalImage
    {
        public const int UrlMaxLength = 500;
    }

    public static class PersonalityTrait
    {
        public const int NameMaxLength = 50;
        public const int DescriptionMaxLength = 200;
    }

    public static class AdoptionRequest
    {
        public const int MessageMinLength = 20;
        public const int MessageMaxLength = 1000;
    }

    public static class StrayReport
    {
        public const int DescriptionMaxLength = 1000;
        public const int AddressMaxLength = 200;
        public const int ImageUrlMaxLength = 500;
        public const int AiFieldMaxLength = 100;
        public const int AiDescriptionMaxLength = 1000;
    }

    public static class Notification
    {
        public const int MessageMaxLength = 500;
        public const int LinkMaxLength = 300;
    }
}
