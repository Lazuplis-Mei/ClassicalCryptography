namespace ClassicalCryptography.Transposition;

/// <summary>
/// 取后平移密码
/// </summary>
[Introduction("取后平移密码", "每次取出n个字符后，剩余部分平移k个单位。")]
public partial class TakeTranslateCipher : TranspositionCipher<(int N, int K)>
{
    private static TranspositionCipher<(int, int)>? cipher;

    /// <summary>
    /// <see cref="TakeTranslateCipher"/>的实例
    /// </summary>
    public static TranspositionCipher<(int, int)> Cipher => cipher ??= new TakeTranslateCipher();

    /// <inheritdoc/>
    protected override ushort[] Transpose(ushort[] indexes, IKey<(int N, int K)> key)
    {
        (int n, int k) = key.KeyValue;
        if (k == 0 || n + k >= indexes.Length)
            return indexes;
        
        var list = new LinkedList(indexes);
        while (list.MoveStep(n - 1) && !list.IsEnd)
        {
            var list2 = list.SubList();
            if (!list2!.MoveStep(k - 1))
                break;
            var list3 = list2.SubList();
            if (list3 is null)
                break;
            list.LinkCurrent(list3);
            list3.LinkLast(list2);
            list.EndCurrent(list2);
            list.MoveStep(1);
        }
        return list.ToArray();
    }
}
