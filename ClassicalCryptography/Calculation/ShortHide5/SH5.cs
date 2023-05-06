using System.Collections;
using System.Runtime.InteropServices;
using static ClassicalCryptography.Utils.BaseConverter;

namespace ClassicalCryptography.Calculation.ShortHide5;

/// <summary>
/// ShortHide5密文结果的结构
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public readonly partial struct SH5 : IEnumerable<int>
{
    #region 只读字段

    /// <summary>
    /// <see href="https://www.bilibili.com/read/cv15676311">标准的1组SH5推荐字母表</see>
    /// </summary>
    /// <remarks>
    /// <c>EADIHTNORFS</c>
    /// </remarks>
    public static readonly string AlphaBetSingle = "EADIHTNORFS";

    /// <summary>
    /// 2组SH5的字母表
    /// </summary>
    /// <remarks>
    /// <c>XABCDEFGHIJKLMNOPQRSTUVWYZ</c>
    /// </remarks>
    public static readonly string AlphaBetDouble = "XABCDEFGHIJKLMNOPQRSTUVWYZ";

    /// <summary>
    /// 3组SH5的字母表(虽然我觉得这不够好，多几个标点符号又何妨，但我没有更改它)
    /// </summary>
    /// <remarks>
    /// <c>[空格]ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,</c>
    /// </remarks>
    public static readonly string AlphaBetTriple = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,";

    /// <summary>
    /// 第一组的大写字母值U和乘数V
    /// </summary>
    [FieldOffset(0)]
    public readonly (int U, ulong_long V) Pair1;

    /// <summary>
    /// 第二组的大写字母值U和乘数V
    /// </summary>
    [FieldOffset(0x20)]
    public readonly (int U, ulong_long V) Pair2;

    /// <summary>
    /// 第三组的大写字母值U和乘数V
    /// </summary>
    [FieldOffset(0x40)]
    public readonly (int U, ulong_long V) Pair3;

    /// <summary>
    /// 表示结构的等级
    /// </summary>
    [FieldOffset(8)]
    public readonly SH5Level Level;

    /// <summary>
    /// 前缀X的数量(2组SH5独有的)
    /// </summary>
    [FieldOffset(0x28)]
    public readonly int PrefixCount;

    /// <summary>
    /// 是否排序字母值
    /// </summary>
    [FieldOffset(0x48)]
    public readonly bool SortPair;

    #endregion 只读字段

    /// <summary>
    /// 一组的SH5结构
    /// </summary>
    public SH5(int u, ulong_long v)
    {
        Pair1 = (u, v);
        Level = SH5Level.Single;
    }

    /// <summary>
    /// 获得2组SH5的前缀X
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetPrefix() => PrefixCount switch
    {
        0 => string.Empty,
        1 => "X",
        2 => "XX",
        3 => "XXX",
        4 => "XXXX",
        5 => "XXXXX",
        _ => 'X'.Repeat(PrefixCount),
    };

    /// <summary>
    /// 两组的SH5结构
    /// </summary>
    public SH5(int u1, ulong_long v1, int u2, ulong_long v2, int prefixCount = 0, bool sortPair = false)
    {
        Pair1 = (u1, v1);
        Pair2 = (u2, v2);
        if (sortPair && u1 > u2)
            (Pair1, Pair2) = (Pair2, Pair1);
        Level = SH5Level.Double;
        PrefixCount = prefixCount;
        SortPair = sortPair;
    }

    /// <summary>
    /// 三组的SH5结构
    /// </summary>
    public SH5(int u1, ulong_long v1, int u2, ulong_long v2, int u3, ulong_long v3, bool sortPair = false)
    {
        Pair1 = (u1, v1);
        Pair2 = (u2, v2);
        Pair3 = (u3, v3);
        if (sortPair)
        {
            if (Pair1.U > Pair2.U)
                (Pair1, Pair2) = (Pair2, Pair1);
            if (Pair1.U > Pair3.U)
                (Pair1, Pair3) = (Pair3, Pair1);
            if (Pair2.U > Pair3.U)
                (Pair2, Pair3) = (Pair3, Pair2);
        }
        Level = SH5Level.Trible;
        SortPair = sortPair;
    }

    [GeneratedRegex("[A-Z][0-9a-z]+")]
    private static partial Regex SH5GroupRegex();

    /// <summary>
    /// 表示一个SH5结构
    /// </summary>
    /// <remarks>
    /// 一个正确的SH5结构应该符合如下的正则表达式<br/>
    /// <c>"X*([A-Z][0-9a-z]+){1,3}"</c>
    /// </remarks>
    public SH5(string SH5Patten, bool sortPair = false)
    {
        var matches = SH5GroupRegex().Matches(SH5Patten);
        var level = (SH5Level)matches.Count;
        GuardEx.IsDefined(level);

        switch (level)
        {
            case SH5Level.Single:
                var value = matches[0].ValueSpan;
                Pair1 = (value[0].LetterNumber(), FromBase36(value[1..]));
                break;
            case SH5Level.Double:
                value = matches[0].ValueSpan;
                Pair1 = (DoubleAlphaBet(value[0]), FromBase36(value[1..]));
                value = matches[1].ValueSpan;
                Pair2 = (DoubleAlphaBet(value[0]), FromBase36(value[1..]));
                if (sortPair && Pair1.U > Pair2.U)
                    (Pair1, Pair2) = (Pair2, Pair1);
                PrefixCount = GetPrefixCount(SH5Patten);
                break;
            case SH5Level.Trible:
                value = matches[0].ValueSpan;
                Pair1 = (value[0].LetterNumber(), FromBase36(value[1..]));
                value = matches[1].ValueSpan;
                Pair2 = (value[0].LetterNumber(), FromBase36(value[1..]));
                value = matches[2].ValueSpan;
                Pair3 = (value[0].LetterNumber(), FromBase36(value[1..]));
                if (sortPair)
                {
                    if (Pair1.U > Pair2.U)
                        (Pair1, Pair2) = (Pair2, Pair1);
                    if (Pair1.U > Pair3.U)
                        (Pair1, Pair3) = (Pair3, Pair1);
                    if (Pair2.U > Pair3.U)
                        (Pair2, Pair3) = (Pair3, Pair2);
                }
                break;
        }
        Level = level;
        SortPair = sortPair;
    }

    /// <inheritdoc cref="SH5(string, bool)"/>
    public static SH5 Parse(string SH5Patten) => new(SH5Patten);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int DoubleAlphaBet(char character) => character switch
    {
        'X' => 0,
        <= 'W' => character - 'A' + 1,
        <= 'Z' => character - 'A',
        _ => throw new ArgumentException("字符不在2组SH5的字母表中", nameof(character))
    };

    /// <summary>
    /// 获得前缀X数量
    /// </summary>
    public static int GetPrefixCount(string word)
    {
        int count = 0;
        while (count < word.Length && word[count] is 'X' or 'x')
            count++;
        return count;
    }

    /// <summary>
    /// 当前SH5结构的字母表
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetAlphaBet() => Level switch
    {
        SH5Level.Single => AlphaBetSingle,
        SH5Level.Double => AlphaBetDouble,
        SH5Level.Trible => AlphaBetTriple,
        _ => throw new Exception("不存在的字母表"),
    };

    /// <summary>
    /// SH5结构的字符串形式
    /// </summary>
    public override string ToString() => Level switch
    {
        SH5Level.Single => $"{U_Letters[Pair1.U - 1]}{ToBase36(Pair1.V)}",
        SH5Level.Double => $"{GetPrefix()}{AlphaBetDouble[Pair1.U]}{ToBase36(Pair1.V)}{AlphaBetDouble[Pair2.U]}{ToBase36(Pair2.V)}",
        SH5Level.Trible => $"{U_Letters[Pair1.U - 1]}{ToBase36(Pair1.V)}{U_Letters[Pair2.U - 1]}{ToBase36(Pair2.V)}{U_Letters[Pair3.U - 1]}{ToBase36(Pair3.V)}",
        _ => string.Empty
    };

    /// <summary>
    /// 倒序计算SH5的结果
    /// </summary>
    public IEnumerator<int> GetEnumerator()
    {
        var (u1, v1) = Pair1;
        var (u2, v2) = Pair2;
        var (u3, v3) = Pair3;
        switch (Level)
        {
            case SH5Level.Single:
                while (v1 != 0)
                {
                    //额外的计算规则在 https://www.bilibili.com/read/cv15676311 中有解释
                    yield return u1 * (int)(v1 % 5) + u1 / 11;
                    v1 /= 5;
                }
                yield break;
            case SH5Level.Double:
                while ((v1, v2) != (0, 0))
                {
                    yield return u1 * (int)(v1 % 5) + u2 * (int)(v2 % 5);
                    v1 /= 5; v2 /= 5;
                }
                yield break;
            case SH5Level.Trible:
                while ((v1, v2, v3) != (0, 0, 0))
                {
                    yield return u1 * (int)(v1 % 5) + u2 * (int)(v2 % 5) + u3 * (int)(v3 % 5);
                    v1 /= 5; v2 /= 5; v3 /= 5;
                }
                yield break;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    public override int GetHashCode() => Level switch
    {
        SH5Level.Single => HashCode.Combine(Level, Pair1),
        SH5Level.Double => HashCode.Combine(Level, Pair1, Pair2, PrefixCount),
        SH5Level.Trible => HashCode.Combine(Level, Pair1, Pair2, Pair3),
        _ => 0
    };
}
