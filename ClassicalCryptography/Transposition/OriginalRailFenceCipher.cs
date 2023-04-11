namespace ClassicalCryptography.Transposition;

/// <summary>
/// 原始栅栏密码
/// </summary>
/// <remarks>
/// 参考资料:<see href="https://en.wikipedia.org/wiki/Rail_fence_cipher">wikipedia/Rail_fence_cipher</see>
/// </remarks>
[Introduction("原始栅栏密码", "明文沿对角线向下写在栅栏的连续“轨道”上，到达底部后向上移动，重复此过程。")]
public class OriginalRailFenceCipher : TranspositionCipher<ushort>
{
    private static TranspositionCipher<ushort>? cipher;

    /// <summary>
    /// <see cref="OriginalRailFenceCipher"/>的实例
    /// </summary>
    public static TranspositionCipher<ushort> Cipher => cipher ??= new OriginalRailFenceCipher();

    /// <summary>
    /// 原始栅栏密码
    /// </summary>
    public OriginalRailFenceCipher() => FillOrder = false;

    /// <inheritdoc/>
    protected override ushort[] Transpose(ushort[] indexes, IKey<ushort> key)
    {
        int m = key.KeyValue;
        int n = indexes.Length;
        if (m == 1 || m >= n)
            return indexes.FillOrder();
        
        int k = (--m) << 1, index = 0;
        for (int i = 0; i < n; i += k)
            indexes[index++] = (ushort)i;
        for (int i = 1; i < m; i++)
        {
            bool b = false;
            for (int s, j = i; j < n; j += s)
            {
                indexes[index++] = (ushort)j;
                b = !b;
                s = b ? k - (i << 1) : (i << 1);
            }
        }
        for (int i = m; i < n; i += k)
            indexes[index++] = (ushort)i;
        return indexes;
    }
}
