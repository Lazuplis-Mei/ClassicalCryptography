using System.Collections;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Calculation;

/// <summary>
/// 完美洗牌密码
/// </summary>
/// <remarks>
/// 对于字母表进行2种交替式的完美洗牌，取指定的首字母作为结果。
/// <list type="bullet">
///     <item>
///         <term>参考资料</term>
///         <description>
///             <see href="https://erikdemaine.org/papers/JuggleShuffle_Graham80/paper.pdf">erikdemaine/JuggleShuffle_Graham80</see>
///         </description>
///     </item>
///     <item>
///         <term>在线工具</term>
///         <description>
///             <see href="http://erikdemaine.org/fonts/shuffle/">erikdemaine/shuffle</see>
///         </description>
///     </item>
/// </list>
/// </remarks>
[Introduction("完美洗牌密码", "对于字母表进行2种交替式的完美洗牌，取指定的首字母作为结果")]
public static class PerfectShuffle
{
    private const char INSIDE = '-';
    private const char OUTSIDE = '.';
    private const char CHARACTER_POSITION = '/';
    private const char WORD_SEPARATOR = ' ';
    private static readonly char[] separators = { ' ', ',', '.', ':', '"', '\'' };
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

    /// <inheritdoc/>
    public static CipherType Type => CipherType.Calculation;

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static string Decrypt(string text)
    {
        ReadOnlySpan<char> uLetters = GlobalTables.U_Letters;
        Span<char> span = stackalloc char[26];

        var result = new StringBuilder();
        var words = text.Split(WORD_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            uLetters.CopyTo(span);
            foreach (var character in word)
            {
                if (character is INSIDE)
                    ShuffleInside(span);
                else if (character is OUTSIDE)
                    ShuffleOutside(span);
                else if (character is CHARACTER_POSITION)
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
    /// 如果<paramref name="randomize"/>为<see langword="true"/>，则每个字母都有50%的可能性插入1次额外的洗牌。<br/>
    /// 如果已经插入了1次额外的洗牌，那么将有25%的可能性插入第2次。
    /// </remarks>
    /// <param name="text">要加密的文本</param>
    /// <param name="randomize">是否启用随机性</param>
    [SkipLocalsInit]
    public static string Encrypt(string text, bool randomize = false)
    {
        ReadOnlySpan<char> uLetters = GlobalTables.U_Letters;
        Span<char> span = stackalloc char[26];

        var result = new StringBuilder();
        var words = text.ToUpper().Split(separators, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            uLetters.CopyTo(span);
            foreach (var character in word)
            {
                if (randomize && RandomHelper.TwoBits > 0)
                    InsertRandomShuffling(result, span);

                foreach (bool inside in FindShufflings(span, character))
                {
                    Shuffle(span, inside);
                    result.Append(inside ? INSIDE : OUTSIDE);
                }
                result.Append(CHARACTER_POSITION);
            }
            result.Append(WORD_SEPARATOR);
        }
        return result.RemoveLast().ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void InsertRandomShuffling(StringBuilder result, Span<char> span)
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
    }

    /// <summary>
    /// 加密内容
    /// </summary>
    /// <remarks>
    /// 对于每个单词，它会分成为<paramref name="wordSplitCount"/>组。<br/>
    /// 每一组分别穷举以得到最短的表达形式(这并不意味着总体是最短的)。
    /// </remarks>
    /// <param name="text">要加密的文本</param>
    /// <param name="wordSplitCount">单词分割数</param>
    [SkipLocalsInit]
    [Obsolete("这个方法的速度很慢，使用`EncryptShortInsert`方法代替")]
    public static string EncryptShort(string text, int wordSplitCount = 4)
    {
        ReadOnlySpan<char> uLetters = GlobalTables.U_Letters;
        Span<char> originalSpan = stackalloc char[26];
        Span<char> span = stackalloc char[26];
        int[] oneElementArray = { int.MaxValue };

        var characterList = new List<char>();
        var bestPositions = oneElementArray;
        var bestShuffling = new BitArray(0);

        var result = new StringBuilder();
        var words = text.ToUpper().Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var repeatPosition = new List<int>();
        foreach (var word in words)
        {
            uLetters.CopyTo(originalSpan);

            int rIndex = 0, charCount = 0;
            var subWords = FilterWord(word, repeatPosition).Partition(wordSplitCount);
            for (int i = 0; i < subWords.Length; i++)
            {
                string subWord = subWords[i];
                if (subWord.Length == 1)
                {
                    bestShuffling = FindShufflings(originalSpan, subWord[0]);
                    bestPositions = oneElementArray;
                    bestPositions[0] = bestShuffling.Length;
                }
                else
                {
                    int minCount = FindShufflings(originalSpan, subWord[0]).Length + subWord.Length - 1;
                    int maxCount = subWord.Length * 5;
                    for (int shuffeCount = minCount; shuffeCount <= maxCount; shuffeCount++)
                    {
                        for (int shuffeNumber = 0; shuffeNumber < (1 << shuffeCount); shuffeNumber++)
                        {
                            originalSpan.CopyTo(span);
                            characterList.Add(span[0]);

                            var shuffling = shuffeNumber.ToBinary(shuffeCount);
                            foreach (bool inside in shuffling)
                            {
                                Shuffle(span, inside);
                                characterList.Add(span[0]);
                            }

                            if (characterList.ContainsSubString(subWord, out int[] position))
                            {
                                if (position[^1] < bestPositions[^1])
                                {
                                    bestPositions = position;
                                    bestShuffling = shuffling;
                                    maxCount = shuffling.Count;
                                }
                            }
                            characterList.Clear();
                        }
                    }
                }

                for (int position = 0, pIndex = 0; position < bestShuffling.Length;)
                {
                    if (pIndex < bestPositions.Length && position == bestPositions[pIndex])
                    {
                        result.Append(CHARACTER_POSITION);
                        pIndex++; charCount++;
                    }
                    if (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                    {
                        result.Append(CHARACTER_POSITION);
                        rIndex++; charCount++;
                    }
                    else
                    {
                        result.Append(bestShuffling[position] ? INSIDE : OUTSIDE);
                        position++;
                    }
                }

                if (bestPositions[^1] == bestShuffling.Length)
                {
                    result.Append(CHARACTER_POSITION);
                    charCount++;
                }
                while (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                {
                    result.Append(CHARACTER_POSITION);
                    rIndex++; charCount++;
                }

                if (i < subWords.Length - 1)
                    Shuffle(originalSpan, bestShuffling);
                bestPositions[^1] = int.MaxValue;
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
    /// 穷举在这一组前面插入<paramref name="insertCount"/>次洗牌后的最短表达形式(这并不意味着总体是最短的)
    /// </remarks>
    /// <param name="text">要加密的文本</param>
    /// <param name="insertCount">插入洗牌次数</param>
    /// <param name="wordSplitCount">单词分割数</param>
    public static string EncryptShortInsert(string text, int insertCount = 5, int wordSplitCount = 5)
    {
        ReadOnlySpan<char> uLetters = GlobalTables.U_Letters;
        Span<char> originalSpan = stackalloc char[26];
        Span<char> span = stackalloc char[26];
        Span<int> currentPosition = stackalloc int[wordSplitCount];
        int[] oneElementArray = { int.MaxValue };

        var bestPositions = oneElementArray;
        var shufflingList = new List<BitArray>();
        var bestShufflingList = new List<BitArray>();

        var result = new StringBuilder();
        var words = text.ToUpper().Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var repeatPosition = new List<int>();
        foreach (var word in words)
        {
            uLetters.CopyTo(originalSpan);

            int rIndex = 0, charCount = 0;
            var subWords = FilterWord(word, repeatPosition).Partition(wordSplitCount);
            for (int i = 0; i < subWords.Length; i++)
            {
                string subWord = subWords[i];
                if (subWord.Length == 1)
                {
                    var shuffling = FindShufflings(originalSpan, subWord[0]);
                    bestPositions = oneElementArray;
                    bestPositions[0] = shuffling.Length;
                    bestShufflingList.Add(shuffling);
                }
                else
                {
                    for (int shuffeCount = 0; shuffeCount <= insertCount; shuffeCount++)
                    {
                        for (int shuffeNumber = 0; shuffeNumber < (1 << shuffeCount); shuffeNumber++)
                        {
                            originalSpan.CopyTo(span);
                            var position = currentPosition[..subWord.Length];

                            var shuffling = shuffeNumber.ToBinary(shuffeCount);
                            shufflingList.Add(shuffling);
                            Shuffle(span, shuffling);

                            int charPosition = shuffling.Length;
                            int currentIndex = 0;
                            foreach (var character in subWord)
                            {
                                shuffling = FindShufflings(span, character);
                                Shuffle(span, shuffling);
                                shufflingList.Add(shuffling);
                                charPosition += shuffling.Length;
                                position[currentIndex++] = charPosition;
                            }

                            if (currentPosition[^1] < bestPositions[^1])
                            {
                                bestPositions = position.ToArray();
                                bestShufflingList.Clear();
                                bestShufflingList.AddRange(shufflingList);
                            }
                            shufflingList.Clear();
                        }
                    }
                }

                int shufflingCount = bestShufflingList.Sum(bits => bits.Length);
                var bestShuffling = new BitArray(shufflingCount);
                shufflingCount = 0;
                foreach (var item in bestShufflingList)
                {
                    foreach (bool subItem in item)
                    {
                        bestShuffling[shufflingCount++] = subItem;
                    }
                }

                for (int position = 0, sIndex = 0; position < bestShuffling.Length;)
                {
                    if (sIndex < bestPositions.Length && position == bestPositions[sIndex])
                    {
                        result.Append(CHARACTER_POSITION);
                        sIndex++; charCount++;
                    }
                    if (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                    {
                        result.Append(CHARACTER_POSITION);
                        rIndex++; charCount++;
                    }
                    else
                    {
                        result.Append(bestShuffling[position] ? INSIDE : OUTSIDE);
                        position++;
                    }
                }

                if (bestPositions[^1] == bestShuffling.Length)
                {
                    result.Append(CHARACTER_POSITION);
                    charCount++;
                }
                while (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                {
                    result.Append(CHARACTER_POSITION);
                    rIndex++; charCount++;
                }

                if (i < subWords.Length - 1)
                    Shuffle(originalSpan, bestShuffling);
                bestPositions[^1] = int.MaxValue;
                bestShufflingList.Clear();
            }
            result.Append(WORD_SEPARATOR);
        }
        return result.RemoveLast().ToString();
    }

    private static BitArray FindShufflings(Span<char> permut, char character)
    {
        for (int i = 0; i < 26; i++)
            if (permut[i] == character)
                return shufflings[i];
        throw new ArgumentException($"{character}不在指定范围中", nameof(character));
    }

    private static string FilterWord(string word, List<int> repeatPosition)
    {
        repeatPosition.Clear();
        int count = word.Length;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        span[0] = word[0];
        for (int i = count = 1; i < word.Length; i++)
        {
            if (span[count - 1] == word[i])
                repeatPosition.Add(i);
            else
                span[count++] = word[i];
        }
        return new(span[..count]);
    }

    private static void Shuffle(Span<char> letterSpan, BitArray bits)
    {
        foreach (bool inside in bits)
        {
            Shuffle(letterSpan, inside);
        }
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
