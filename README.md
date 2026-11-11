# Платформа за приют за животни

Дипломен проект — уеб приложение, което свързва гражданите с приютите за животни.

**Автор:** [трите имена], [клас] — ЧПГДН „СофтУни Будител“
**Дипломен консултант:** [име на консултанта]
**Професия / специалност:** 481030 Приложен програмист / 4810301 Приложно програмиране
**Учебна година:** 2026/2027

## Предназначение и цел

Платформата обединява няколко приюта в едно приложение. Гражданите разглеждат каталог с животни за
осиновяване, подават онлайн заявки и сигнализират за бездомни животни със снимка и местоположение на карта.
AI услуга описва вида, породата и цвета на животното по снимката и открива повторни сигнали, а най-близкият
приют получава известие.

## Основни функционалности

| Функционалност | Състояние |
| --- | --- |
| Структура на решението, база данни, роли | ✅ Етап 1 |
| Собствени страници за грешки 404 / 403 / 500 | ✅ Етап 1 |
| Admin зона със статистика | ✅ Етап 1 (основа) |
| Банер за съгласие с бисквитки | ✅ Етап 1 |
| Алгоритъм за откриване на повторни сигнали | ✅ Етап 1 (логика + тестове) |
| Каталог с филтри и профили на животни | ⏳ Етап 2 |
| Заявки за осиновяване | ⏳ Етап 3 |
| Сигнали с карта (Leaflet + OpenStreetMap) | ⏳ Етап 3 |
| AI описание на снимки | ⏳ Етап 3 |
| Известия и любими животни | ⏳ Етап 3 |

## Използвани технологии

- **.NET 10**, **ASP.NET Core MVC**, Razor изгледи
- **MS SQL Server**, **Entity Framework Core 10** (Code First)
- **ASP.NET Core Identity** — потребители и роли
- **Bootstrap 5**, CSS анимации, JavaScript
- **xUnit**, **Moq**, **EF Core InMemory**, **Coverlet** — тестове и покритие
- **GitHub Actions** — непрекъсната интеграция (CI)

## Структура на решението

```
PetShelter.slnx
├── src/
│   ├── PetShelter.Data/       Entities, ApplicationDbContext, конфигурации, начални данни
│   ├── PetShelter.Services/   Бизнес логика, помощни класове, AI анализатор
│   └── PetShelter.Web/        Контролери, изгледи, Admin зона, Program.cs
├── tests/
│   └── PetShelter.Tests/      Unit тестове
└── .github/workflows/ci.yml   CI: build + тестове при всеки push
```

## Структура на базата данни

| Таблица | Предназначение |
| --- | --- |
| AspNetUsers (ApplicationUser) | Потребители; служителите са свързани с приют |
| Shelters | Приюти с координати |
| Species, Breeds | Видове и породи |
| Animals | Животни за осиновяване |
| AnimalImages | Снимки на животните |
| PersonalityTraits, AnimalTraits | Черти на характера (много към много) |
| AdoptionRequests | Заявки за осиновяване |
| StrayReports | Сигнали за бездомни животни с AI резултат и хеш на снимката |
| Notifications | Известия |
| FavoriteAnimals | Любими животни на потребителя |

## Роли

| Роля | Права |
| --- | --- |
| Гост (без вход) | Разглежда каталога и картата |
| User | Подава сигнали и заявки, запазва любими |
| ShelterStaff | Управлява животните и заявките на своя приют, обработва сигнали |
| Administrator | Пълен достъп до Admin зоната |

## Инструкции за стартиране

### Необходим софтуер

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (LocalDB идва с Visual Studio) или SQL Server Express
- Visual Studio 2026 / Rider / VS Code
- Инструментът за миграции: `dotnet tool install --global dotnet-ef`

### Стъпки

1. Клонирайте хранилището и отворете `PetShelter.slnx`.
2. Настройте тайните (User Secrets) — изпълнете от папката на проекта:

   ```bash
   cd src/PetShelter.Web
   dotnet user-secrets set "AdminUser:Email" "admin@petshelter.local"
   dotnet user-secrets set "AdminUser:Password" "<ваша парола, поне 8 знака, с главна буква и цифра>"
   ```

   Ако не използвате LocalDB, задайте и връзката към базата:

   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<вашата връзка>"
   ```

3. Създайте първата миграция и базата (от главната папка на решението):

   ```bash
   dotnet ef migrations add InitialCreate --project src/PetShelter.Data --startup-project src/PetShelter.Web
   dotnet ef database update --project src/PetShelter.Data --startup-project src/PetShelter.Web
   ```

4. Стартирайте приложението:

   ```bash
   dotnet run --project src/PetShelter.Web
   ```

   При стартиране се създават ролите и администраторът от стъпка 2.

5. Тестове:

   ```bash
   dotnet test
   ```

## Тестови потребители

| Роля | Имейл | Парола |
| --- | --- | --- |
| Administrator | задава се в User Secrets | задава се в User Secrets |

Реални пароли не се публикуват в хранилището.
