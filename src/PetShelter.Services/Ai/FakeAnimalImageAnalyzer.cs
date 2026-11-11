namespace PetShelter.Services.Ai;

/// <summary>
/// Тестова реализация: не вика външна услуга и връща примерен резултат.
/// Позволява приложението да работи без AI ключ и тестовете — без интернет.
/// Истинската реализация се добавя в Етап 3.
/// </summary>
public class FakeAnimalImageAnalyzer : IAnimalImageAnalyzer
{
    public static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];

    public Task<AnimalImageAnalysis> AnalyzeAsync(Stream image, string contentType, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(image);

        if (!AllowedContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Поддържат се само снимки във формат JPEG, PNG или WebP.", nameof(contentType));
        }

        var result = new AnimalImageAnalysis(
            IsAnimal: true,
            Species: "Куче",
            Breed: "Смесена порода",
            Color: "кафяв",
            Description: "Примерен резултат от тестовия анализатор.",
            Confidence: 0.5);

        return Task.FromResult(result);
    }
}
