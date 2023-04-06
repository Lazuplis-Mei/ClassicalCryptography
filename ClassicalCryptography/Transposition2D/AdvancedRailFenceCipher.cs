namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 扩展栅栏密码
/// </summary>
[Introduction("扩展栅栏密码", "按字数分割，按列读出，多位密钥代表读取顺序。")]
public partial class AdvancedRailFenceCipher : TranspositionCipher2D<ushort[]>
{
    private static TranspositionCipher2D<ushort[]>? cipher;

    /// <summary>
    /// <see cref="AdvancedRailFenceCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D<ushort[]> Cipher => cipher ??= new AdvancedRailFenceCipher();

    /// <summary>
    /// 扩展栅栏密码
    /// </summary>
    public AdvancedRailFenceCipher()
    {
        FillOrder = false;
        ByColumn = true;
    }

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<ushort[]> key)
    {
        int width = key.KeyValue.Length;
        if (width == 1)
            width = key.KeyValue[0];
        int height = textLength.DivCeil(width);
        return (width, height);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<ushort[]> key)
    {
        ushort[] permut = key.KeyValue;
        int height = indexes.GetLength(1);
        int width = permut.Length;
        if (permut.Length == 1)
            return indexes.FillOrderByRow();
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                indexes[permut[x], y] = (ushort)(x + y * width);
        return indexes;
    }
}
