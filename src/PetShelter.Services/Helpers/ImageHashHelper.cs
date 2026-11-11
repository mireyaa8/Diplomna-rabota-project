using System.Numerics;

namespace PetShelter.Services.Helpers;

/// <summary>
/// Перцептивен хеш на снимка (difference hash, „dHash“).
///
/// Идеята: снимката се смалява до 9×8 пиксела в нюанси на сивото.
/// За всеки ред сравняваме всеки пиксел със съседа вдясно: 1, ако е по-светъл, иначе 0.
/// Получаваме 8 реда × 8 сравнения = 64 бита. Сходни снимки (дори с различен размер
/// или леко различна яркост) дават хешове, които се различават в малко битове.
///
/// Смаляването на снимката става в слоя за качване на файлове (Етап 3);
/// тук е само чистият алгоритъм, за да може да се тества.
/// </summary>
public static class ImageHashHelper
{
    public const int HashWidth = 9;
    public const int HashHeight = 8;

    /// <summary>
    /// Изчислява 64-битов dHash от смалена снимка 9×8.
    /// </summary>
    /// <param name="grayscalePixels">Яркост на пикселите [ред, колона], стойности 0–255.</param>
    public static long ComputeDifferenceHash(byte[,] grayscalePixels)
    {
        ArgumentNullException.ThrowIfNull(grayscalePixels);

        if (grayscalePixels.GetLength(0) != HashHeight || grayscalePixels.GetLength(1) != HashWidth)
        {
            throw new ArgumentException($"Очаква се масив {HashHeight}×{HashWidth} (редове × колони).", nameof(grayscalePixels));
        }

        ulong hash = 0;
        int bit = 0;

        for (int row = 0; row < HashHeight; row++)
        {
            for (int col = 0; col < HashWidth - 1; col++)
            {
                if (grayscalePixels[row, col] > grayscalePixels[row, col + 1])
                {
                    hash |= 1UL << bit;
                }

                bit++;
            }
        }

        // SQL Server няма тип без знак, затова пазим битовете в long.
        return unchecked((long)hash);
    }

    /// <summary>
    /// Разстояние на Хеминг: в колко бита се различават два хеша (0 = еднакви, 64 = напълно различни).
    /// </summary>
    public static int HammingDistance(long firstHash, long secondHash)
        => BitOperations.PopCount(unchecked((ulong)(firstHash ^ secondHash)));
}
