using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 希尔伯特曲线密码
/// </summary>
[Introduction("希尔伯特曲线密码", "使用希尔伯特曲线顺序加密文本。")]
public class HilbertCurveCipher : TranspositionCipher2D
{
    private static TranspositionCipher2D? cipher;

    /// <summary>
    /// <see cref="HilbertCurveCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D Cipher => cipher ??= new HilbertCurveCipher();

    /// <summary>
    /// 希尔伯特曲线密码
    /// </summary>
    public HilbertCurveCipher() => FillOrder = false;

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength)
    {
        int n = 1 << (int)Math.Ceiling(Math.Log2(Math.Sqrt(textLength)));
        return (n, n);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes)
    {
        HilbertCurve(indexes, indexes.GetLength(0));
        return indexes;
    }

    private static void HilbertCurve(ushort[,] indexes, int n)
    {
        if (n == 2)
        {
            InternalInit(indexes);
            return;
        }
        int h = n >> 1;
        HilbertCurve(indexes, h);
        RestPart(indexes, h);
        FlipLeftTop(indexes, h);
        FlipLeftBottom(indexes, h);
    }

    private static void FlipLeftTop(ushort[,] indexes, int n)
    {
        for (int x = 0; x < n - 1; x++)
        {
            for (int y = x + 1; y < n; y++)
            {
                (indexes[y, x], indexes[x, y]) = (indexes[x, y], indexes[y, x]);
            }
        }
    }

    private static void FlipLeftBottom(ushort[,] indexes, int n)
    {
        for (int y = 0; y < n - 1; y++)
        {
            for (int x = 0; x + y < n - 1; x++)
            {
                (indexes[n - y - 1, n + n - x - 1], indexes[x, y + n]) = (indexes[x, y + n], indexes[n - y - 1, n + n - x - 1]);
            }
        }
    }

    private static void RestPart(ushort[,] indexes, int n)
    {
        int m = n * n;
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                indexes[x + n, y] = (ushort)(indexes[x, y] + m);
                indexes[x + n, y + n] = (ushort)(indexes[x, y] + (m << 1));
                indexes[x, y + n] = (ushort)(indexes[x, y] + (m << 1) + m);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void InternalInit(ushort[,] indexes)
    {
        indexes[0, 0] = 0;
        indexes[1, 0] = 1;
        indexes[1, 1] = 2;
        indexes[0, 1] = 3;
    }
}
