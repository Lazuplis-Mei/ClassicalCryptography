using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 魔改Baker's映射变换密码
/// </summary>
[Introduction("魔改Baker's映射变换密码",
    "文本按照以Baker's映射(x,y)=>(2x,y/2)|2x>=N|(2x-N,(y+N)/2)为基础魔改的版本的顺序加密文本。")]
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
                if ((x << 1) >= N)
                {
                    xi = (x << 1) - N;
                    xi += (N + 1) & 1;
                    yi = (y + N) >> 1;
                }
                if (((y + N) & 1) == 1)
                    yi = N - yi - 1;
                indexes[xi, yi] = (ushort)(x + y * N);
            }
        }
        return indexes;
    }
}