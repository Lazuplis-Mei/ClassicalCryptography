using ClassicalCryptography.Interfaces;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 幻方正序密码
/// </summary>
[Introduction("幻方正序密码", "用N阶幻方的数序表示文本。")]
public class MagicSquareCipher : TranspositionCipher2D
{
    /// <summary>
    /// 幻方正序密码
    /// </summary>
    public MagicSquareCipher()
    {
        FillOrder = false;
    }
    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    protected override (int Width, int Height) Partition(int textLength)
    {
        int N = (int)Math.Ceiling(Math.Sqrt(textLength));
        return (N, N);
    }

    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    protected override ushort[,] Transpose(ushort[,] indexes)
    {
        int N = indexes.GetLength(0);
        if (N % 2 == 1)//奇数阶幻方，louberel法
            LouberelMethod(indexes, N);
        else if (N % 4 == 0)//双偶阶幻方，对称交换法
            Exchange(indexes, N);
        else
            StracheyMethod(indexes, N);
        return indexes;
    }

    private static void StracheyMethod(ushort[,] indexes, int N)
    {
        int M = N / 4;
        throw new NotImplementedException();
    }

    private static void Exchange(ushort[,] indexes, int N)
    {
        for (int i = 0; i < N; i += 4)
        {
            for (int j = 0; j < N; j += 4)
            {
                for (int ki = 0; ki < 4; ki++)
                {
                    for (int kj = 0; kj < 4; kj++)
                    {
                        int k = ki + i + ((kj + j) * N);
                        if (ki == kj || ki + kj == 3)
                            indexes[ki + i, kj + j] = (ushort)k;
                        else
                            indexes[ki + i, kj + j] = (ushort)(indexes.Length - k - 1);
                    }
                }
            }
        }
    }

    private static void LouberelMethod(ushort[,] indexes, int N)
    {
        int i = N / 2, j = 0;
        for (ushort k = 0; k < indexes.Length; k++)
        {
            indexes[i, j] = k == 0 ? (ushort)1 : k;
            i++;
            j--;
            if (i == N && j == -1)
            {
                i = N - 1;
                j = 1;
            }
            else if (j == -1)
                j = N - 1;
            else if (i == N)
                i = 0;
            if (indexes[i, j] != 0)
            {
                i--;
                j += 2;
            }
        }
        indexes[N / 2, 0] = 0;
    }
}