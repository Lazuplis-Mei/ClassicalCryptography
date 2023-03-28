namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 旋转阵列密码
/// </summary>
[Introduction("旋转阵列密码", "旋转将会打乱字符")]
public partial class TwiddlePuzzle : TranspositionCipher2D<ushort[]>
{
    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<ushort[]> key)
    {
        int N = textLength.SqrtCeil();
        return (N, N);
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
