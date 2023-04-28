namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 面包师映射变换密码<see href="https://en.wikipedia.org/wiki/Baker%27s_map">Baker's_map</see>
/// </summary>
/// <remarks>
/// 原始版本的变换公式存在一定程度上的不便<br/>
/// 实际的变换过程以代码为准
/// </remarks>
[Introduction("面包师映射变换密码", "文本按照面包师映射变换的顺序加密文本。")]
public class VBakersMapCipher : TranspositionCipher2D
{
    private static TranspositionCipher2D? cipher;

    /// <summary>
    /// <see cref="VBakersMapCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D Cipher => cipher ??= new VBakersMapCipher();

    /// <summary>
    /// Baker's映射变换密码
    /// </summary>
    public VBakersMapCipher()
    {
        FillOrder = false;
    }

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength)
    {
        int n = textLength.SqrtCeil();
        return (n, n);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes)
    {
        int n = indexes.GetLength(0);
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                int xi = x << 1, yi = y >> 1;
                if ((n & 1) == 1 && y == n - 1)
                    xi = x;
                else
                {
                    xi += y & 1;
                    if (xi >= n)
                    {
                        xi -= n;
                        yi += n >> 1;
                        yi += n & 1;
                    }
                }
                indexes[xi, yi] = (ushort)(x + y * n);
            }
        }
        return indexes;
    }
}
