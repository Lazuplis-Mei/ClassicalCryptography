namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 旋转阵列密码
/// </summary>
[Introduction("旋转阵列密码", "旋转将会打乱字符")]
public partial class TwiddlePuzzle : TranspositionCipher2D<ushort[]>
{
    private static TranspositionCipher2D<ushort[]>? cipher;

    /// <summary>
    /// <see cref="TwiddlePuzzle"/>的实例
    /// </summary>

    public static TranspositionCipher2D<ushort[]> Cipher => cipher ??= new TwiddlePuzzle();

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<ushort[]> key)
    {
        int n = textLength.SqrtCeil();
        return (n, n);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<ushort[]> key)
    {
        int n = indexes.GetLength(0) - 1;
        foreach (var center in key.KeyValue)
        {
            int x = (center - 1) % n;
            int y = (center - 1) / n;
            var value = indexes[x, y];
            indexes[x, y] = indexes[x, y + 1];
            indexes[x, y + 1] = indexes[x + 1, y + 1];
            indexes[x + 1, y + 1] = indexes[x + 1, y];
            indexes[x + 1, y] = value;
        }
        return indexes;
    }
}
