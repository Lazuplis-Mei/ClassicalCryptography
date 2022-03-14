using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 锯齿分割密码
/// </summary>
[Introduction("锯齿分割密码", "文本方阵边长的整数分拆为界限排列成锯齿,以特点顺序加密文本。")]
public partial class JigsawCipher : TranspositionCipher2D<ushort[]>
{
    /// <summary>
    /// 锯齿分割密码
    /// </summary>
    public JigsawCipher()
    {
        FillOrder = false;
    }
    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    /// <param name="key">密钥</param>
    protected override (int Width, int Height) Partition(int textLength, IKey<ushort[]> key)
    {
        int N = textLength.SqrtCeil();
        return (N, N);
    }

    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    /// <param name="key">密钥</param>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<ushort[]> key)
    {
        int N = indexes.GetLength(0);
        ushort val = (ushort)(N - 1), col = (ushort)N;
        for (int j = key.KeyValue.Length - 1; j >= 0; j--)//N的某个(有序的)整数分拆
        {
            ushort p = key.KeyValue[j];
            col -= p;
            int y = 0, k = 0, row = 0;
            for (int i = 0; i < p; i++)
            {
                if (k != 0)
                {
                    for (int x = 0; x < p - k; x++)
                        indexes[col + x, y] = val--;
                    row += ((N - (p - k)) / p) + 1;
                    y++;
                }
                else
                    row += N / p;
                for (; y < row; y++)
                    for (int x = 0; x < p; x++)
                        indexes[col + x, y] = val--;
                k = k != 0 ? (N - (p - k)) % p : N % p;
                for (int x = p - k; x < p; x++)
                    indexes[col + x, y] = val--;
                val += (ushort)(N << 1);
            }
        }
        return indexes;
    }
}