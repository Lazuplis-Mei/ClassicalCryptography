﻿using System.Collections;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Interfaces;
/// <summary>
/// 换位密码使用的扩展方法
/// </summary>
public static class TranspositionHelper
{
    /// <summary>
    /// 补充字符
    /// </summary>
    public static char PaddingChar { get; set; } = '`';

    /// <summary>
    /// 字符不足时使用的补充字符
    /// </summary>
    public static PaddingMode PaddingMode { get; set; } = PaddingMode.SingleChar;

    /// <summary>
    /// 获得补充字符
    /// </summary>
    public static char GetPaddingChar()
    {
        return PaddingMode switch
        {
            PaddingMode.SingleChar => PaddingChar,
            PaddingMode.Digit => (char)('0' + Random.Shared.Next(10)),
            PaddingMode.LowerLetter => (char)('a' + Random.Shared.Next(26)),
            PaddingMode.UpperLetter => (char)('A' + Random.Shared.Next(26)),
            _ => ' ',
        };
    }

    /// <summary>
    /// 填充顺序
    /// </summary>
    /// <param name="order">顺序数组</param>
    public static ushort[] FillOrder(this ushort[] order)
    {
        for (ushort i = 0; i < order.Length; i++)
            order[i] = i;
        return order;
    }

    /// <summary>
    /// 填充顺序
    /// </summary>
    public static List<ushort> FillOrder(this List<ushort> order, int count)
    {
        for (ushort i = 0; i < count; i++)
            order.Add(i);
        return order;
    }

