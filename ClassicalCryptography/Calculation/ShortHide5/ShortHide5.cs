using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using static ClassicalCryptography.Calculation.ShortHide5.SH5;

namespace ClassicalCryptography.Calculation.ShortHide5;

/// <summary>
/// ShortHide5密码
/// </summary>
/// <remarks>
/// 参考资料:<see href="https://www.bilibili.com/read/cv15660906">Standard Short Hide5(标准SH5)</see>
/// </remarks>
[Introduction("ShortHide5密码", "一种自创的英文文本加密方法")]
public static class ShortHide5
{
    /// <summary>
    /// 1组SH5所有可能的字母表
    /// </summary>
    public static readonly string[] SingleAlphaBets;

    /// <summary>
    /// 2组SH5所有可能的字母表
    /// </summary>
    public static readonly string[,] DoubleAlphaBets;

    /// <summary>
    /// 自定义算法
    /// </summary>
    public static CipherType Type => CipherType.Calculation;

    [SkipLocalsInit]
    static ShortHide5()
    {
        Span2D<char> span2D = stackalloc char[25].AsSpan2D(5, 5);
        Span<char> span = span2D.GetRowSpan(0);

        SingleAlphaBets = new string[26];
        for (int u = 1; u <= 26; u++)
        {
            //额外的计算规则在 https://www.bilibili.com/read/cv15676311 中有解释
            for (int i = 0; i < 5; i++)
                span[i] = AlphaBetSingle[(u * i + u / 11) % 11];
            SingleAlphaBets[u - 1] = new(span);
        }

        DoubleAlphaBets = new string[25, 25];
        for (int u1 = 1; u1 <= 25; u1++)
        {
            for (int u2 = 1; u2 <= 25; u2++)
            {
                for (int i = 0; i < 5; i++) for (int j = 0; j < 5; j++)
                        span2D[i, j] = AlphaBetDouble[(u1 * i + u2 * j) % 26];
                DoubleAlphaBets[u1 - 1, u2 - 1] = new(span2D.ToFlatSpan());
            }
        }
    }

    /// <summary>
    /// 解密SH5结构的内容
    /// </summary>
    /// <param name="cipher"><see cref="SH5"/>结构</param>
    /// <returns>文本内容</returns>
    [SkipLocalsInit]
    public static string DecryptSH5(this SH5 cipher)
    {
        var alphaBet = cipher.GetAlphaBet();
        //Log5(ulong_long.MaxValue) = 55.1
        int index = 55;
        Span<char> span = stackalloc char[index];
        foreach (var value in cipher)
            span[--index] = alphaBet[value % alphaBet.Length];
        if (cipher.Level != SH5Level.Double)
            return new(span[index..]);
        int count = cipher.PrefixCount;
        if (count <= index)
        {
            span = span[(index - count)..];
            span[..count].Fill('X');
            return new(span);
        }
        return $"{cipher.GetPrefix()}{span[index..]}";
    }

    /// <summary>
    /// 解密SH5结构的内容
    /// </summary>
    /// <param name="cipherText"><see cref="SH5"/>结构字符串</param>
    /// <returns>文本内容</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        //Log5(ulong_long.MaxValue) = 55.1
        Guard.HasSizeLessThanOrEqualTo(plainText, GetPrefixCount(plainText) + 55);

        var results = EncryptSingles(plainText);
        if (results.Length > 0)
            return results.RandomItem();

        results = EncryptDoubles(plainText);
        if (results.Length > 0)
            return results.RandomItem();

