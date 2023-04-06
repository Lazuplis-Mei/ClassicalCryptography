namespace ClassicalCryptography.Transposition;

/// <summary>
/// 约瑟夫置换密码
/// </summary>
[Introduction("约瑟夫置换密码", "约瑟夫问题中的出列次序。")]
public partial class JosephusCipher : TranspositionCipher<ushort>
{
    private static TranspositionCipher<ushort>? cipher;

    /// <summary>
    /// <see cref="JosephusCipher"/>的实例
    /// </summary>
    public static TranspositionCipher<ushort> Cipher => cipher ??= new JosephusCipher();

    /// <inheritdoc/>
    protected override ushort[] Transpose(ushort[] indexes, IKey<ushort> key)
    {
        int m = key.KeyValue - 1;
        var linkedList = new LinkedList(indexes);
        linkedList.Last.Next = linkedList.First;
        linkedList.MoveStep(m - 1);
        for (int i = 0; i < indexes.Length; linkedList.MoveStep(m))
        {
            var current = linkedList.Current;
            linkedList.Current = current.Next!;
            indexes[i++] = linkedList.Current.Value;
            current.Next = linkedList.Current.Next!;
        }
        return indexes;
    }
}
