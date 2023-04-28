using System.Drawing;
using System.Runtime.Versioning;
using ZXing.Common;

namespace ClassicalCryptography.Image;

/// <summary>
/// <a href="https://tieba.baidu.com/p/7814788182">编织图形密码</a>
/// </summary>
/// <remarks>
/// 注意对于扩展的(带有三角形的)图形的实现，我没有完全根据天青的设想<br/>
/// 它的正式名称应该是Hitomezashi Stitch Patterns(一目刺し)，请参考<br/>
/// <see href="https://www.felissimo.co.jp/couturier/blog/categorylist/japanese-handicraft/sashiko/post-14871/">japanese-handicraft</see>
/// </remarks>
[Introduction("编织图形密码", "https://tieba.baidu.com/p/7814788182")]
[SupportedOSPlatform("windows")]
public partial class WeaveCipher
{
    private const int BLOCK_SIZE = 16;
    private const int MID_POINT = BLOCK_SIZE / 2;

    /// <summary>
    /// 前景颜色
    /// </summary>
    public static Brush Foreground { get; set; } = Brushes.Black;

    /// <summary>
    /// 背景颜色
    /// </summary>
    public static Brush Background { get; set; } = Brushes.White;

    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 加密为图像
    /// </summary>
    public static Bitmap Encrypt(Span<byte> bytes1, Span<byte> bytes2)
    {
        var bits1 = new BitArray();
        var bits2 = new BitArray();

        for (int i = 0; i < bytes1.Length; i++)
            bits1.appendBits(bytes1[i], 8);
        for (int i = 0; i < bytes2.Length; i++)
            bits2.appendBits(bytes2[i], 8);

        return BitsToImage(EncryptBits(bits1, bits2));
    }

    /// <summary>
    /// 加密为图像
    /// </summary>
    public static Bitmap Encrypt(string text)
    {
        var bytes = Encoding.GetBytes(text).AsSpan();
        var bytes1 = bytes[..(bytes.Length / 2)];
        var bytes2 = bytes[(bytes.Length / 2)..];
        return Encrypt(bytes1, bytes2);
    }

    /// <summary>
    /// 解密图像
    /// </summary>
    public static string Decrypt(Bitmap bitmap)
    {
        var columns = bitmap.Width / BLOCK_SIZE;
        var rows = bitmap.Height / BLOCK_SIZE;
        var bytes = new byte[(columns + rows) >> 3];
        var bits = new BitArray();
        for (int x = BLOCK_SIZE + MID_POINT; x < bitmap.Width; x += BLOCK_SIZE)
            bits.appendBit(bitmap.GetPixel(x, MID_POINT) != bitmap.GetPixel(x - BLOCK_SIZE, MID_POINT));
        for (int y = BLOCK_SIZE + MID_POINT; y < bitmap.Height; y += BLOCK_SIZE)
            bits.appendBit(bitmap.GetPixel(MID_POINT, y) != bitmap.GetPixel(MID_POINT, y - BLOCK_SIZE));
        bits.toBytes(0, bytes, 0, bytes.Length);
        return Encoding.GetString(bytes);
    }

    /// <summary>
    /// 加密为矩阵
    /// </summary>
    public static BitMatrix EncryptBits(BitArray bits1, BitArray bits2)
    {
        var matrix = new BitMatrix(bits1.Size + 1, bits2.Size + 1);

        for (int x = 1; x < matrix.Width; x++)
            matrix[x, 0] = bits1[x - 1] ? !matrix[x - 1, 0] : matrix[x - 1, 0];

        for (int y = 1; y < matrix.Height; y++)
            for (int x = 0; x < matrix.Width; x++)
                matrix[x, y] = (x % 2 == 0 ? bits2[y - 1] : !bits2[y - 1])
                    ? !matrix[x, y - 1] : matrix[x, y - 1];

        return matrix;
    }

    /// <summary>
    /// 加密矩阵为图像
    /// </summary>
    public static Bitmap BitsToImage(BitMatrix matrix)
    {
        var bitmap = new Bitmap(matrix.Width * BLOCK_SIZE, matrix.Height * BLOCK_SIZE);
        using var graphics = Graphics.FromImage(bitmap);
        for (int x = 0; x < matrix.Width; x++)
        {
            for (int y = 0; y < matrix.Height; y++)
            {
                graphics.FillRectangle(matrix[x, y] ? Foreground : Background,
                    x * BLOCK_SIZE, y * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
            }
        }
        return bitmap;
    }
}
