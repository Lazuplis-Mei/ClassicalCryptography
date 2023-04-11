namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 数字华容道密码
/// </summary>
[Introduction("数字华容道密码", "移动步骤将会打乱字符")]
public partial class FifteenPuzzle : TranspositionCipher2D<QuaterArray>
{
    private static TranspositionCipher2D<QuaterArray>? cipher;

    /// <summary>
    /// <see cref="FifteenPuzzle"/>的实例
    /// </summary>
    public static TranspositionCipher2D<QuaterArray> Cipher => cipher ??= new FifteenPuzzle();

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<QuaterArray> key)
    {
        int n = textLength.SqrtCeil();
        return (n, n);
    }

    /// <inheritdoc/>
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<QuaterArray> key)
    {
        int n = indexes.GetLength(0);
        int x = n - 1;
        int y = x;
        foreach (var move in key.KeyValue)
        {
            ushort value;
            switch (move)
            {
                case 0:
                    value = indexes[x, y];
                    indexes[x, y] = indexes[x + 1, y];
                    indexes[x + 1, y] = value;
                    x++;
                    break;
                case 1:
                    value = indexes[x, y];
                    indexes[x, y] = indexes[x, y - 1];
                    indexes[x, y - 1] = value;
                    y--;
                    break;
                case 2:
                    value = indexes[x, y];
                    indexes[x, y] = indexes[x - 1, y];
                    indexes[x - 1, y] = value;
                    x--;
                    break;
                case 3:
                    value = indexes[x, y];
                    indexes[x, y] = indexes[x, y + 1];
                    indexes[x, y + 1] = value;
                    y++;
                    break;
            }
        }
        return indexes;
    }
}
