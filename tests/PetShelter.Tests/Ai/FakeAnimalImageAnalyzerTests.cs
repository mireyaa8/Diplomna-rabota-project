using PetShelter.Services.Ai;

namespace PetShelter.Tests.Ai;

public class FakeAnimalImageAnalyzerTests
{
    private readonly FakeAnimalImageAnalyzer analyzer = new();

    [Theory]
    [InlineData("image/jpeg")]
    [InlineData("image/PNG")]
    [InlineData("image/webp")]
    public async Task AnalyzeAsync_AllowedImage_ReturnsAnimal(string contentType)
    {
        using var image = new MemoryStream([1, 2, 3]);

        var result = await analyzer.AnalyzeAsync(image, contentType);

        Assert.True(result.IsAnimal);
        Assert.False(string.IsNullOrWhiteSpace(result.Species));
        Assert.InRange(result.Confidence, 0, 1);
    }

    [Fact]
    public async Task AnalyzeAsync_NotAnImage_Throws()
    {
        using var file = new MemoryStream([1, 2, 3]);

        await Assert.ThrowsAsync<ArgumentException>(() => analyzer.AnalyzeAsync(file, "application/pdf"));
    }
}
