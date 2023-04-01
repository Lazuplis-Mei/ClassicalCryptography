using CommunityToolkit.HighPerformance;
using System.Runtime.CompilerServices;
using static ClassicalCryptography.Calculation.ShortHide5.SH5;

namespace ClassicalCryptography.Calculation.ShortHide5;

/// <summary>
/// ShortHide5密码<br/>
/// <a href="https://www.bilibili.com/read/cv15660906">Standard Short Hide5(标准SH5)</a>
/// </summary>
[Introduction("ShortHide5密码", "一种自创的英文文本加密方法")]
public static class ShortHide5
{
    private static readonly string[] singleAlphaBets;
    private static readonly string[,] doubleAlphaBets;

    [SkipLocalsInit]
    unsafe static ShortHide5()
    {
        Span2D<char> span2D = stackalloc char[25].AsSpan2D(5, 5);
        Span<char> span = span2D.GetRowSpan(0);

        singleAlphaBets = new string[26];
        for (int u = 1; u <= 26; u++)
        {
            for (int i = 0; i < 5; i++)
                span[i] = AlphaBetSingle[(u * i + u / 11) % 11];
            singleAlphaBets[u - 1] = new(span);
        }

        doubleAlphaBets = new string[25, 25];
        for (int u1 = 1; u1 <= 25; u1++)
        {
            for (int u2 = 1; u2 <= 25; u2++)
            {
                for (int i = 0; i < 5; i++) for (int j = 0; j < 5; j++)
                        span2D[i, j] = AlphaBetDouble[(u1 * i + u2 * j) % 26];
                doubleAlphaBets[u1 - 1, u2 - 1] = new(span2D.AsSpan());
            }
        }
    }

    /// <inheritdoc/>
    public static CipherType Type => CipherType.Calculation;

    /// <summary>
    /// 解密SH5结构的内容
    /// </summary>
    /// <param name="cipher">SH5结构</param>
    /// <returns>文本内容</returns>
    [SkipLocalsInit]
    public static string DecryptSH5(SH5 cipher)
    {
        var alphaBet = cipher.GetAlphaBet();
        int index = 28;
        Span<char> text = stackalloc char[index];
        foreach (var value in cipher)
            text[--index] = alphaBet[value % alphaBet.Length];
        if (cipher.Level != SH5Level.Double)
            return new(text[index..]);
        return $"{cipher.Prefix}{text[index..]}";
    }

    /// <inheritdoc/>
    public static string Decrypt(string cipherText) => DecryptSH5(new(cipherText));

    /// <summary>
    /// 加密指定的内容
    /// </summary>
    /// <remarks>
    /// <paramref name="plainText"/>应在<see cref="AlphaBetTriple"/>的范围中。<br/>
    /// 加密的结果是随机选择的。
    /// </remarks>
    /// <param name="plainText">明文文本</param>
    /// <returns>对应的<see cref="SH5"/>结构</returns>
    public static SH5 EncryptSH5(string plainText)
    {
        Guard.HasSizeLessThanOrEqualTo(plainText, GetPrefixCount(plainText) + 28);

        var results = EncryptSingles(plainText);
        if (results.Length > 0)
            return results.RandomItem();

        results = EncryptDoubles(plainText, 20);
        if (results.Length > 0)
            return results.RandomItem();

        return EncryptTrible(plainText, plainText.ToHashSet());
    }

    /// <inheritdoc/>
    public static string Encrypt(string plainText) => EncryptSH5(plainText).ToString();

    /// <summary>
    /// 随机的3组加密
    /// </summary>
    /// <remarks>
    /// 重点：方法最多尝试2倍于<paramref name="count"/>的次数。<br/>
    /// 所以结果的数量有可能会小于<paramref name="count"/>。
    /// </remarks>
    /// <param name="plainText">要加密的内容</param>
    /// <param name="count">要加密的数量</param>
    public static SH5[] EncryptTribles(string plainText, int count = 100)
    {
        var result = new HashSet<SH5>();
        var characterSet = plainText.ToHashSet();
        int repeatCount = 0;
        while (characterSet.Count < count && repeatCount <= (count << 1))
        {
            result.Add(EncryptTrible(plainText, characterSet));
            repeatCount++;
        }
        return result.ToArray();
    }

