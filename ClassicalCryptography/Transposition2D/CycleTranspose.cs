using CommunityToolkit.HighPerformance;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 周期置换密码
/// </summary>
[Introduction("周期置换密码", "明文排成方阵，并根据每组密钥顺序进行列的轮换，按行/列读出。")]
public partial class CycleTranspose : TranspositionCipher2D<ushort[][]>
{
    private static TranspositionCipher2D<ushort[][]>? cipher;

    /// <summary>
    /// <see cref="CycleTranspose"/>的实例
    /// </summary>
    public static TranspositionCipher2D<ushort[][]> Cipher => cipher ??= new CycleTranspose();

    /// <inheritdoc/>
    protected override (int Width, int Height) Partition(int textLength, IKey<ushort[][]> key)
    {
        int width = 0;
        ushort[][] keyValue = key.KeyValue;
        for (int i = 0; i < keyValue.Length; i++)
            for (int j = 0; j < keyValue[i].Length; j++)
                width = Math.Max(width, keyValue[i][j]);
        int height = textLength.DivCeil(width);
        return (width, height);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<ushort[][]> key)
    {
        var span2D = indexes.AsSpan2D();
        int height = indexes.GetLength(1);
        int size = height << 1;
        Span<ushort> t = size.CanAllocInt16() ? stackalloc ushort[size] : new ushort[size];
        Span<ushort> t1 = t[..height], t2 = t[height..];
        foreach (var cycle in key.KeyValue)
        {
            var rowSpan = span2D.GetRowSpan(cycle[^1] - 1);
            rowSpan.CopyTo(t1);
            foreach (var value in cycle)
            {
                rowSpan = span2D.GetRowSpan(value - 1);
                rowSpan.CopyTo(t2);
                t1.CopyTo(rowSpan);
                t = t1;
                t1 = t2;
                t2 = t;
            }
        }
        return indexes;
    }
}
