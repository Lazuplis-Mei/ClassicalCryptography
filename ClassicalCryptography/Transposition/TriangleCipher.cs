using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Transposition;

/// <summary>
/// 三角排列密码
/// </summary>
[Introduction("三角排列密码", "文本按行排列成三角形，按列读出。")]
public class TriangleCipher : TranspositionCipher
{
    /// <summary>
    /// 三角排列密码
    /// </summary>
    public TriangleCipher()
    {
        FillOrder = false;
    }
    /// <summary>
    /// 补足长度
    /// </summary>
    /// <param name="length">文本长度</param>
    protected override int PadLength(int length)
    {
        int N = length.SqrtCeil();
        return N * N;
    }
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    protected override ushort[] Transpose(ushort[] indexes)
    {
        int N = (int)Math.Sqrt(indexes.Length);
        int i = 0;
        for (int j = N - 1; j >= 0; j--)
        {
            indexes[i++] = (ushort)(j * j);
            for (int k = j + 1; k < N; k++, i++)
                indexes[i] = (ushort)(indexes[i - 1] + (k << 1));
        }
        for (int j = 2; j <= N; j++)
        {
            indexes[i++] = (ushort)(j * j - 1);
            for (int k = j; k < N; k++, i++)
                indexes[i] = (ushort)(indexes[i - 1] + (k << 1));
        }
        return indexes;
    }
}