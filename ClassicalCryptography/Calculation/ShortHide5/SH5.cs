using ClassicalCryptography.Utils;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using static ClassicalCryptography.Utils.BaseConverter;
using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Calculation.ShortHide5;

/// <summary>
/// ShortHide5密文结果的结构
/// </summary>
public partial class SH5 : IEnumerable<int>
{
    /// <summary>
    /// <para>1组SH5的字母表(推荐的)</para>
    /// <see href="https://www.bilibili.com/read/cv15676311"/>
    /// </summary>
    public static readonly string AlphaBetS = "EADIHTNORFS";
    /// <summary>
    /// 2组SH5的字母表
    /// </summary>
    public static readonly string AlphaBetD = "XABCDEFGHIJKLMNOPQRSTUVWYZ";
    /// <summary>
    /// 3组SH5的字母表
    /// </summary>
    public static readonly string AlphaBetT = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,";

    /// <summary>
    /// 第一组的大写字母值 <see href="U"/> 和乘数 <see href="V"/>
    /// </summary>
    public readonly (int U, ulong V) Pair1;
    /// <summary>
    /// 第二组的大写字母值 <see href="U"/> 和乘数 <see href="V"/>
    /// </summary>
    public readonly (int U, ulong V) Pair2;
    /// <summary>
    /// 第三组的大写字母值 <see href="U"/> 和乘数 <see href="V"/>
    /// </summary>
    public readonly (int U, ulong V) Pair3;

    /// <summary>
    /// 等级
    /// </summary>
    public readonly SH5Level Level;

    /// <summary>
    /// 当前SH5结构的字母表
    /// </summary>
    public string GetAlphaBet() => Level switch
    {
        SH5Level.Single => AlphaBetS,
        SH5Level.Double => AlphaBetD,
        SH5Level.Trible => AlphaBetT,
        _ => throw new Exception("不存在的字母表"),
    };

    /// <summary>
    /// 前缀X的数量
    /// </summary>
    public readonly int PrefixCount;

    /// <summary>
    /// 前缀X
    /// </summary>
    public string Prefix => PrefixCount switch
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
    /// 获得前缀X数量
    /// </summary>
    public static int GetPrefixCount(string str)
    {
        int count = 0;
        while (str[count] is 'X' or 'x' && count < str.Length)
            count++;
        return count;
    }


    /// <summary>
    /// 仅一组的SH5结构
    /// </summary>
    public SH5(int u, ulong v)
    {
        Pair1.U = u;
        Pair1.V = v;
        Level = SH5Level.Single;
    }

    /// <summary>
    /// 两组的SH5结构
    /// </summary>
    public SH5(int u1, ulong v1, int u2, ulong v2, int prefixCount = 0)
    {
        Pair1.U = u1;
        Pair1.V = v1;
        Pair2.U = u2;
        Pair2.V = v2;
        Level = SH5Level.Double;
        PrefixCount = prefixCount;
    }

    /// <summary>
    /// 三组的SH5结构
    /// </summary>
    public SH5(int u1, ulong v1, int u2, ulong v2, int u3, ulong v3)
    {
        Pair1.U = u1;
        Pair1.V = v1;
        Pair2.U = u2;
        Pair2.V = v2;
        Pair3.U = u3;
        Pair3.V = v3;
        Level = SH5Level.Trible;
    }

    [GeneratedRegex("[A-Z][0-9a-z]+")]
    private static partial Regex SH5GroupRegex();

    /// <summary>
    /// 通过字符串创建SH5结构
    /// </summary>
    public SH5(string str)
    {
        var matches = SH5GroupRegex().Matches(str);
        Level = (SH5Level)matches.Count;
        Guard.IsTrue(Enum.IsDefined(Level));

        ReadOnlySpan<char> vstr;
        switch (Level)
        {
            case SH5Level.Single:
                vstr = matches[0].ValueSpan;
                Pair1.U = vstr[0].LetterNumber() + 1;
                Pair1.V = FromBase36(vstr[1..]);
                break;
            case SH5Level.Double:
                vstr = matches[0].ValueSpan;
                Pair1.U = AlphaBetD.IndexOf(vstr[0]);
                Pair1.V = FromBase36(vstr[1..]);
                vstr = matches[1].ValueSpan;
                Pair2.U = AlphaBetD.IndexOf(vstr[0]);
                Pair2.V = FromBase36(vstr[1..]);
                PrefixCount = GetPrefixCount(str);
                break;
            case SH5Level.Trible:
                vstr = matches[0].ValueSpan;
                Pair1.U = vstr[0].LetterNumber() + 1;
                Pair1.V = FromBase36(vstr[1..]);
                vstr = matches[1].ValueSpan;
                Pair2.U = vstr[0].LetterNumber() + 1;
                Pair2.V = FromBase36(vstr[1..]);
                vstr = matches[2].ValueSpan;
                Pair3.U = vstr[0].LetterNumber() + 1;
                Pair3.V = FromBase36(vstr[1..]);
                break;
        }
    }

    /// <summary>
    /// 字符串形式
    /// </summary>
    public override string ToString()
    {
        var result = new StringBuilder();
        switch (Level)
        {
            case SH5Level.Single:
                result.Append(U_Letters[Pair1.U - 1]);
                result.Append(ToBase36(Pair1.V));
                break;
            case SH5Level.Double:
                result.Append(Prefix);
                result.Append(AlphaBetD[Pair1.U]);
                result.Append(ToBase36(Pair1.V));
                result.Append(AlphaBetD[Pair2.U]);
                result.Append(ToBase36(Pair2.V));
                break;
            case SH5Level.Trible:
                result.Append(U_Letters[Pair1.U - 1]);
                result.Append(ToBase36(Pair1.V));
                result.Append(U_Letters[Pair2.U - 1]);
                result.Append(ToBase36(Pair2.V));
                result.Append(U_Letters[Pair3.U - 1]);
                result.Append(ToBase36(Pair3.V));
                break;
        }
        return result.ToString();
    }

    /// <summary>
    /// 倒序计算的值
    /// </summary>
    public IEnumerator<int> GetEnumerator()
    {
        ulong v1, v2, v3;

        switch (Level)
        {
            case SH5Level.Single:
                v1 = Pair1.V;
                while (v1 != 0)
                {
                    yield return Pair1.U * (int)(v1 % 5)
                        + Pair1.U / AlphaBetS.Length;//额外的规则
                    v1 /= 5;
                }
                yield break;
            case SH5Level.Double:
                v1 = Pair1.V;
                v2 = Pair2.V;
                while (v1 != 0 || v2 != 0)
                {
                    yield return Pair1.U * (int)(v1 % 5)
                        + Pair2.U * (int)(v2 % 5);
                    v1 /= 5;
                    v2 /= 5;
                }
                yield break;
            case SH5Level.Trible:
                v1 = Pair1.V;
                v2 = Pair2.V;
                v3 = Pair3.V;
                while (v1 != 0 || v2 != 0 || v3 != 0)
                {
                    yield return Pair1.U * (int)(v1 % 5)
                        + Pair2.U * (int)(v2 % 5)
                        + Pair3.U * (int)(v3 % 5);
                    v1 /= 5;
                    v2 /= 5;
                    v3 /= 5;
                }
                yield break;
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
