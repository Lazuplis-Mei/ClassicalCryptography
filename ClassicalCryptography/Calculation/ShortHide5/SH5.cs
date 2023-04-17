using System.Collections;
using System.Runtime.CompilerServices;
using static ClassicalCryptography.Utils.BaseConverter;
using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Calculation.ShortHide5;

/// <summary>
/// ShortHide5密文结果的结构
/// </summary>
public readonly partial struct SH5 : IEnumerable<int>
{
    #region 只读字段

    /// <summary>
    /// <see href="https://www.bilibili.com/read/cv15676311">标准的1组SH5推荐字母表</see>
    /// </summary>
    public static readonly string AlphaBetSingle = "EADIHTNORFS";

    /// <summary>
    /// 2组SH5的字母表
    /// </summary>
    public static readonly string AlphaBetDouble = "XABCDEFGHIJKLMNOPQRSTUVWYZ";

    /// <summary>
    /// 3组SH5的字母表
    /// </summary>
    public static readonly string AlphaBetTriple = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,";

    /// <summary>
    /// 第一组的大写字母值U和乘数V
    /// </summary>
    public readonly (int U, ulong V) Pair1;

    /// <summary>
    /// 第二组的大写字母值U和乘数V
    /// </summary>
    public readonly (int U, ulong V) Pair2;

    /// <summary>
    /// 第三组的大写字母值U和乘数V
    /// </summary>
    public readonly (int U, ulong V) Pair3;

    /// <summary>
    /// 表示结构的等级
    /// </summary>
    public readonly SH5Level Level;

    /// <summary>
    /// 前缀X的数量(2组SH5独有的)
    /// </summary>
    public readonly int PrefixCount;

    #endregion 只读字段

    /// <summary>
    /// 一组的SH5结构
    /// </summary>
    public SH5(int u, ulong v)
    {
        Pair1 = (u, v);
        Level = SH5Level.Single;
    }

    /// <summary>
    /// 2组SH5的前缀X
    /// </summary>
    public string Prefix
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => PrefixCount switch
        {
            0 => string.Empty,
            1 => "X",
            2 => "XX",
            3 => "XXX",
            4 => "XXXX",
            5 => "XXXXX",
            _ => 'X'.Repeat(PrefixCount),
        };
    }

    /// <summary>
    /// 两组的SH5结构
    /// </summary>
    public SH5(int u1, ulong v1, int u2, ulong v2, int prefixCount = 0)
    {
        Pair1 = (u1, v1);
        Pair2 = (u2, v2);
        Level = SH5Level.Double;
        PrefixCount = prefixCount;
    }

    /// <summary>
    /// 三组的SH5结构
    /// </summary>
    public SH5(int u1, ulong v1, int u2, ulong v2, int u3, ulong v3)
    {
        Pair1 = (u1, v1);
        Pair2 = (u2, v2);
        Pair3 = (u3, v3);
        Level = SH5Level.Trible;
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
    public SH5(string SH5Patten)
    {
        var matches = SH5GroupRegex().Matches(SH5Patten);
        Level = (SH5Level)matches.Count;
        Guard.IsTrue(Enum.IsDefined(Level));

        ReadOnlySpan<char> value;
        switch (Level)
        {
            case SH5Level.Single:
                value = matches[0].ValueSpan;
                Pair1 = (value[0].LetterNumber(), FromBase36(value[1..]));
                break;
            case SH5Level.Double:
                value = matches[0].ValueSpan;
                Pair1 = (DoubleAlphaBet(value[0]), FromBase36(value[1..]));
                value = matches[1].ValueSpan;
                Pair2 = (DoubleAlphaBet(value[0]), FromBase36(value[1..]));
                PrefixCount = GetPrefixCount(SH5Patten);
                break;
            case SH5Level.Trible:
                value = matches[0].ValueSpan;
                Pair1 = (value[0].LetterNumber(), FromBase36(value[1..]));
                value = matches[1].ValueSpan;
                Pair2 = (value[0].LetterNumber(), FromBase36(value[1..]));
                value = matches[2].ValueSpan;
                Pair3 = (value[0].LetterNumber(), FromBase36(value[1..]));
                break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int DoubleAlphaBet(char character) => character switch
        {
            'X' => 0,
            <= 'W' => character - 'A' + 1,
            <= 'Z' => character - 'A',
            _ => throw new ArgumentException("字符不在2组SH5的字母表中", nameof(character))
        };
    }

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
        SH5Level.Double => $"{Prefix}{AlphaBetDouble[Pair1.U]}{ToBase36(Pair1.V)}{AlphaBetDouble[Pair2.U]}{ToBase36(Pair2.V)}",
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
                    yield return u1 * (int)(v1 % 5) + u1 / AlphaBetSingle.Length;
                    v1 /= 5;
                }
                yield break;
            case SH5Level.Double:
                while (v1 != 0 || v2 != 0)
                {
                    yield return u1 * (int)(v1 % 5) + u2 * (int)(v2 % 5);
                    v1 /= 5; v2 /= 5;
                }
                yield break;
            case SH5Level.Trible:
                while (v1 != 0 || v2 != 0 || v3 != 0)
                {
                    yield return u1 * (int)(v1 % 5) + u2 * (int)(v2 % 5) + u3 * (int)(v3 % 5);
                    v1 /= 5; v2 /= 5; v3 /= 5;
                }
                yield break;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
