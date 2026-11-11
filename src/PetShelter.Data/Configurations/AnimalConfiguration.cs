using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.Data.Entities;

namespace PetShelter.Data.Configurations;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);

        // Индекс за най-честия филтър в каталога: вид + статус.
        builder.HasIndex(a => new { a.SpeciesId, a.Status });

        // Restrict навсякъде, където има риск от няколко пътя за каскадно изтриване
        // (SQL Server не ги позволява). Използваме меко изтриване вместо това.
        builder
            .HasOne(a => a.Shelter)
            .WithMany(s => s.Animals)
            .HasForeignKey(a => a.ShelterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(a => a.Species)
            .WithMany(s => s.Animals)
            .HasForeignKey(a => a.SpeciesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(a => a.Breed)
            .WithMany(b => b.Animals)
            .HasForeignKey(a => a.BreedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AnimalImageConfiguration : IEntityTypeConfiguration<AnimalImage>
{
    public void Configure(EntityTypeBuilder<AnimalImage> builder)
    {
        builder
            .HasOne(i => i.Animal)
            .WithMany(a => a.Images)
            .HasForeignKey(i => i.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AnimalTraitConfiguration : IEntityTypeConfiguration<AnimalTrait>
{
    public void Configure(EntityTypeBuilder<AnimalTrait> builder)
    {
        builder.HasKey(at => new { at.AnimalId, at.PersonalityTraitId });

        builder
            .HasOne(at => at.Animal)
            .WithMany(a => a.Traits)
            .HasForeignKey(at => at.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(at => at.PersonalityTrait)
            .WithMany(t => t.Animals)
            .HasForeignKey(at => at.PersonalityTraitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class FavoriteAnimalConfiguration : IEntityTypeConfiguration<FavoriteAnimal>
{
    public void Configure(EntityTypeBuilder<FavoriteAnimal> builder)
    {
        builder.HasKey(f => new { f.UserId, f.AnimalId });

        builder
            .HasOne(f => f.User)
            .WithMany(u => u.FavoriteAnimals)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(f => f.Animal)
            .WithMany(a => a.FavoritedBy)
            .HasForeignKey(f => f.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
