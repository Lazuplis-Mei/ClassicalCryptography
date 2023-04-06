using ClassicalCryptography.Transposition2D;

namespace ClassicalCryptography.Transposition;

/// <summary>
/// 栅栏密码
/// </summary>
/// <remarks>
/// 这事实上是镰刀密码(ScytaleCipher)，但此版本被广泛使用<br/>
/// 请参考:<see href="https://en.wikipedia.org/wiki/Transposition_cipher">wikipedia/Transposition_cipher</see><br/>
/// 中<i>#Rail_Fence_cipher</i>和<i>#Scytale</i>的对比<br/>
/// 原始版本的栅栏密码:<seealso cref="OriginalRailFenceCipher"/>
/// </remarks>
[Introduction("栅栏密码", "按字数分割，并按列读出。")]
public partial class RailFenceCipher : TranspositionCipher<ushort>
{
    private static TranspositionCipher<ushort>? cipher;

    /// <summary>
    /// <see cref="RailFenceCipher"/>的实例
    /// </summary>
    public static TranspositionCipher<ushort> Cipher => cipher ??= new RailFenceCipher();

    /// <summary>
    /// 栅栏密码
    /// </summary>
    public RailFenceCipher()
    {
        FillOrder = false;
    }

    /// <inheritdoc/>
    protected override ushort[] Transpose(ushort[] indexes, IKey<ushort> key)
    {
        int m = key.KeyValue;
        int n = indexes.Length;
        if (m == 1 || m >= n)
            return indexes.FillOrder();

        int k = 0;
        for (int i = 0; i < m; i++)
            for (int j = i; j < n; j += m)
                indexes[k++] = (ushort)j;
        return indexes;
    }
}