        return EncryptTrible(plainText);
    }

    /// <summary>
    /// 加密指定的内容
    /// </summary>
    /// <remarks>
    /// <paramref name="plainText"/>应在<see cref="AlphaBetTriple"/>的范围中。<br/>
    /// 加密的结果是随机选择的。
    /// </remarks>
    /// <param name="plainText">明文文本</param>
    /// <returns>对应的<see cref="SH5"/>结构字符串</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    public static SH5[] EncryptTribles(string plainText, int count = 20)
    {
        var characterSet = plainText.ToHashSet();
        GuardEx.IsSubsetOf(characterSet, AlphaBetTriple);

        var result = new HashSet<SH5>();
        int repeatCount = 0;
        while (characterSet.Count < count && repeatCount++ <= (count * 2))
            result.Add(EncryptTrible(plainText, characterSet));
        return result.ToArray();
    }

    /// <summary>
    /// 随机的3组加密
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SH5 EncryptTrible(string plainText)
    {
        var characterSet = plainText.ToHashSet();
        GuardEx.IsSubsetOf(characterSet, AlphaBetTriple);

        return EncryptTrible(plainText, characterSet);
    }

    private static SH5 EncryptTrible(string plainText, HashSet<char> characterSet)
    {
        using var memory = MemoryOwner<char>.Allocate(125);
        var alphaBet = memory.DangerousGetArray();
        int u1, u2, u3;
        do
        {
            u1 = Random.Shared.Next(1, 27);
            u2 = Random.Shared.Next(1, 27);
            u3 = Random.Shared.Next(1, 27);
            int index = 0;
            for (int i = 0; i < 5; i++) for (int j = 0; j < 5; j++) for (int k = 0; k < 5; k++)
                        alphaBet[index++] = AlphaBetTriple[(i * u1 + j * u2 + k * u3) % 64];
        }
        while (!characterSet.IsSubsetOf(alphaBet));

        ulong_long v1 = 0, v2 = 0, v3 = 0;
        foreach (char character in plainText)
        {
            var value = (ulong)alphaBet.FindAll(character).RandomItem();
            checked
            {
                v1 *= 5;
                v1 += value / 25;
                v2 *= 5;
                v2 += (value % 25) / 5;
                v3 *= 5;
                v3 += value % 5;
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
    public static SH5[] EncryptDoubles(string word, int count = 20)
    {
        int prefixCount = GetPrefixCount(word);
        if (prefixCount == word.Length)
            return Array.Empty<SH5>();

        word = word[prefixCount..].ToUpperAscii();

        var characterSet = word.ToHashSet();
        if (!characterSet.IsSubsetOf(AlphaBetDouble))
            return Array.Empty<SH5>();

        var list = new List<(int U1, int U2)>();
        for (int u1 = 0; u1 < 25; u1++) for (int u2 = 0; u2 < 25; u2++)
                if (characterSet.IsSubsetOf(DoubleAlphaBets[u1, u2]))
                    list.Add((u1 + 1, u2 + 1));

        if (list.Count == 0)
            return Array.Empty<SH5>();

        var result = new HashSet<SH5>();
        int repeatCount = 0;
        while (result.Count < count && repeatCount <= (count * 2))
        {
            (int u1, int u2) = list.RandomItem();

            ulong_long v1 = 0, v2 = 0;
            var alphaBet = DoubleAlphaBets[u1 - 1, u2 - 1];
            foreach (char character in word)
            {
                var value = (ulong)alphaBet.FindAll(character).RandomItem();
                checked
                {
                    v1 *= 5;
                    v1 += value / 5;
                    v2 *= 5;
                    v2 += value % 5;
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
        word = word.ToUpperAscii();
        var characterSet = word.ToHashSet();
        if (characterSet.Count > 5)
            return Array.Empty<SH5>();

        var result = new HashSet<SH5>();
        for (int u1 = 0; u1 < 26; u1++)
        {
            var alphaBet = SingleAlphaBets[u1];
            if (characterSet.IsSubsetOf(alphaBet))
            {
                if (alphaBet[0] != word[0])
                {
                    ulong_long v1 = 0;
                    foreach (char character in word)
                    {
                        checked
                        {
                            v1 *= 5;
                            v1 += (ulong)alphaBet.IndexOf(character);
                        }
                    }
                    result.Add(new(u1 + 1, v1));
                }
            }
        }
        return result.ToArray();
    }
}
