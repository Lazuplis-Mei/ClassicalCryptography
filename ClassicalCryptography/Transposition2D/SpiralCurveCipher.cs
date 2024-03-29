﻿namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 螺旋曲线密码
/// </summary>
[Introduction("螺旋曲线密码", "文本按照左上=>右上=>右下=>左下=>左上的顺序加密文本。")]
public partial class SpiralCurveCipher : TranspositionCipher2D<int>
{
    private static TranspositionCipher2D<int>? cipher;

    /// <summary>
    /// <see cref="SpiralCurveCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D<int> Cipher => cipher ??= new SpiralCurveCipher();

    /// <summary>
    /// 螺旋曲线密码
    /// </summary>
    public SpiralCurveCipher() => FillOrder = false;

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<int> key)
    {
        int width = key.KeyValue;
        return (width, textLength.DivCeil(width));
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<int> key)
    {
        int sy = 0, sx = 0;
        int width = indexes.GetLength(0);
        int height = indexes.GetLength(1);
        ushort k = 0;
        while (true)
        {
            if (height == 0 || width == 0)
                break;
            for (int x = sx; x < sx + width; x++)
                indexes[x, sy] = k++;
            sy++;
            height--;
            if (height == 0 || width == 0)
                break;
            for (int y = sy; y < sy + height; y++)
                indexes[sx + width - 1, y] = k++;
            width--;
            if (height == 0 || width == 0)
                break;
            for (int x = sx + width - 1; x >= sx; x--)
                indexes[x, sy + height - 1] = k++;
            height--;
            if (height == 0 || width == 0)
                break;
            for (int y = sy + height - 1; y >= sy; y--)
                indexes[sx, y] = k++;
            sx++;
            width--;
        }
        return indexes;
    }
}
