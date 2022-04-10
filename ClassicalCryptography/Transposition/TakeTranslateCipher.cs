using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Transposition;

/// <summary>
/// 取平移密码
/// </summary>
[Introduction("取平移密码", "每次取出n个字符后，剩余部分平移k个单位。")]
public partial class TakeTranslateCipher : TranspositionCipher<(int N, int K)>
{

    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序(该参数不会被写入)</param>
    /// <param name="key">密钥</param>
    protected override ushort[] Transpose(ushort[] indexes, IKey<(int N, int K)> key)
    {
        (int N, int K) = key.KeyValue;
        if (K != 0 && N + K < indexes.Length)
        {
            var list = new LinkedList(indexes);
            while (list.MoveStep(N - 1) && !list.IsEnd)
            {
                var list2 = list.SubList();
                if (!list2!.MoveStep(K - 1))
                    break;
                var list3 = list2.SubList();
                if (list3 == null)
                    break;
                list.LinkCurrent(list3);
                list3.LinkLast(list2);
                list.EndCurrent(list2);
                list.MoveStep(1);
            }
            return list.ToArray();
        }
        return indexes;
    }
}
