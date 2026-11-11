using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.Data.Entities;

namespace PetShelter.Data.Configurations;

public class AdoptionRequestConfiguration : IEntityTypeConfiguration<AdoptionRequest>
{
    public void Configure(EntityTypeBuilder<AdoptionRequest> builder)
    {
        builder.HasIndex(r => new { r.AnimalId, r.Status });

        builder
            .HasOne(r => r.Animal)
            .WithMany(a => a.AdoptionRequests)
            .HasForeignKey(r => r.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.AdoptionRequests)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StrayReportConfiguration : IEntityTypeConfiguration<StrayReport>
{
    public void Configure(EntityTypeBuilder<StrayReport> builder)
    {
        // Проверката за дубликати търси скорошни сигнали — индекс по дата и статус.
        builder.HasIndex(r => new { r.Status, r.CreatedOn });

        builder
            .HasOne(r => r.Reporter)
            .WithMany(u => u.StrayReports)
            .HasForeignKey(r => r.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.AssignedShelter)
            .WithMany(s => s.AssignedReports)
            .HasForeignKey(r => r.AssignedShelterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Връзка към самата таблица: сигнал → възможен оригинал.
        builder
            .HasOne(r => r.PossibleDuplicateOf)
            .WithMany()
            .HasForeignKey(r => r.PossibleDuplicateOfId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Най-честата заявка: непрочетените известия на един потребител.
        builder.HasIndex(n => new { n.UserId, n.IsRead });

        builder
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
