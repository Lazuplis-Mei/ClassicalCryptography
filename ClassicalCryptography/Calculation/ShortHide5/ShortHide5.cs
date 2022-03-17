using ClassicalCryptography.Interfaces;
using static ClassicalCryptography.Calculation.ShortHide5.SH5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Calculation.ShortHide5;

/// <summary>
/// ShortHide5密码
/// </summary>
[Introduction("ShortHide5密码", "一种自创的英文文本加密方法。")]
public class ShortHide5 : ICipher<string, string>
{
    private static readonly string[] sGroups;//26
    private static readonly string[,] dGroups;//25*25
    static ShortHide5()
    {
        Span<char> span = stackalloc char[25];

        sGroups = new string[26];
        int slen = AlphaBetS.Length;
        for (int u1 = 1; u1 <= 26; u1++)
        {
            for (int i = 0; i < 5; i++)
                span[i] = AlphaBetS[(u1 * i + u1 / slen) % slen];
            sGroups[u1 - 1] = new(span[..5]);
        }

        dGroups = new string[25, 25];
        slen = AlphaBetD.Length;

        for (int u1 = 1; u1 <= 25; u1++)
        {
            for (int u2 = 1; u2 <= 25; u2++)
            {
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        span[i + (i << 2) + j] = AlphaBetD[(u1 * i + u2 * j) % slen];
                dGroups[u1 - 1, u2 - 1] = new(span);
            }
        }
    }
    /// <summary>
    /// 密码类型
    /// </summary>
    public CipherType Type => CipherType.Calculation;

    /// <summary>
    /// 解密指定的内容
    /// </summary>
    /// <param name="cipher"></param>
    public static string DecryptSH5(SH5 cipher)
    {
        var alphaBet = cipher.GetAlphaBet();
        int index = 28;//最大加密长度
        Span<char> text = stackalloc char[index];
        foreach (var value in cipher)
            text[--index] = alphaBet[value % alphaBet.Length];
        return $"{cipher.Prefix}{text[index..]}";
    }

    /// <summary>
    /// 解密指定的内容
    /// </summary>
    /// <param name="cipherText"></param>
    public string Decrypt(string cipherText)
    {
        return DecryptSH5(new(cipherText));
    }

    /// <summary>
    /// 加密指定的内容
    /// </summary>
    /// <param name="plainText">纯英文单词</param>
    public static SH5 EncryptSH5(string plainText)
    {
        if (plainText.Length - GetPrefixCount(plainText) > 28)
            throw new ArgumentException("字符串太长", nameof(plainText));
        var results = EncryptSingles(plainText);
        if (results.Count > 0)
            return results.Count == 1 ? results[0] : results.RandomItem();

        results = EncryptDoubles(plainText);
        if (results.Count > 0)
            return results.RandomItem();

        return EncryptTrible(plainText, plainText.ToHashSet());

    }

    /// <summary>
    /// 加密指定的内容
    /// </summary>
    /// <param name="plainText">纯英文单词</param>
    public string Encrypt(string plainText)
    {
        return EncryptSH5(plainText).ToString();
    }

    /// <summary>
    /// 随机的3组加密
    /// </summary>
    /// <param name="plainText">要加密的内容</param>
    /// <param name="count">要加密的数量(可能有部分重复项)</param>
    public static List<SH5> EncryptTribles(string plainText, int count = 100)
    {
        var result = new List<SH5>();
        var set = plainText.ToHashSet();
        for (int i = 0; i < count; i++)
            result.Add(EncryptTrible(plainText, set));
        return result;
    }

    private static SH5 EncryptTrible(string plainText, HashSet<char> set)
    {
        if (set.IsSubsetOf(AlphaBetT))
        {
            char[] group = new char[125];
            int u1 = 0, u2 = 0, u3 = 0;
            while (!set.IsSubsetOf(group))
            {
                u1 = Random.Shared.Next(26) + 1;
                u2 = Random.Shared.Next(26) + 1;
                u3 = Random.Shared.Next(26) + 1;
                int p = 0;
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        for (int k = 0; k < 5; k++)
                            group[p++] = AlphaBetT[(i * u1 + j * u2 + k * u3) % 64];
            }

            ulong v1 = 0, v2 = 0, v3 = 0;
            ulong b = 1;
            for (int i = plainText.Length - 1; i >= 0; i--)
            {
                checked
                {
                    var t = group.FindAll(plainText[i]).RandomItem();
                    v1 += (ulong)(t / 25) * b;
                    v2 += (ulong)((t % 25) / 5) * b;
                    v3 += (ulong)(t % 5) * b;

                    b += b << 2;//base *= 5;
                }
            }
            return new(u1, v1, u2, v2, u3, v3);
        }

        throw new ArgumentOutOfRangeException(nameof(plainText), "无法加密指定的内容");
    }

    /// <summary>
    /// 随机的2组加密
    /// </summary>
    /// <param name="uText">要加密的内容</param>
    /// <param name="count">要加密的数量(可能有部分重复项)</param>
    /// <param name="minimize">最小化处理(可消除重复项,但结果大大减少)</param>
    public static List<SH5> EncryptDoubles(string uText, int count = 100, bool minimize = false)
    {
        var result = new List<SH5>();
        int prefixCount = GetPrefixCount(uText);
        uText = uText[prefixCount..].ToUpper();

        var set = uText.ToHashSet();
        if (set.Count > 25)
            return result;

        var list = new List<(int U1, int U2)>();
        for (int u1 = 0; u1 < 25; u1++)
        {
            for (int u2 = 0; u2 < 25; u2++)
            {
                if (set.IsSubsetOf(dGroups[u1, u2]))
                    list.Add((u1 + 1, u2 + 1));
            }
        }
        if (list.Count == 0)
            return result;

        if (minimize)
            count = Math.Min(count, list.Count);

        for (int j = 0; j < count; j++)
        {
            (int u1, int u2) = minimize ? list.PopRandomItem() : list.RandomItem();

            ulong v1 = 0, v2 = 0;
            ulong b = 1;
            var group = dGroups[u1 - 1, u2 - 1];
            for (int i = uText.Length - 1; i >= 0; i--)
            {
                checked
                {
                    var t = group.FindAll(uText[i]).RandomItem();
                    v1 += (ulong)(t / 5) * b;
                    v2 += (ulong)(t % 5) * b;

                    b += b << 2;//base *= 5;
                }
            }
            result.Add(new(u1, v1, u2, v2, prefixCount));
        }
        return result;
    }

    /// <summary>
    /// 1组加密
    /// </summary>
    public static List<SH5> EncryptSingles(string uText)
    {
        var result = new List<SH5>();
        uText = uText.ToUpper();
        var set = uText.ToHashSet();
        if (set.Count > 5)
            return result;

        var list = new List<int>();
        for (int u1 = 0; u1 < sGroups.Length; u1++)
        {
            if (set.IsSubsetOf(sGroups[u1]))
                list.Add(u1);
        }

        for (int j = 0; j < list.Count; j++)
        {
            int u1 = list[j];
            if (sGroups[u1][0] != uText[0])
            {
                ulong v1 = 0;
                ulong b = 1;
                for (int i = uText.Length - 1; i >= 0; i--)
                {
                    checked
                    {
                        v1 += (ulong)sGroups[u1].IndexOf(uText[i]) * b;
                        b += b << 2;//base *= 5;
                    }
                }
                result.Add(new(u1 + 1, v1));
            }
        }
        return result;
    }

}
