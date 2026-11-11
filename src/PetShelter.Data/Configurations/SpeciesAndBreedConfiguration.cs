using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.Data.Entities;
using PetShelter.Data.Seeding;

namespace PetShelter.Data.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.HasIndex(s => s.Name).IsUnique();

        builder.HasData(SeedData.Species);
    }
}

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        // Една и съща порода не може да се повтаря в рамките на един вид.
        builder.HasIndex(b => new { b.SpeciesId, b.Name }).IsUnique();

        builder
            .HasOne(b => b.Species)
            .WithMany(s => s.Breeds)
            .HasForeignKey(b => b.SpeciesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(SeedData.Breeds);
    }
}

public class PersonalityTraitConfiguration : IEntityTypeConfiguration<PersonalityTrait>
{
    public void Configure(EntityTypeBuilder<PersonalityTrait> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();

        builder.HasData(SeedData.PersonalityTraits);
    }
}
