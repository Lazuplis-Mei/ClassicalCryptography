using System.Collections;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Calculation;

/// <summary>
/// 完美洗牌密码，天青留下的最后一个密码<br/>
/// </summary>
/// <remarks>
/// 对于字母表进行2种交替式的完美洗牌，取指定的首字母作为结果。<br/>
/// 在线工具：<a href="http://erikdemaine.org/fonts/shuffle/">erikdemaine/shuffle</a>
/// </remarks>
[Introduction("完美洗牌密码", "对于字母表进行2种交替式的完美洗牌，取指定的首字母作为结果")]
public static class PerfectShuffle
{

    private static readonly char[] separators = { ' ', ',', '.', ':', '"', '\'' };

    private const char INSIDE = '-';
    private const char OUTSIDE = '.';
    private const char CHARACTER_END = '/';
    private const char WORD_SEPARATOR = ' ';

    private static readonly BitArray[] shufflings = new BitArray[]
    {
        new(0),
        new(new[] { true, false, true, true }),
        new(new[] { false, true, true, false, true }),
        new(new[] { false, true, true }),
        new(new[] { true, true, false, true }),
        new(new[] { true, false, false, false, true }),
        new(new[] { true, true }),
        new(new[] { false, false, false, true, true }),
        new(new[] { false, true, true, true }),
        new(new[] { true, false, true }),
        new(new[] { true, true, true, true, true }),
        new(new[] { false, false, false, true }),
        new(new[] { false, true, false, false, true }),
        new(new[] { true }),
        new(new[] { false, false, true, true }),
        new(new[] { true, true, true, false, true }),
        new(new[] { true, true, true }),
        new(new[] { false, true, false, true }),
        new(new[] { false, false, false, false, true }),
        new(new[] { false, true }),
        new(new[] { true, false, false, true, true }),
        new(new[] { true, true, true, true }),
        new(new[] { false, false, true }),
        new(new[] { false, true, true, true, true }),
        new(new[] { true, false, false, true }),
        new(new[] { true, true, false, false, true }),
    };

    private static BitArray FindShufflings(Span<char> uLetters, char character)
    {
        for (int i = 0; i < 26; i++)
        {
            if (uLetters[i] == character)
                return shufflings[i];
        }
        throw new ArgumentException($"{character}不在指定范围中", nameof(character));
    }

