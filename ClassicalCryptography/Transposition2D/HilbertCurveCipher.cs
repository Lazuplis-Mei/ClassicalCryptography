using ClassicalCryptography.Interfaces;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 希尔伯特曲线密码
/// </summary>
[Introduction("希尔伯特曲线密码", "使用希尔伯特曲线顺序加密文本。")]
public class HilbertCurveCipher : TranspositionCipher2D
{
    /// <summary>
    /// 希尔伯特曲线密码
    /// </summary>
    public HilbertCurveCipher()
    {
        FillOrder = false;
    }
    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    protected override (int Width, int Height) Partition(int textLength)
    {
        int N = 1 << (int)Math.Ceiling(Math.Log2(Math.Sqrt(textLength)));
        return (N, N);
    }
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    protected override ushort[,] Transpose(ushort[,] indexes)
    {
        HilbertCurve(indexes, indexes.GetLength(0));
        return indexes;
    }

    private static void HilbertCurve(ushort[,] indexes, int N)
    {
        if (N == 2)
        {
            InternalInit(indexes);
            return;
        }
        int n = N >> 1;
        HilbertCurve(indexes, n);//左上角初始化
        RightPart(indexes, n);
        FlipLeftTop(indexes, n);
        FlipLeftBottom(indexes, n);
        static void InternalInit(ushort[,] indexes)
        {
            indexes[0, 0] = 0;
            indexes[1, 0] = 1;
            indexes[1, 1] = 2;
            indexes[0, 1] = 3;
        }
        static void RightPart(ushort[,] indexes, int n)
        {
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    indexes[x + n, y] = (ushort)(indexes[x, y] + n * n);
                    indexes[x + n, y + n] = (ushort)(indexes[x, y] + 2 * n * n);
                    indexes[x, y + n] = (ushort)(indexes[x, y] + 3 * n * n);
                }
            }
        }
        static void FlipLeftBottom(ushort[,] indexes, int n)
        {
            for (int y = 0; y < n - 1; y++)
            {
                for (int x = 0; x + y < n - 1; x++)
                {
                    ushort t = indexes[x, y + n];
                    indexes[x, y + n] = indexes[n - y - 1, n + n - x - 1];
                    indexes[n - y - 1, n + n - x - 1] = t;
                }
            }
        }
        static void FlipLeftTop(ushort[,] indexes, int n)
        {
            for (int x = 0; x < n - 1; x++)
            {
                for (int y = x + 1; y < n; y++)
                {
                    ushort t = indexes[x, y];
                    indexes[x, y] = indexes[y, x];
                    indexes[y, x] = t;
                }
            }
        }
    }
}
