using ClassicalCryptography.Interfaces;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

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
        int M = N >> 2, n = N >> 1;
        LouberelMethod(indexes, n);
        LouberelMethod(indexes, n, n, n, n * n);
        LouberelMethod(indexes, n, n, 0, 2 * n * n);
        LouberelMethod(indexes, n, 0, n, 3 * n * n);
        for (int y = 0; y < n; y++)
        {
            ushort temp;
            for (int x = 0; x < M; x++)
            {
                int m = y == M ? 1 : 0;
                temp = indexes[x + m, y];
                indexes[x + m, y] = indexes[x + m, y + n];
                indexes[x + m, y + n] = temp;
            }
            for (int x = 1; x < M; x++)
            {
                temp = indexes[N - x, y];
                indexes[N - x, y] = indexes[N - x, y + n];
                indexes[N - x, y + n] = temp;
            }
        }
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
        int i = N >> 1, j = N - 1;
        indexes[i++, 0] = 1;
        for (ushort k = 1; k < N * N; k++)
        {
            indexes[i, j] = k;
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
        indexes[N >> 1, 0] = 0;
    }
    //si, sj, start != 0
    private static void LouberelMethod(ushort[,] indexes, int N, int si, int sj, int start)
    {
        int i = N >> 1, j = 0;
        for (ushort k = 0; k < N * N; k++)
        {
            indexes[si + i, sj + j] = (ushort)(k + start);
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
            if (indexes[si + i, sj + j] != 0)
            {
                i--;
                j += 2;
            }
        }
    }
}