using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShelter.Data;
using PetShelter.Data.Entities;
using PetShelter.Services.Admin;
using PetShelter.Services.Ai;
using PetShelter.Services.Reports;
using PetShelter.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ---------- База данни ----------
// Връзката идва от appsettings.json или от User Secrets (с предимство).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Липсва връзка 'DefaultConnection' към базата данни.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ---------- Потребители и роли (ASP.NET Core Identity) ----------
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Бисквитката за вход: само през HTTPS и недостъпна за JavaScript (защита срещу XSS).
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
    // Отказан достъп → нашата страница 403 вместо стандартната на Identity.
    options.AccessDeniedPath = "/Home/StatusCode/403";
});

// Банер за съгласие с бисквитките: незадължителните бисквитки се записват само след съгласие.
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

// ---------- MVC ----------
// AutoValidateAntiforgeryToken: всяка POST заявка се проверява срещу CSRF атаки.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
builder.Services.AddRazorPages();

// ---------- Услуги (бизнес логика) ----------
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();

// Strategy: тестов анализатор, докато не бъде добавена истинската AI услуга.
builder.Services.AddScoped<IAnimalImageAnalyzer, FakeAnimalImageAnalyzer>();

var duplicateOptions = builder.Configuration.GetSection("DuplicateDetection").Get<DuplicateDetectionOptions>()
    ?? new DuplicateDetectionOptions();
builder.Services.AddSingleton(duplicateOptions);
builder.Services.AddSingleton<IDuplicateReportDetector, DuplicateReportDetector>();

var app = builder.Build();

// ---------- Обработка на грешки ----------
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // Необработено изключение → собствена страница 500.
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 404, 403 и др. → собствени страници.
app.UseStatusCodePagesWithReExecute("/Home/StatusCode/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Маршрут за областите (Areas), например /Admin/Dashboard.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

await app.InitializeDatabaseAsync();

app.Run();

// Нужно за интеграционните тестове (WebApplicationFactory<Program>).
public partial class Program
{
}
