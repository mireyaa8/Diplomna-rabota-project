namespace PetShelter.Data.Entities;

/// <summary>
/// Любимо животно на потребител. Свързваща таблица със съставен ключ (UserId, AnimalId).
/// </summary>
public class FavoriteAnimal
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    public DateTime AddedOn { get; set; } = DateTime.UtcNow;
}
