namespace PetShelter.Services.Animals;

/// <summary>Пресмята възрастта на животно по датата му на раждане.</summary>
public static class AgeCalculator
{
    public static int? GetAgeInYears(DateOnly? birthDate, DateOnly today)
    {
        if (birthDate is null || birthDate > today)
        {
            return null;
        }

        int age = today.Year - birthDate.Value.Year;

        // Ако рожденият ден тази година още не е минал.
        if (birthDate.Value.AddYears(age) > today)
        {
            age--;
        }

        return age;
    }
}
