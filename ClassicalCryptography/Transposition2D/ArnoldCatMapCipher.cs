namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 猫映射变换密码<a href="https://en.wikipedia.org/wiki/Arnold%27s_cat_map">Arnold's_cat_map</a>
/// </summary>
[Introduction("猫映射变换密码", "文本按照Arnold猫变换的顺序加密文本。")]
public class ArnoldCatMapCipher : TranspositionCipher2D
{
    /// <summary>
    /// 猫映射密码
    /// </summary>
    public ArnoldCatMapCipher()
    {
        FillOrder = false;
    }
    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    protected override (int Width, int Height) Partition(int textLength)
    {
        int N = textLength.SqrtCeil();
        return (N, N);
    }
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    protected override ushort[,] Transpose(ushort[,] indexes)
    {
        int N = indexes.GetLength(0);
        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                int xi = ((x << 1) + y) % N;
                int yi = (x + y) % N;
                indexes[xi, yi] = (ushort)(x + y * N);
            }
        }
        return indexes;
    }
}
