using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Transposition;


/// <summary>
/// 栅栏密码
/// </summary>
[Introduction("栅栏密码", "按字数分割，并按列读出。")]
public partial class RailFenceCipher : TranspositionCipher<int>
{
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    /// <param name="key">密钥</param>
    protected override ushort[] Transpose(ushort[] indexes, IKey<int> key)
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
