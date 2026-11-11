using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Data.Common;
using PetShelter.Services.Admin;

namespace PetShelter.Web.Areas.Admin.Controllers;

/// <summary>
/// Начална страница на администраторската зона. Достъпна само за роля Administrator —
/// всички останали получават 403 (или се пренасочват към вход, ако не са влезли).
/// </summary>
[Area("Admin")]
[Authorize(Roles = RoleNames.Administrator)]
public class DashboardController : Controller
{
    private readonly IAdminDashboardService dashboardService;

    public DashboardController(IAdminDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var statistics = await dashboardService.GetStatisticsAsync(cancellationToken);
        return View(statistics);
    }
}