    /// <inheritdoc cref="ICipher{TP, TC}.Decrypt(TC)"/>
    [SkipLocalsInit]
    public static string Decrypt(string text)
    {
        var words = text.Split(WORD_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
        ReadOnlySpan<char> uLetters = GlobalTables.U_Letters.AsSpan();
        Span<char> span = stackalloc char[26];
        var result = new StringBuilder();
        foreach (var word in words)
        {
            uLetters.CopyTo(span);
            foreach (var character in word)
            {
                if (character == INSIDE)
                    ShuffleInside(span);
                else if (character == OUTSIDE)
                    ShuffleOutside(span);
                else if (character == CHARACTER_END)
                    result.Append(span[0]);
            }
            result.Append(WORD_SEPARATOR);
        }
        return result.RemoveLast().ToString();
    }

    /// <summary>
    /// 加密内容
    /// </summary>
    /// <remarks>
    /// 如果<paramref name="randomize"/>为<see langword="true"/>，则每个单词有50%的可能性插入1次额外的洗牌。<br/>
    /// 如果插入了1次额外的洗牌，那么将有25%的可能性插入第2次。
    /// </remarks>
    /// <param name="text">要加密的文本</param>
    /// <param name="randomize">是否启用随机性</param>
    [SkipLocalsInit]
    public static string Encrypt(string text, bool randomize = false)
    {
        var result = new StringBuilder();
        var words = text.ToUpper().Split(separators, StringSplitOptions.RemoveEmptyEntries);
        Span<char> uLetters = GlobalTables.U_Letters.ToCharArray();
        Span<char> span = stackalloc char[26];
        foreach (var word in words)
        {
            uLetters.CopyTo(span);
            foreach (var character in word)
            {
                if (randomize && RandomHelper.TwoBits > 0)
                {
                    bool inside = RandomHelper.TrueOrFalse;
                    Shuffle(span, inside);
                    result.Append(inside ? INSIDE : OUTSIDE);
                    if (RandomHelper.TwoBits == 0)
                    {
                        inside = RandomHelper.TrueOrFalse;
                        Shuffle(span, inside);
                        result.Append(inside ? INSIDE : OUTSIDE);
                    }
                }
                var bits = FindShufflings(span, character);
                for (int i = 0; i < bits.Length; i++)
                {
                    Shuffle(span, bits[i]);
                    result.Append(bits[i] ? INSIDE : OUTSIDE);
                }
                result.Append(CHARACTER_END);
            }
            result.Append(WORD_SEPARATOR);
        }
        return result.RemoveLast().ToString();
    }

    /// <summary>
    /// 加密内容
    /// </summary>
    /// <remarks>
    /// 对于每个单词，它会分成为<paramref name="wordSplitCount"/>组。<br/>
    /// 每一组分别穷举以得到最短的表达形式(这并不意味着总体是最短的)
    /// </remarks>
    /// <param name="text">要加密的文本</param>
    /// <param name="wordSplitCount">单词分割数</param>
    [SkipLocalsInit]
    [Obsolete("这个方法的速度很慢，使用`EncryptShortInsert`方法代替")]
    public static string EncryptShort(string text, int wordSplitCount = 4)
    {
        var result = new StringBuilder();
        var words = text.ToUpper().Split(separators, StringSplitOptions.RemoveEmptyEntries);
        ReadOnlySpan<char> uLetters = GlobalTables.U_Letters.AsSpan();
        Span<char> span = stackalloc char[26];
        Span<char> originalSpan = stackalloc char[26];

        var characterList = new List<char>();
        int[] minPositions = new int[] { int.MaxValue };
        var minShuffling = new BitArray(0);

        foreach (var word in words)
        {
            uLetters.CopyTo(originalSpan);
            var repeatPosition = new List<int>();
            var subWords = FilterWord(word, repeatPosition).Partition(wordSplitCount);
            int repeatIndex = 0, characterCount = 0;
            for (int i = 0; i < subWords.Length; i++)
            {
                string subWord = subWords[i];
                if (subWord.Length == 1)
                {
                    minShuffling = FindShufflings(originalSpan, subWord[0]);
                    minPositions = new int[] { minShuffling.Length };
                }
                else
                {
                    int minCount = FindShufflings(originalSpan, subWord[0]).Length + subWord.Length - 1;
                    int maxCount = subWord.Length * 5;
                    for (int shuffeCount = minCount; shuffeCount <= maxCount; shuffeCount++)
                    {
                        for (int shuffeNumber = 0; shuffeNumber < (1 << shuffeCount); shuffeNumber++)
                        {
                            var shuffling = shuffeNumber.ToBinary(shuffeCount);
                            originalSpan.CopyTo(span);
                            characterList.Add(span[0]);
                            foreach (bool inside in shuffling)
                            {
                                Shuffle(span, inside);
                                characterList.Add(span[0]);
                            }
                            if (characterList.ContainsSubString(subWord, out int[] position))
                            {
                                if (position[^1] < minPositions[^1])
                                {
                                    minPositions = position;
                                    minShuffling = shuffling;
                                    maxCount = shuffling.Count;
                                }
                            }
                            characterList.Clear();
                        }
                    }
                }

                for (int position = 0, shufflingIndex = 0; position < minShuffling.Length;)
                {
                    if (shufflingIndex < minPositions.Length &&
                        position == minPositions[shufflingIndex])
                    {
                        result.Append(CHARACTER_END);
                        shufflingIndex++;
                        characterCount++;
                    }
                    if (repeatIndex < repeatPosition.Count &&
                        repeatPosition[repeatIndex] == characterCount)
                    {
                        result.Append(CHARACTER_END);
                        repeatIndex++;
                        characterCount++;
                    }
                    else
                    {
                        result.Append(minShuffling[position] ? INSIDE : OUTSIDE);
                        position++;
                    }
                }
                if (minPositions[^1] == minShuffling.Length)
                {
                    result.Append(CHARACTER_END);
                    characterCount++;
                }
                while (repeatIndex < repeatPosition.Count &&
                       repeatPosition[repeatIndex] == characterCount)
                {
                    result.Append(CHARACTER_END);
                    repeatIndex++;
                    characterCount++;
                }

                if (i < subWords.Length - 1)
                    for (int j = 0; j < minShuffling.Length; j++)
                        Shuffle(originalSpan, minShuffling[j]);
                minPositions[^1] = int.MaxValue;
            }
            result.Append(WORD_SEPARATOR);
        }
        result.RemoveLast();
        return result.ToString();
    }

    private static string FilterWord(string word, List<int> repeatPosition)
    {
        var filteredWord = new StringBuilder();
        filteredWord.Append(word[0]);
        for (int i = 1; i < word.Length; i++)
        {
            if (filteredWord[^1] == word[i])
                repeatPosition.Add(i);
            else
                filteredWord.Append(word[i]);
        }
        return filteredWord.ToString();
    }

    /// <summary>
    /// 加密内容
    /// </summary>
    /// <remarks>
    /// 对于每个单词，它会分成为<paramref name="wordSplitCount"/>组。<br/>
    /// 穷举在这一组前面插入<paramref name="insertCount"/>次洗牌后的最短表达形式(这并不意味着总体是最短的)
    /// </remarks>
    /// <param name="text">要加密的文本</param>
    /// <param name="insertCount">插入洗牌次数</param>
    /// <param name="wordSplitCount">单词分割数</param>
    public static string EncryptShortInsert(string text, int insertCount = 4, int wordSplitCount = 4)
    {
        throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Shuffle(Span<char> letterSpan, bool inside)
    {
        if (inside)
            ShuffleInside(letterSpan);
        else
            ShuffleOutside(letterSpan);
    }

    [SkipLocalsInit]
    private static void ShuffleInside(Span<char> letterSpan)
    {
        Span<char> copy = stackalloc char[26];
        int half = copy.Length >> 1;
        letterSpan.CopyTo(copy);
        for (int i = 0; i < half; i++)
            letterSpan[(i << 1) + 1] = copy[i];
        for (int i = half; i < copy.Length; i++)
            letterSpan[(i - half) << 1] = copy[i];
    }

    [SkipLocalsInit]
    private static void ShuffleOutside(Span<char> letterSpan)
    {
        Span<char> copy = stackalloc char[26];
        int half = copy.Length >> 1;
        letterSpan.CopyTo(copy);
        for (int i = 0; i < half; i++)
            letterSpan[i << 1] = copy[i];
        for (int i = half; i < copy.Length; i++)
            letterSpan[((i - half) << 1) + 1] = copy[i];
    }

}
