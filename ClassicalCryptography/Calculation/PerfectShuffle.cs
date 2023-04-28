using System.Collections;

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
    #region 常量

    /// <summary>
    /// 代表内侧洗牌的符号
    /// </summary>
    private const char INSIDE = '-';

    /// <summary>
    /// 代表外侧洗牌的符号
    /// </summary>
    private const char OUTSIDE = '.';

    /// <summary>
    /// 代表字母位置的符号
    /// </summary>
    private const char CHARACTER_SIGN = '/';

    /// <summary>
    /// 代表单词结束的符号
    /// </summary>
    private const char WORD_END_SIGN = ' ';

    #endregion

    /// <summary>
    /// 语句中单词的分隔符
    /// </summary>
    private static readonly char[] wordSeparators = { ' ', ',', '.', ':', '"', '\'' };

    /// <summary>
    /// 初始状态下让字母变为首位的最短洗牌过程
    /// </summary>
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

    /// <summary>
    /// 自定义算法
    /// </summary>
    public static CipherType Type => CipherType.Calculation;

    /// <inheritdoc cref="ICipher{TP, TC}.Decrypt"/>
    [SkipLocalsInit]
    public static string Decrypt(string cipherText)
    {
        ReadOnlySpan<char> uLetters = U_Letters;
        Span<char> span = stackalloc char[26];

        var result = new StringBuilder();
        var words = cipherText.Split(WORD_END_SIGN, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            uLetters.CopyTo(span);
            foreach (var character in word)
            {
                if (character is INSIDE)
                    ShuffleInside(span);
                else if (character is OUTSIDE)
                    ShuffleOutside(span);
                else if (character is CHARACTER_SIGN)
                    result.Append(span[0]);
            }
            result.Append(WORD_END_SIGN);
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
        ReadOnlySpan<char> uLetters = U_Letters;
        Span<char> span = stackalloc char[26];

        var result = new StringBuilder();
        var words = text.ToUpperAscii().Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            uLetters.CopyTo(span);
            foreach (var character in word)
            {
                if (randomize && RandomHelper.TwoBits > 0)
                    result.InsertRandomShuffling(span);

                foreach (var side in FindShufflings(span, character).EnumeratorUnBox())
                {
                    Shuffle(span, side);
                    result.Append(side ? INSIDE : OUTSIDE);
                }
                result.Append(CHARACTER_SIGN);
            }
            result.Append(WORD_END_SIGN);
        }
        return result.RemoveLast().ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void InsertRandomShuffling(this StringBuilder result, Span<char> span)
    {
        bool side = RandomHelper.TrueOrFalse;
        Shuffle(span, side);
        result.Append(side ? INSIDE : OUTSIDE);
        if (RandomHelper.TwoBits == 0)
        {
            side = RandomHelper.TrueOrFalse;
            Shuffle(span, side);
            result.Append(side ? INSIDE : OUTSIDE);
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
        ReadOnlySpan<char> uLetters = U_Letters;
        Span<char> originalSpan = stackalloc char[26];
        Span<char> span = stackalloc char[26];
        int[] oneElementArray = { int.MaxValue };

        var characterList = new List<char>();
        var bestPositions = oneElementArray;
        var bestShuffling = shufflings[0];

        var result = new StringBuilder();
        var words = text.ToUpper().Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);
        var repeatPosition = new List<int>();
        foreach (var word in words)
        {
            uLetters.CopyTo(originalSpan);

            int rIndex = 0, charCount = 0;
            var subWords = word.Filter(repeatPosition).PartitionUpTo(wordSplitCount);
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
                        int maxShuffeNumber = 1 << shuffeCount;
                        for (int shuffeNumber = 0; shuffeNumber < maxShuffeNumber; shuffeNumber++)
                        {
                            originalSpan.CopyTo(span);
                            characterList.Add(span[0]);

                            var shuffling = shuffeNumber.ToBitArray(shuffeCount);
                            foreach (var side in shuffling.EnumeratorUnBox())
                            {
                                Shuffle(span, side);
                                characterList.Add(span[0]);
                            }

                            if (characterList.ContainsSubsequence(subWord, out int[] position))
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
                        result.Append(CHARACTER_SIGN);
                        pIndex++; charCount++;
                    }
                    if (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                    {
                        result.Append(CHARACTER_SIGN);
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
                    result.Append(CHARACTER_SIGN);
                    charCount++;
                }
                while (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                {
                    result.Append(CHARACTER_SIGN);
                    rIndex++; charCount++;
                }

                if (i != subWords.Length - 1)
                    Shuffle(originalSpan, bestShuffling);
                bestPositions[^1] = int.MaxValue;
            }
            result.Append(WORD_END_SIGN);
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
        ReadOnlySpan<char> uLetters = U_Letters;
        Span<char> originalSpan = stackalloc char[26];
        Span<char> span = stackalloc char[26];
        Span<int> currentPosition = stackalloc int[wordSplitCount];
        int[] oneElementArray = { int.MaxValue };

        var bestPositions = oneElementArray;
        var shufflingList = new List<BitArray>();
        var bestShufflingList = new List<BitArray>();

        var result = new StringBuilder();
        var words = text.ToUpper().Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);
        var repeatPosition = new List<int>();
        foreach (var word in words)
        {
            uLetters.CopyTo(originalSpan);

            int rIndex = 0, charCount = 0;
            var subWords = Filter(word, repeatPosition).PartitionUpTo(wordSplitCount);
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
                        int maxShuffeNumber = 1 << shuffeCount;
                        for (int shuffeNumber = 0; shuffeNumber < maxShuffeNumber; shuffeNumber++)
                        {
                            originalSpan.CopyTo(span);
                            var position = currentPosition[..subWord.Length];

                            var shuffling = shuffeNumber.ToBitArray(shuffeCount);
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
                    foreach (var subItem in item.EnumeratorUnBox())
                        bestShuffling[shufflingCount++] = subItem;

                for (int position = 0, sIndex = 0; position < bestShuffling.Length;)
                {
                    if (sIndex < bestPositions.Length && position == bestPositions[sIndex])
                    {
                        result.Append(CHARACTER_SIGN);
                        sIndex++; charCount++;
                    }
                    if (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                    {
                        result.Append(CHARACTER_SIGN);
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
                    result.Append(CHARACTER_SIGN);
                    charCount++;
                }
                while (rIndex < repeatPosition.Count && charCount == repeatPosition[rIndex])
                {
                    result.Append(CHARACTER_SIGN);
                    rIndex++; charCount++;
                }

                if (i != subWords.Length - 1)
                    Shuffle(originalSpan, bestShuffling);
                bestPositions[^1] = int.MaxValue;
                bestShufflingList.Clear();
            }
            result.Append(WORD_END_SIGN);
        }
        return result.RemoveLast().ToString();
    }

    private static BitArray FindShufflings(Span<char> uLetters, char character)
    {
        int i = uLetters.IndexOf(character);
        if (i == -1)
            throw new ArgumentException($"{character}不在指定范围中", nameof(character));
        return shufflings[i];
    }

    /// <summary>
    /// 过滤相邻重复字符，并记录位置
    /// </summary>
    [SkipLocalsInit]
    private static string Filter(this string word, List<int> repeatPosition)
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
        foreach (var side in bits.EnumeratorUnBox())
        {
            Shuffle(letterSpan, side);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Shuffle(Span<char> letterSpan, bool side)
    {
        if (side)
            ShuffleInside(letterSpan);
        else
            ShuffleOutside(letterSpan);
    }

    [SkipLocalsInit]
    private static void ShuffleInside(Span<char> letterSpan)
    {
        Span<char> copy = stackalloc char[26];
        int half = copy.Length / 2;
        letterSpan.CopyTo(copy);
        for (int i = 0; i < half; i++)
            letterSpan[2 * i + 1] = copy[i];
        for (int i = half; i < copy.Length; i++)
            letterSpan[2 * (i - half)] = copy[i];
    }

    [SkipLocalsInit]
    private static void ShuffleOutside(Span<char> letterSpan)
    {
        Span<char> copy = stackalloc char[26];
        int half = copy.Length / 2;
        letterSpan.CopyTo(copy);
        for (int i = 0; i < half; i++)
            letterSpan[2 * i] = copy[i];
        for (int i = half; i < copy.Length; i++)
            letterSpan[2 * (i - half) + 1] = copy[i];
    }
}
