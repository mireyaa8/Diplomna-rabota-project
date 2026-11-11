using PetShelter.Services.Helpers;

namespace PetShelter.Tests.Helpers;

public class ImageHashHelperTests
{
    /// <summary>Създава снимка 8×9, в която всеки ред има стойностите от функцията.</summary>
    private static byte[,] CreateImage(Func<int, int, byte> pixel)
    {
        var image = new byte[ImageHashHelper.HashHeight, ImageHashHelper.HashWidth];
        for (int row = 0; row < ImageHashHelper.HashHeight; row++)
        {
            for (int col = 0; col < ImageHashHelper.HashWidth; col++)
            {
                image[row, col] = pixel(row, col);
            }
        }

        return image;
    }

    [Fact]
    public void ComputeDifferenceHash_BrighteningToTheRight_AllBitsZero()
    {
        // Всеки пиксел е по-тъмен от съседа си вдясно → нито едно сравнение не дава 1.
        var image = CreateImage((_, col) => (byte)(col * 20));

        Assert.Equal(0L, ImageHashHelper.ComputeDifferenceHash(image));
    }

    [Fact]
    public void ComputeDifferenceHash_DarkeningToTheRight_AllBitsOne()
    {
        var image = CreateImage((_, col) => (byte)(200 - col * 20));

        // Всички 64 бита са 1 → като long това е -1.
        Assert.Equal(-1L, ImageHashHelper.ComputeDifferenceHash(image));
    }

    [Fact]
    public void ComputeDifferenceHash_SameImageBrighter_GivesSameHash()
    {
        // Перцептивният хеш не се влияе от равномерна промяна на яркостта.
        var original = CreateImage((row, col) => (byte)((row * 7 + col * 13) % 100));
        var brighter = CreateImage((row, col) => (byte)((row * 7 + col * 13) % 100 + 50));

        Assert.Equal(
            ImageHashHelper.ComputeDifferenceHash(original),
            ImageHashHelper.ComputeDifferenceHash(brighter));
    }

    [Fact]
    public void ComputeDifferenceHash_WrongSize_Throws()
    {
        Assert.Throws<ArgumentException>(() => ImageHashHelper.ComputeDifferenceHash(new byte[8, 8]));
    }

    [Theory]
    [InlineData(0L, 0L, 0)]
    [InlineData(0L, 1L, 1)]
    [InlineData(0b1011L, 0b0001L, 2)]
    [InlineData(0L, -1L, 64)]
    public void HammingDistance_CountsDifferentBits(long first, long second, int expected)
    {
        Assert.Equal(expected, ImageHashHelper.HammingDistance(first, second));
    }
}
