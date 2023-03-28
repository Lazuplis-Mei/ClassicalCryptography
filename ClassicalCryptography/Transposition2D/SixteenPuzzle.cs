namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 移动数字华容道密码
/// </summary>
[Introduction("移动数字华容道密码", "移动整行列将会打乱字符")]
public partial class SixteenPuzzle : TranspositionCipher2D<short[]>
{
    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<short[]> key)
    {
        int N = textLength.SqrtCeil();
        return (N, N);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<short[]> key)
    {
        int N = indexes.GetLength(0);
        foreach (var move in key.KeyValue)
        {
            ushort value;
            if (move > 0)
            {
                int y = move - 1;
                value = indexes[N - 1, y];
                for (int x = N - 1; x > 0; x--)
                    indexes[x, y] = indexes[x - 1, y];
                indexes[0, y] = value;
            }
            else if (move < 0)
            {
                int x = -move - 1;
                value = indexes[x, N - 1];
                for (int y = N - 1; y > 0; y--)
                    indexes[x, y] = indexes[x, y - 1];
                indexes[x, 0] = value;
            }
        }
        return indexes;
    }
}