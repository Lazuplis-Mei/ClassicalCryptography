using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 旋转栅格密码
/// </summary>
/// <remarks>
/// <see href="https://en.wikipedia.org/wiki/Grille_(cryptography)#Turning_grilles"/>
/// </remarks>
[Introduction("旋转栅格密码", "在一个4N^2的方格阵列里恰当的选择一些空位，通过旋转的方式依次填入所有的信息。")]
public partial class RotatingGrillesCipher : TranspositionCipher2D<QuaterArray>
{
    /// <summary>
    /// 是否逆时针旋转
    /// </summary>
    public bool AntiClockwise { get; set; }

    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    /// <param name="key">密钥</param>
    protected override (int Width, int Height) Partition(int textLength, IKey<QuaterArray> key)
    {
        int N = (int)Math.Sqrt(key.KeyValue.Count);
        return (N << 1, N << 1);
    }
    /// <summary>
    /// 旋转栅格密码
    /// </summary>
    public RotatingGrillesCipher()
    {
        FillOrder = false;
    }
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    /// <param name="key">密钥</param>
    [SkipLocalsInit]
    protected override ushort[,] Transpose(ushort[,] indexes, IKey<QuaterArray> key)
    {
        ushort count = (ushort)key.KeyValue.Count;
        int N = (int)Math.Sqrt(count);
        int length = indexes.GetLength(0);
        Span<int> rot = stackalloc int[4];
        Span<int> pos = stackalloc int[count];
        int k = 0;
        for (int i = 0; i < 4; i++, k += count)
        {
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    rot[0] = x;
                    rot[1] = length - y - 1;
                    rot[2] = length - x - 1;
                    rot[3] = y;
                    int j = x + (N * y);
                    int t = key.KeyValue[j] + (AntiClockwise ? 4 - i : i);
                    pos[j] = rot[t % 4] + rot[(t + 3) % 4] * length;
                }
            }
            pos.Sort();
            for (int j = 0; j < count; j++)
            {
                int x = pos[j] % length;
                int y = pos[j] / length;
                indexes[x, y] = (ushort)(k + j);
            }
        }
        return indexes;
    }

}