    /// <summary>
    /// 填充二维数组的顺序(按行填充)
    /// </summary>
    /// <param name="order">顺序数组</param>
    public static ushort[,] FillOrderByRow(this ushort[,] order)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                order[x, y] = (ushort)(x + y * width);
        return order;
    }

    /// <summary>
    /// 填充二维数组的顺序(按列填充)
    /// </summary>
    /// <param name="order">顺序数组</param>
    public static ushort[,] FillOrderByColumn(this ushort[,] order)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                order[x, y] = (ushort)(x * height + y);
        return order;
    }

    /// <summary>
    /// 依据顺序拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    [SkipLocalsInit]
    public static string AssembleText(this ushort[] order, string text)
    {
        int length = order.Length;
        using var memory = length.TryAllocString();
        Span<char> buffer = length.CanAllocString() ? stackalloc char[length] : memory.Span;

        for (int i = 0; i < length; i++)
            buffer[i] = order[i] < text.Length ? text[order[i]] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据逆向顺序拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    [SkipLocalsInit]
    public static string AssembleTextInverse(this ushort[] order, string text)
    {
        int length = order.Length;
        using var memory = length.TryAllocString();
        Span<char> buffer = length.CanAllocString() ? stackalloc char[length] : memory.Span;

        for (int i = 0; i < length; i++)
            buffer[order[i]] = i < text.Length ? text[i] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据顺序按行拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    [SkipLocalsInit]
    public static string AssembleTextByRow(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        int size = width * height;
        using var memory = size.TryAllocString();
        Span<char> buffer = size.CanAllocString() ? stackalloc char[size] : memory.Span;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[x + y * width] = order[x, y] < text.Length ?
                    text[order[x, y]] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据逆向顺序按行拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    [SkipLocalsInit]
    public static string AssembleTextByRowInverse(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        int size = width * height;
        using var memory = size.TryAllocString();
        Span<char> buffer = size.CanAllocString() ? stackalloc char[size] : memory.Span;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[order[x, y]] = x + y * width < text.Length ?
                        text[x + y * width] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据顺序按列拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    [SkipLocalsInit]
    public static string AssembleTextByColumn(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        int size = width * height;
        using var memory = size.TryAllocString();
        Span<char> buffer = size.CanAllocString() ? stackalloc char[size] : memory.Span;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[x * height + y] = order[x, y] < text.Length ?
                    text[order[x, y]] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据逆向顺序按列拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    [SkipLocalsInit]
    public static string AssembleTextByColumnInverse(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        int size = width * height;
        using var memory = size.TryAllocString();
        Span<char> buffer = size.CanAllocString() ? stackalloc char[size] : memory.Span;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[order[x, y]] = x * height + y < text.Length ?
                        text[x * height + y] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 获得周期
    /// </summary>
    /// <param name="order">顺序</param>
    public static int GetPeriod(ushort[] order)
    {
        var cycles = new List<int>();
        var visited = new BitArray(order.Length);
        for (int i = 0; i < order.Length; i++)
        {
            if (visited[i])
                continue;
            int cycle = 0;
            int next = i;
            do
            {
                visited[next] = true;
                next = order[next];
                cycle++;
            }
            while (next != i);
            cycles.Add(cycle);
        }

        return LCM(cycles.ToArray().AsSpan());
    }

    /// <summary>
    /// 获得周期
    /// </summary>
    /// <param name="order">顺序</param>
    public static int GetPeriod(ushort[,] order)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        var cycles = new List<int>();
        var visited = new BitArray(order.Length);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cur = x * height + y;
                if (visited[cur])
                    continue;
                int cycle = 0;
                (int nx, int ny) = (x, y);
                ushort next;
                do
                {
                    visited[nx * height + ny] = true;
                    next = order[nx, ny];
                    nx = next / height;
                    ny = next % height;
                    cycle++;
                }
                while (next != cur);
                cycles.Add(cycle);
            }
        }
        return LCM(cycles.ToArray().AsSpan());
    }

    /// <summary>
    /// 多重置换
    /// </summary>
    /// <param name="order">顺序</param>
    /// <param name="n">次数</param>
    public static ushort[] MultiTranspose(this ushort[] order, int n)
    {
        if (n == 1)
            return order;
        int p = GetPeriod(order);
        if (p == 1)
            return order;
        n %= p;
        ushort[] result = new ushort[order.Length];
        result.FillOrder();
        ushort[] temp = new ushort[order.Length];
        while (n != 0)
        {
            if ((n & 1) == 1)
            {
                for (int i = 0; i < result.Length; i++)
                    temp[i] = result[order[i]];
                (temp, result) = (result, temp);
            }
            if (n == 1)
                break;
            for (int i = 0; i < order.Length; i++)
                temp[i] = order[order[i]];
            (temp, order) = (order, temp);
            n >>= 1;
        }
        return result;
    }

    /// <summary>
    /// 多重置换(二维)
    /// </summary>
    /// <param name="order">顺序</param>
    /// <param name="n">次数</param>
    /// <param name="byColumn">按列</param>
    public static ushort[,] MultiTranspose(this ushort[,] order, int n, bool byColumn = false)
    {
        if (n == 1)
            return order;
        int p = GetPeriod(order);
        if (p == 1)
            return order;
        n %= p;
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        ushort[,] result = new ushort[width, height];
        result.FillOrderByRow();
        ushort[,] temp = new ushort[width, height];
        while (n != 0)
        {
            if ((n & 1) == 1)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var t = order[x, y];
                        temp[x, y] = result[t % width, t / width];
                    }
                }
                (temp, result) = (result, temp);
            }
            if (n == 1)
                break;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var t = order[x, y];
                    temp[x, y] = byColumn ?
                        order[t / height, t % height] :
                        order[t % width, t / width];
                }
            }
            (temp, order) = (order, temp);
            n >>= 1;
        }
        return result;
    }

    /// <summary>
    /// 最小公倍数
    /// </summary>
    /// <param name="nums"></param>
    public static int LCM(Span<int> nums)
    {
        if (nums.Length == 1)
            return nums[0];
        if (nums.Length == 2)
            return LCM(nums[0], nums[1]);
        int mid = nums.Length / 2;
        return LCM(LCM(nums[..mid]), LCM(nums[mid..]));
    }

    /// <summary>
    /// 最大公约数
    /// </summary>
    public static int GCD(int n, int m)
    {
        while (m != n)
        {
            if (m > n)
                m -= n;
            else
                n -= m;
        }
        return m;
    }

    /// <summary>
    /// 最小公倍数
    /// </summary>
    public static int LCM(int n, int m) => n * m / GCD(n, m);
}
