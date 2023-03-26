namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 面包师映射变换密码<a href="https://en.wikipedia.org/wiki/Baker%27s_map">Baker's_map</a>
/// </summary>
/// <remarks>
/// 原始版本的变换公式存在一定程度上的不便<br/>
/// 实际的变换过程以代码为准
/// </remarks>
[Introduction("面包师映射变换密码", "文本按照面包师映射变换的顺序加密文本。")]
public class VBakersMapCipher : TranspositionCipher2D
{
    /// <summary>
    /// Baker's映射变换密码
    /// </summary>
    public VBakersMapCipher()
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
                int xi = x << 1, yi = y >> 1;
                if ((N & 1) == 1 && y == N - 1)
                    xi = x;
                else
                {
                    xi += y & 1;
                    if (xi >= N)
                    {
                        xi -= N;
                        yi += N >> 1;
                        yi += N & 1;
                    }
                }
                indexes[xi, yi] = (ushort)(x + y * N);
            }
        }
        return indexes;
    }
}