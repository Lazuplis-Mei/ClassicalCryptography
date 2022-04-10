using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Transposition;

/// <summary>
/// 约瑟夫置换
/// </summary>
[Introduction("约瑟夫置换密码", "约瑟夫问题中，出列次序。")]
public partial class JosephusCipher : TranspositionCipher<int>
{
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    /// <param name="key">密钥</param>
    protected override ushort[] Transpose(ushort[] indexes, IKey<int> key)
    {
        int M = key.KeyValue;
        var linkedList = new LinkedList(indexes);
        linkedList.Last.Next = linkedList.First;
        int p = 0;
        linkedList.MoveStep(M - 2);
        while (p < indexes.Length)
        {
            var pre = linkedList.Current;
            linkedList.Current = pre.Next!;
            indexes[p++] = linkedList.Current.Value;
            pre.Next = linkedList.Current.Next!;
            linkedList.MoveStep(M - 1);
        }
        return indexes;
    }
}
