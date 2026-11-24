using Microsoft.EntityFrameworkCore;
using PetShelter.Data;
using PetShelter.Data.Entities;
using PetShelter.Data.Enums;

namespace PetShelter.Services.Animals;

public interface IAnimalService
{
    Task<PagedResult<AnimalCardModel>> SearchAsync(AnimalQueryModel query, CancellationToken cancellationToken = default);

    Task<AnimalFilterOptions> GetFilterOptionsAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Бизнес логика за животните. Контролерите не работят директно с базата,
/// а извикват тази услуга.
/// </summary>
public class AnimalService : IAnimalService
{
    private readonly ApplicationDbContext dbContext;
    private readonly TimeProvider timeProvider;

    public AnimalService(ApplicationDbContext dbContext, TimeProvider timeProvider)
    {
        this.dbContext = dbContext;
        this.timeProvider = timeProvider;
    }

    private DateOnly Today => DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

    public async Task<PagedResult<AnimalCardModel>> SearchAsync(
        AnimalQueryModel query,
        CancellationToken cancellationToken = default)
    {
        // В каталога не се показват осиновените животни.
        IQueryable<Animal> animals = dbContext.Animals
            .AsNoTracking()
            .Where(a => a.Status != AnimalStatus.Adopted);

        animals = ApplyFilters(animals, query);
        animals = ApplySorting(animals, query.Sorting);

        int totalCount = await animals.CountAsync(cancellationToken);
        int page = Math.Max(1, query.Page);

        var rows = await animals
            .Skip((page - 1) * AnimalQueryModel.PageSize)
            .Take(AnimalQueryModel.PageSize)
            .Select(a => new
            {
                a.Id,
                a.Name,
                SpeciesName = a.Species.Name,
                BreedName = a.Breed != null ? a.Breed.Name : null,
                a.Gender,
                a.Size,
                a.BirthDate,
                MainImageUrl = a.Images
                    .OrderByDescending(i => i.IsMain)
                    .ThenBy(i => i.Id)
                    .Select(i => i.Url)
                    .FirstOrDefault(),
                ShelterName = a.Shelter.Name,
                a.Shelter.City,
                a.Status,
            })
            .ToListAsync(cancellationToken);

        // Възрастта се пресмята след заявката, защото зависи от днешната дата.
        var items = rows
            .Select(r => new AnimalCardModel(
                r.Id,
                r.Name,
                r.SpeciesName,
                r.BreedName,
                r.Gender,
                r.Size,
                AgeCalculator.GetAgeInYears(r.BirthDate, Today),
                r.MainImageUrl,
                r.ShelterName,
                r.City,
                r.Status))
            .ToList();

        return new PagedResult<AnimalCardModel>(items, totalCount, page, AnimalQueryModel.PageSize);
    }

    public async Task<AnimalFilterOptions> GetFilterOptionsAsync(CancellationToken cancellationToken = default)
    {
        var species = await dbContext.Species
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new SelectOption(s.Id, s.Name, null))
            .ToListAsync(cancellationToken);

        var breeds = await dbContext.Breeds
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .Select(b => new SelectOption(b.Id, b.Name, b.SpeciesId))
            .ToListAsync(cancellationToken);

        var shelters = await dbContext.Shelters
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new SelectOption(s.Id, s.Name, null))
            .ToListAsync(cancellationToken);

        var traits = await dbContext.PersonalityTraits
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .Select(t => new SelectOption(t.Id, t.Name, null))
            .ToListAsync(cancellationToken);

        return new AnimalFilterOptions(species, breeds, shelters, traits);
    }

    private IQueryable<Animal> ApplyFilters(IQueryable<Animal> animals, AnimalQueryModel query)
    {
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            string term = query.SearchTerm.Trim();
            animals = animals.Where(a => a.Name.Contains(term) || a.Description.Contains(term));
        }

        if (query.SpeciesId.HasValue)
        {
            animals = animals.Where(a => a.SpeciesId == query.SpeciesId);
        }

        if (query.BreedId.HasValue)
        {
            animals = animals.Where(a => a.BreedId == query.BreedId);
        }

        if (query.ShelterId.HasValue)
        {
            animals = animals.Where(a => a.ShelterId == query.ShelterId);
        }

        if (query.Gender.HasValue)
        {
            animals = animals.Where(a => a.Gender == query.Gender);
        }

        if (query.Size.HasValue)
        {
            animals = animals.Where(a => a.Size == query.Size);
        }

        if (query.Age.HasValue)
        {
            // Възрастта се превръща в интервал от дати на раждане, за да може
            // филтърът да се изпълни от базата данни.
            DateOnly oneYearAgo = Today.AddYears(-1);
            DateOnly sevenYearsAgo = Today.AddYears(-7);

            animals = query.Age switch
            {
                AgeGroup.Young => animals.Where(a => a.BirthDate > oneYearAgo),
                AgeGroup.Adult => animals.Where(a => a.BirthDate <= oneYearAgo && a.BirthDate > sevenYearsAgo),
                AgeGroup.Senior => animals.Where(a => a.BirthDate <= sevenYearsAgo),
                _ => animals,
            };
        }

        // Животното трябва да има ВСИЧКИ избрани черти.
        foreach (int traitId in query.TraitIds.Distinct())
        {
            animals = animals.Where(a => a.Traits.Any(t => t.PersonalityTraitId == traitId));
        }

        return animals;
    }

    private static IQueryable<Animal> ApplySorting(IQueryable<Animal> animals, AnimalSorting sorting)
        => sorting switch
        {
            AnimalSorting.Oldest => animals.OrderBy(a => a.CreatedOn).ThenBy(a => a.Id),
            AnimalSorting.Name => animals.OrderBy(a => a.Name).ThenBy(a => a.Id),
            _ => animals.OrderByDescending(a => a.CreatedOn).ThenByDescending(a => a.Id),
        };
}
