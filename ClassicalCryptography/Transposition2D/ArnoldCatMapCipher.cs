namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 猫映射变换密码<a href="https://en.wikipedia.org/wiki/Arnold%27s_cat_map">Arnold's_cat_map</a>
/// </summary>
[Introduction("猫映射变换密码", "文本按照Arnold猫变换的顺序加密文本。")]
public class ArnoldCatMapCipher : TranspositionCipher2D
{
    private static TranspositionCipher2D? cipher;

    /// <summary>
    /// <see cref="ArnoldCatMapCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D Cipher => cipher ??= new ArnoldCatMapCipher();

    /// <summary>
    /// 猫映射密码
    /// </summary>
    public ArnoldCatMapCipher() => FillOrder = false;

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
                int xi = ((x << 1) + y) % n;
                int yi = (x + y) % n;
                indexes[xi, yi] = (ushort)(x + y * n);
            }
        }
        return indexes;
    }
}
