namespace ClassicalCryptography.Transposition;

/// <summary>
/// 栅栏密码
/// </summary>
[Introduction("栅栏密码", "按字数分割，并按列读出。")]
public partial class RailFenceCipher : TranspositionCipher<ushort>
{
    private static TranspositionCipher<ushort>? cipher;

    /// <summary>
    /// <see cref="RailFenceCipher"/>的实例
    /// </summary>
    public static TranspositionCipher<ushort> Cipher => cipher ??= new RailFenceCipher();

    /// <inheritdoc/>
    protected override ushort[] Transpose(ushort[] indexes, IKey<ushort> key)
    {
        int m = key.KeyValue;
        int n = indexes.Length;
        int k = 0;
        ushort[] result = new ushort[n];
        for (int i = 0; i < m; i++)
            for (int j = i; j < n; j += m)
                result[k++] = indexes[j];
        return result;
    }
}
