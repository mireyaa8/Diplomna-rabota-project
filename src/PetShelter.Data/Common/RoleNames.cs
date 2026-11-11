namespace PetShelter.Data.Common;

/// <summary>
/// Имената на ролите в системата. „Гост“ не е роля — това е невлязъл потребител.
/// </summary>
public static class RoleNames
{
    public const string Administrator = "Administrator";
    public const string ShelterStaff = "ShelterStaff";
    public const string User = "User";

    public static readonly IReadOnlyList<string> All = [Administrator, ShelterStaff, User];
}