    private static SH5 EncryptTrible(string plainText, HashSet<char> characterSet)
    {
        Guard.IsTrue(characterSet.IsSubsetOf(AlphaBetTriple));

        char[] alphaBet = new char[125];
        int u1 = 0, u2 = 0, u3 = 0;
        while (!characterSet.IsSubsetOf(alphaBet))
        {
            u1 = Random.Shared.Next(26) + 1;
            u2 = Random.Shared.Next(26) + 1;
            u3 = Random.Shared.Next(26) + 1;
            int index = 0;
            for (int i = 0; i < 5; i++) for (int j = 0; j < 5; j++) for (int k = 0; k < 5; k++)
                        alphaBet[index++] = AlphaBetTriple[(i * u1 + j * u2 + k * u3) % 64];
        }

        ulong v1 = 0, v2 = 0, v3 = 0;
        ulong @base = 1;
        for (int i = plainText.Length - 1; i >= 0; i--)
        {
            checked
            {
                ulong value = (ulong)alphaBet.FindAll(plainText[i]).RandomItem();
                v1 += value / 25 * @base;
                v2 += (value % 25) / 5 * @base;
                v3 += value % 5 * @base;
                @base += @base << 2;
            }
        }
        return new(u1, v1, u2, v2, u3, v3);
    }

    /// <summary>
    /// 随机的2组加密
    /// </summary>
    /// <remarks>
    /// 重点：方法最多尝试2倍于<paramref name="count"/>的次数。<br/>
    /// 所以结果的数量有可能会小于<paramref name="count"/>。
    /// </remarks>
    /// <param name="word">要加密的内容(必须为纯英文字母)</param>
    /// <param name="count">要加密的数量</param>
    public static SH5[] EncryptDoubles(string word, int count = 100)
    {
        var result = new HashSet<SH5>();
        int prefixCount = GetPrefixCount(word);
        if (prefixCount == word.Length)
            return Array.Empty<SH5>();

        word = word[prefixCount..].ToUpper();

        var characterSet = word.ToHashSet();
        if (characterSet.Count > 25)
            return Array.Empty<SH5>();

        var list = new List<(int U1, int U2)>();
        for (int u1 = 0; u1 < 25; u1++)
        {
            for (int u2 = 0; u2 < 25; u2++)
            {
                if (characterSet.IsSubsetOf(doubleAlphaBets[u1, u2]))
                    list.Add((u1 + 1, u2 + 1));
            }
        }
        if (list.Count == 0)
            return Array.Empty<SH5>();

        int repeatCount = 0;
        while (result.Count < count && repeatCount <= (count << 1))
        {
            (int u1, int u2) = list.RandomItem();

            ulong v1 = 0, v2 = 0;
            ulong @base = 1;
            var alphaBet = doubleAlphaBets[u1 - 1, u2 - 1];
            for (int i = word.Length - 1; i >= 0; i--)
            {
                checked
                {
                    var value = alphaBet.FindAll(word[i]).RandomItem();
                    v1 += (ulong)(value / 5) * @base;
                    v2 += (ulong)(value % 5) * @base;
                    @base += @base << 2;
                }
            }
            result.Add(new(u1, v1, u2, v2, prefixCount));
            repeatCount++;
        }
        return result.ToArray();
    }

    /// <summary>
    /// 1组加密
    /// </summary>
    /// <param name="word">特定的单词</param>
    /// <returns>所有可能的结果</returns>
    public static SH5[] EncryptSingles(string word)
    {
        var result = new HashSet<SH5>();
        word = word.ToUpper();
        var characterSet = word.ToHashSet();
        if (characterSet.Count > 5)
            return Array.Empty<SH5>();

        var list = new List<int>();
        for (int u1 = 0; u1 < singleAlphaBets.Length; u1++)
        {
            if (characterSet.IsSubsetOf(singleAlphaBets[u1]))
                list.Add(u1);
        }

        for (int j = 0; j < list.Count; j++)
        {
            int u1 = list[j];
            if (singleAlphaBets[u1][0] != word[0])
            {
                ulong v1 = 0;
                ulong @base = 1;
                for (int i = word.Length - 1; i >= 0; i--)
                {
                    checked
                    {
                        v1 += (ulong)singleAlphaBets[u1].IndexOf(word[i]) * @base;
                        @base += @base << 2;
                    }
                }
                result.Add(new(u1 + 1, v1));
            }
        }
        return result.ToArray();
    }
}
