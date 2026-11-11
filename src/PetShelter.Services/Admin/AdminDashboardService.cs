using Microsoft.EntityFrameworkCore;
using PetShelter.Data;
using PetShelter.Data.Enums;

namespace PetShelter.Services.Admin;

/// <summary>Обобщени числа за началната страница на администратора.</summary>
public record DashboardStatistics(
    int Shelters,
    int AvailableAnimals,
    int AdoptedAnimals,
    int PendingAdoptionRequests,
    int OpenStrayReports);

public interface IAdminDashboardService
{
    Task<DashboardStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);
}

public class AdminDashboardService : IAdminDashboardService
{
    private readonly ApplicationDbContext dbContext;

    public AdminDashboardService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<DashboardStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        // Заявките са последователни: един DbContext не позволява паралелни заявки.
        int shelters = await dbContext.Shelters.CountAsync(cancellationToken);

        int available = await dbContext.Animals
            .CountAsync(a => a.Status == AnimalStatus.Available, cancellationToken);

        int adopted = await dbContext.Animals
            .CountAsync(a => a.Status == AnimalStatus.Adopted, cancellationToken);

        int pendingRequests = await dbContext.AdoptionRequests
            .CountAsync(r => r.Status == AdoptionRequestStatus.Pending, cancellationToken);

        int openReports = await dbContext.StrayReports
            .CountAsync(r => r.Status == StrayReportStatus.New || r.Status == StrayReportStatus.InProgress, cancellationToken);

        return new DashboardStatistics(shelters, available, adopted, pendingRequests, openReports);
    }
}
