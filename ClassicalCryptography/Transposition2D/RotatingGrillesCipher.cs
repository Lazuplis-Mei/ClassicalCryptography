using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 旋转栅格密码
/// </summary>
/// <remarks>
/// <see href="https://en.wikipedia.org/wiki/Grille_(cryptography)#Turning_grilles">wikipedia/Grille_(cryptography)#Turning_grilles</see>
/// </remarks>
[Introduction("旋转栅格密码", "在一个4n^2的方格阵列里恰当的选择一些空位，通过旋转的方式依次填入所有的信息。")]
public partial class RotatingGrillesCipher : TranspositionCipher2D<QuaterArray>
{
    private static TranspositionCipher2D<QuaterArray>? cipher;

    /// <summary>
    /// <see cref="RotatingGrillesCipher"/>的实例
    /// </summary>
    public static TranspositionCipher2D<QuaterArray> Cipher => cipher ??= new RotatingGrillesCipher();

    /// <summary>
    /// 旋转栅格密码
    /// </summary>
    public RotatingGrillesCipher() => FillOrder = false;

    /// <summary>
    /// 是否逆时针旋转
    /// </summary>
    public bool AntiClockwise { get; set; }

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<QuaterArray> key)
    {
        int n = key.KeyValue.Count.SqrtCeil() << 1;
        return (n, n);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<QuaterArray> key)
    {
        int count = key.KeyValue.Count;
        int n = count.SqrtCeil();
        int length = indexes.GetLength(0);
        Span<int> rot = stackalloc int[4];
        Span<int> pos = count.CanAllocInt32() ? stackalloc int[count] : new int[count];
        for (int k = 0, i = 0; i < 4; i++, k += count)
        {
            for (int x = 0; x < n; x++)
            {
                rot[0] = x;
                rot[2] = length - x - 1;
                for (int y = 0; y < n; y++)
                {
                    rot[3] = y;
                    rot[1] = length - y - 1;
                    int j = x + n * y;
                    int t = key.KeyValue[j] + (AntiClockwise ? 4 - i : i);
                    pos[j] = rot[t & 0B11] + rot[(t + 3) & 0B11] * length;
                }
            }
            pos.Sort();
            for (int j = 0; j < count; j++)
            {
                (int y, int x) = int.DivRem(pos[j], length);
                indexes[x, y] = (ushort)(k + j);
            }
        }
        return indexes;
    }
}
