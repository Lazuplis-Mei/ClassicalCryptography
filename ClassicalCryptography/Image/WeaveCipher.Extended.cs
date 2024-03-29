﻿using System.Drawing;
using ZXing.Common;

namespace ClassicalCryptography.Image;

public partial class WeaveCipher
{
    private static readonly Point pointLT = Point.Empty;
    private static readonly Point pointLB = new(0, BLOCK_SIZE);
    private static readonly Point pointRT = new(BLOCK_SIZE, 0);
    private static readonly Point pointRB = new(BLOCK_SIZE, BLOCK_SIZE);

    private static readonly Point[][] trigs = new[]
    {
        new[] { pointLT, pointLB, pointRB },
        new[] { pointLT, pointRT, pointRB },
        new[] { pointLT, pointLB, pointRT },
        new[] { pointLB, pointRT, pointRB }
    };

    /// <summary>
    /// 加密为图像
    /// </summary>
    public static Bitmap EncryptExtend(Span<byte> bytes1, Span<byte> bytes2, Span<byte> bytes3, Span<byte> bytes4)
    {
        Guard.HasSizeEqualTo(bytes3, bytes1.Length);
        Guard.HasSizeEqualTo(bytes4, bytes2.Length);

        var bits1 = new BitArray();
        var bits2 = new BitArray();
        var bits3 = new BitArray();
        var bits4 = new BitArray();

        for (int i = 0; i < bytes1.Length; i++)
        {
            bits1.appendBits(bytes1[i], 8);
            bits3.appendBits(bytes3[i], 8);
        }
        for (int i = 0; i < bytes2.Length; i++)
        {
            bits2.appendBits(bytes2[i], 8);
            bits4.appendBits(bytes4[i], 8);
        }
        return BitsToImageExtend(EncryptBitsExtend(bits1, bits2, bits3, bits4));
    }

    /// <summary>
    /// 加密为图像
    /// </summary>
    [SkipLocalsInit]
    public static Bitmap EncryptExtend(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text).AsSpan();
        int h = (bytes.Length + bytes.Length % 2) / 2;
        int size = h - h / 2;
        Span<byte> bytes4 = size.CanAlloc() ? stackalloc byte[size] : new byte[size];
        var bytes1 = bytes[..(h / 2)];
        var bytes2 = bytes[(h / 2)..h];
        var bytes3 = bytes.Slice(h, h / 2);
        bytes[(h + h / 2)..].CopyTo(bytes4);
        return EncryptExtend(bytes1, bytes2, bytes3, bytes4);
    }

    internal static BitMatrix EncryptBitsExtend(BitArray bits1, BitArray bits2, BitArray bits3, BitArray bits4)
    {
        Guard.IsEqualTo(bits3.Size, bits1.Size);
        Guard.IsEqualTo(bits4.Size, bits2.Size);

        var matrix = new BitMatrix((bits1.Size + 1) * 2, bits2.Size + 1);

        matrix[1, 0] = true;

        for (int x = 2; x < matrix.Width; x++)
        {
            bool flag = false;
            if (x % 2 == 0)
                flag = bits1[x / 2 - 1];
            else if (x % 4 == 3)
                flag = bits4[x / 4];
            else if (x % 4 == 1)
                flag = bits3[bits1.Size / 2 - x / 4];
            matrix[x, 0] = flag ? !matrix[x - 1, 0] : matrix[x - 1, 0];
        }

        for (int y = 1; y < matrix.Height; y++)
        {
            for (int x = 0; x < matrix.Width; x++)
            {
                if (2 * y + 1 == x)
                    matrix[x, y] = y % 2 == 0 ? !matrix[x - 1, y] : matrix[x - 1, y];
                else if (y % 2 == 1)
                {
                    if (x % 4 == 0)
                        matrix[x, y] = bits2[y - 1] ? !matrix[x, y - 1] : matrix[x, y - 1];
                    else if (x % 4 == 3)
                        matrix[x, y] = (!bits2[y - 1]) ? !matrix[x, y - 1] : matrix[x, y - 1];
                    else if (x % 4 == 1)
                        matrix[x, y] = (((x / 4 + y / 2) < (bits2.Size + 1) / 2) == (y % 2 == 1)
                            ? !bits4[x / 4 + y / 2] : bits4[x / 4 + y / 2])
                            ? !matrix[x - 1, y] : matrix[x - 1, y];
                    else if (x % 2 == 0)
                        matrix[x, y] = (y % 2 == 0 ? bits1[x / 2 - 1] : !bits1[x / 2 - 1])
                            ? !matrix[x - 1, y] : matrix[x - 1, y];
                }
                else
                {
                    if (x % 4 == 1)
                        matrix[x, y] = bits2[y - 1] ? !matrix[x, y - 1] : matrix[x, y - 1];
                    else if (x % 4 == 2)
                        matrix[x, y] = (!bits2[y - 1]) ? !matrix[x, y - 1] : matrix[x, y - 1];
                    else if (x % 4 == 3)
                        matrix[x, y] = (((x / 4 + y / 2) < (bits2.Size + 1) / 2) == (y % 2 == 1)
                            ? !bits4[x / 4 + y / 2] : bits4[x / 4 + y / 2])
                            ? !matrix[x - 1, y] : matrix[x - 1, y];
                    else if (x == 0)
                        matrix[x, y] = (bits3[bits1.Size / 2 + y / 2 - 1] != bits2[y - 1])
                            ? !matrix[x + 1, y - 1] : matrix[x + 1, y - 1];
                    else if (x % 2 == 0)
                        matrix[x, y] = bits1[x / 2 - 1] ? !matrix[x - 1, y] : matrix[x - 1, y];
                }
            }
        }
        return matrix;
    }

    internal static Bitmap BitsToImageExtend(BitMatrix matrix)
    {
        var bitmap = new Bitmap((matrix.Width / 2) * BLOCK_SIZE, matrix.Height * BLOCK_SIZE);
        using var graphics = Graphics.FromImage(bitmap);
        for (int y = 0; y < matrix.Height; y++)
        {
            for (int x = 0; x < matrix.Width; x += 4)
            {
                int k = 2 * (y % 2);
                graphics.FillPolygon(matrix[x, y] ? Foreground : Background, trigs[k]);
                graphics.FillPolygon(matrix[x + 1, y] ? Foreground : Background, trigs[k + 1]);
                graphics.Translate(BLOCK_SIZE);
                graphics.FillPolygon(matrix[x + 2, y] ? Foreground : Background, trigs[2 - k]);
                graphics.FillPolygon(matrix[x + 3, y] ? Foreground : Background, trigs[3 - k]);
                graphics.Translate(BLOCK_SIZE);
            }
            graphics.TranslateBreakLine(BLOCK_SIZE);
        }
        return bitmap;
    }
}
