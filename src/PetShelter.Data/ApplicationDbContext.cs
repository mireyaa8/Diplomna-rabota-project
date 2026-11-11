using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetShelter.Data.Entities;

namespace PetShelter.Data;

/// <summary>
/// Контекстът на базата данни. Наследява IdentityDbContext, за да включи
/// таблиците за потребители и роли (AspNetUsers, AspNetRoles и др.).
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shelter> Shelters => Set<Shelter>();
    public DbSet<Species> Species => Set<Species>();
    public DbSet<Breed> Breeds => Set<Breed>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<AnimalImage> AnimalImages => Set<AnimalImage>();
    public DbSet<PersonalityTrait> PersonalityTraits => Set<PersonalityTrait>();
    public DbSet<AnimalTrait> AnimalTraits => Set<AnimalTrait>();
    public DbSet<AdoptionRequest> AdoptionRequests => Set<AdoptionRequest>();
    public DbSet<StrayReport> StrayReports => Set<StrayReport>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<FavoriteAnimal> FavoriteAnimals => Set<FavoriteAnimal>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Първо Identity конфигурацията, после нашата.
        base.OnModelCreating(builder);

        // Зарежда всички класове IEntityTypeConfiguration<T> от папка Configurations.
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
