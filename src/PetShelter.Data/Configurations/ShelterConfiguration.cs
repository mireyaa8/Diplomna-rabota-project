using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.Data.Entities;
using PetShelter.Data.Seeding;

namespace PetShelter.Data.Configurations;

public class ShelterConfiguration : IEntityTypeConfiguration<Shelter>
{
    public void Configure(EntityTypeBuilder<Shelter> builder)
    {
        // Изтритите приюти не се връщат от заявките.
        builder.HasQueryFilter(s => !s.IsDeleted);

        // Служител → приют: изтриването на приют не трие потребителите.
        builder
            .HasMany(s => s.Staff)
            .WithOne(u => u.Shelter)
            .HasForeignKey(u => u.ShelterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(SeedData.Shelters);
    }
}
