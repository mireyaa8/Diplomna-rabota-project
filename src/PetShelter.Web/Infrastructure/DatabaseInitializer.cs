using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetShelter.Data;
using PetShelter.Data.Common;
using PetShelter.Data.Entities;

namespace PetShelter.Web.Infrastructure;

/// <summary>
/// При стартиране: прилага миграциите, създава ролите и (ако е настроен)
/// първия администратор. Данните за администратора идват от User Secrets.
/// </summary>
public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DatabaseInitializer));
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        if (!dbContext.Database.GetMigrations().Any())
        {
            logger.LogWarning(
                "Няма създадени миграции. Изпълнете: dotnet ef migrations add InitialCreate " +
                "--project src/PetShelter.Data --startup-project src/PetShelter.Web");
            return;
        }

        await dbContext.Database.MigrateAsync();

        await SeedRolesAsync(services.GetRequiredService<RoleManager<IdentityRole>>());
        await SeedAdministratorAsync(services, logger);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (string role in RoleNames.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedAdministratorAsync(IServiceProvider services, ILogger logger)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        string? email = configuration["AdminUser:Email"];
        string? password = configuration["AdminUser:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Не е зададен администратор. Добавете AdminUser:Email и AdminUser:Password в User Secrets.");
            return;
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await userManager.FindByEmailAsync(email);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "Администратор",
                LastName = "Системен",
            };

            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
            {
                logger.LogError(
                    "Администраторът не беше създаден: {Errors}",
                    string.Join("; ", result.Errors.Select(e => e.Description)));
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(admin, RoleNames.Administrator))
        {
            await userManager.AddToRoleAsync(admin, RoleNames.Administrator);
        }
    }
}
