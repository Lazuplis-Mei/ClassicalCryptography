namespace ClassicalCryptography.Transposition;

/// <summary>
/// 三角排列密码
/// </summary>
[Introduction("三角排列密码", "文本按行排列成三角形，按列读出。")]
public class TriangleCipher : TranspositionCipher
{
    private static TranspositionCipher? cipher;

    /// <summary>
    /// <see cref="TriangleCipher"/>的实例
    /// </summary>
    public static TranspositionCipher Cipher => cipher ??= new TriangleCipher();

    /// <summary>
    /// 三角排列密码
    /// </summary>
    public TriangleCipher() => FillOrder = false;

    /// <inheritdoc/>
    protected override int PadLength(int length)
    {
        length = length.SqrtCeil();
        return length * length;
    }

    /// <inheritdoc/>
    protected override ushort[] Transpose(ushort[] indexes)
    {
        int n = (int)Math.Sqrt(indexes.Length);
        int i = 0;
        for (int j = n - 1; j >= 0; j--)
        {
            indexes[i++] = (ushort)(j * j);
            for (int k = j + 1; k < n; k++, i++)
                indexes[i] = (ushort)(indexes[i - 1] + (k << 1));
        }
        for (int j = 2; j <= n; j++)
        {
            indexes[i++] = (ushort)(j * j - 1);
            for (int k = j; k < n; k++, i++)
                indexes[i] = (ushort)(indexes[i - 1] + (k << 1));
        }
        return indexes;
    }
}
