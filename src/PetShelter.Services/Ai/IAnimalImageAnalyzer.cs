namespace PetShelter.Services.Ai;

/// <summary>Резултат от AI анализа на снимка.</summary>
public record AnimalImageAnalysis(
    bool IsAnimal,
    string? Species,
    string? Breed,
    string? Color,
    string? Description,
    double Confidence);

/// <summary>
/// Анализира снимка на животно. Шаблон Strategy: приложението работи с интерфейса,
/// а реализацията (истинска AI услуга или тестова) се избира при стартиране в Program.cs.
/// </summary>
public interface IAnimalImageAnalyzer
{
    Task<AnimalImageAnalysis> AnalyzeAsync(Stream image, string contentType, CancellationToken cancellationToken = default);
}
