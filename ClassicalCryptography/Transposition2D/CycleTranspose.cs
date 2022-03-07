using ClassicalCryptography.Interfaces;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 周期置换密码
/// </summary>
[Introduction("周期/列置换密码", "明文排成方阵，并根据每组密钥顺序进行列的轮换，按行/列读出。")]
public partial class CycleTranspose : TranspositionCipher2D<ushort[][]>
{
    
    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    /// <param name="key">密钥</param>
    protected override (int Width, int Height) Partition(int textLength, IKey<ushort[][]> key)
    {
        int width = 0;
        ushort[][] keyValue = key.KeyValue;
        for (int i = 0; i < keyValue.Length; i++)
            for (int j = 0; j < keyValue[i].Length; j++)
                width = Math.Max(width, keyValue[i][j]);
        int height = textLength / width;
        height += (textLength % width) == 0 ? 0 : 1;
        return (width, height);
    }
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    /// <param name="key">密钥</param>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<ushort[][]> key)
    {
        int height = indexes.GetLength(1);
        Span<ushort> t = stackalloc ushort[height << 1];
        Span<ushort> t1 = t[..height], t2 = t[height..];
        ushort[][] keyValue = key.KeyValue;
        for (int i = 0; i < keyValue.Length; i++)
        {
            ushort[] cycle = keyValue[i];
            int m = cycle[^1] - 1;
            for (int j = 0; j < height; j++)
                t1[j] = indexes[m, j];

            for (int k = 0; k < cycle.Length; k++)
            {
                for (int j = 0; j < height; j++)
                {
                    m = cycle[k] - 1;
                    t2[j] = indexes[m, j];
                    indexes[m, j] = t1[j];
                }
                t = t1; t1 = t2; t2 = t;
            }
        }
        return indexes;
    }


}
