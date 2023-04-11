namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 锯齿分割密码
/// </summary>
[Introduction("锯齿分割密码", "文本方阵边长的整数分拆为界限排列成锯齿,以特点顺序加密文本。")]
public partial class JigsawCipher : TranspositionCipher2D<ushort[]>
{
    private static TranspositionCipher2D<ushort[]>? cipher;

    /// <summary>
    /// <see cref="JigsawCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D<ushort[]> Cipher => cipher ??= new JigsawCipher();

    /// <summary>
    /// 锯齿分割密码
    /// </summary>
    public JigsawCipher() => FillOrder = false;

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<ushort[]> key)
    {
        int n = textLength.SqrtCeil();
        return (n, n);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<ushort[]> key)
    {
        int n = indexes.GetLength(0);
        ushort value = (ushort)(n - 1), column = (ushort)n;
        for (int j = key.KeyValue.Length - 1; j >= 0; j--)
        {
            var p = key.KeyValue[j];
            column -= p;
            int y = 0, k = 0, row = 0;
            for (int i = 0; i < p; i++)
            {
                if (k != 0)
                {
                    for (int x = 0; x < p - k; x++)
                        indexes[column + x, y] = value--;
                    row += ((n - (p - k)) / p) + 1;
                    y++;
                }
                else
                    row += n / p;
                for (; y < row; y++)
                    for (int x = 0; x < p; x++)
                        indexes[column + x, y] = value--;
                k = k != 0 ? (n - (p - k)) % p : n % p;
                for (int x = p - k; x < p; x++)
                    indexes[column + x, y] = value--;
                value += (ushort)(n << 1);
            }
        }
        return indexes;
    }
}
