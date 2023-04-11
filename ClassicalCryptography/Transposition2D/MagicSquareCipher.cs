namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 幻方顺序密码
/// </summary>
[Introduction("幻方顺序密码", "用n阶幻方的数字顺序来表示文本。")]
public class MagicSquareCipher : TranspositionCipher2D
{
    private static TranspositionCipher2D? cipher;

    /// <summary>
    /// <see cref="MagicSquareCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D Cipher => cipher ??= new MagicSquareCipher();

    /// <summary>
    /// 幻方正序密码
    /// </summary>
    public MagicSquareCipher() => FillOrder = false;

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength)
    {
        int n = textLength.SqrtCeil();
        return (n, n);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes)
    {
        int n = indexes.GetLength(0);
        if ((n & 1) == 1)
            LouberelFill(indexes, n);
        else if ((n & 0B11) == 0)
            ExchangeFill(indexes, n);
        else
            StracheyFill(indexes, n);
        return indexes;
    }

    private static void StracheyFill(ushort[,] indexes, int n)
    {
        int h = n >> 1, q = n >> 2, m = h * h;
        LouberelFill(indexes, h);
        LouberelFill(indexes, h, h, h, m);
        LouberelFill(indexes, h, h, 0, m << 1);
        LouberelFill(indexes, h, 0, h, (m << 1) + m);
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < q; x++)
            {
                int t = y == q ? 1 : 0;
                (indexes[x + t, y + h], indexes[x + t, y]) = (indexes[x + t, y], indexes[x + t, y + h]);
            }
            for (int x = 1; x < q; x++)
            {
                (indexes[n - x, y + h], indexes[n - x, y]) = (indexes[n - x, y], indexes[n - x, y + h]);
            }
        }
    }

    private static void ExchangeFill(ushort[,] indexes, int n)
    {
        for (int i = 0; i < n; i += 4)
        {
            for (int j = 0; j < n; j += 4)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    for (int jj = 0; jj < 4; jj++)
                    {
                        int k = ii + i + (jj + j) * n;
                        if (ii == jj || ii + jj == 3)
                            indexes[ii + i, jj + j] = (ushort)k;
                        else
                            indexes[ii + i, jj + j] = (ushort)(indexes.Length - k - 1);
                    }
                }
            }
        }
    }

    private static void LouberelFill(ushort[,] indexes, int n)
    {
        int i = n >> 1, j = n - 1, size = n * n;
        indexes[i++, 0] = 1;
        for (ushort k = 1; k < size; k++)
        {
            indexes[i, j] = k;
            i++;
            j--;
            if (i == n && j == -1)
            {
                i = n - 1;
                j = 1;
            }
            else if (j == -1)
                j = n - 1;
            else if (i == n)
                i = 0;
            if (indexes[i, j] != 0)
            {
                i--;
                j += 2;
            }
        }
        indexes[n >> 1, 0] = 0;
    }

    private static void LouberelFill(ushort[,] indexes, int n, int si, int sj, int start)
    {
        int i = n >> 1, j = 0, size = n * n;
        for (ushort k = 0; k < size; k++)
        {
            indexes[si + i, sj + j] = (ushort)(k + start);
            i++;
            j--;
            if (i == n && j == -1)
            {
                i = n - 1;
                j = 1;
            }
            else if (j == -1)
                j = n - 1;
            else if (i == n)
                i = 0;
            if (indexes[si + i, sj + j] != 0)
            {
                i--;
                j += 2;
            }
        }
    }
}
