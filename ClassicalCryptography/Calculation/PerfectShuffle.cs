using ClassicalCryptography.Utils;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace ClassicalCryptography.Calculation;

/// <summary>
/// 完美洗牌密码，天青留下的最后一个密码
/// </summary>
public class PerfectShuffle
{

    private static readonly char[] separators = new char[] { ' ', ',', '.', ':', '"', '\'' };

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

    /// <summary>
    /// 解密内容
    /// </summary>
    [SkipLocalsInit]
    public static string Decrypt(string text)
    {
        var words = text.Split(WORD_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
        Span<char> uLetters = GlobalTables.U_Letters.ToCharArray();
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
        result.RemoveLast();
        return result.ToString();
    }

    /// <summary>
    /// 加密内容
    /// </summary>
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
        result.RemoveLast();
        return result.ToString();
    }


    /// <summary>
    /// 加密内容以<paramref name="wordSplitCount"/>分割的最短长度，实际上它甚至有可能比简单方法更长
    /// </summary>
    [SkipLocalsInit]
    public static string EncryptShort(string text, int wordSplitCount = 4)
    {
        var result = new StringBuilder();
        var words = text.ToUpper().Split(separators, StringSplitOptions.RemoveEmptyEntries);
        Span<char> uLetters = GlobalTables.U_Letters.ToCharArray();
        Span<char> span = stackalloc char[26];
        Span<char> orirginalSpan = stackalloc char[26];

        var characterList = new List<char>();
        int[] minPositions = new int[] { int.MaxValue };
        var minShuffling = new BitArray(0);

        foreach (var word in words)
        {
            uLetters.CopyTo(orirginalSpan);
            var repeatPosition = new List<int>();
            var subWords = FilterWord(word, repeatPosition).Partition(wordSplitCount);
            int repeatIndex = 0, characterCount = 0;
            for (int i = 0; i < subWords.Length; i++)
            {
                string subWord = subWords[i];
                if (subWord.Length == 1)
                {
                    minShuffling = FindShufflings(orirginalSpan, subWord[0]);
                    minPositions = new int[] { minShuffling.Length };
                }
                else
                {
                    int minCount = FindShufflings(orirginalSpan, subWord[0]).Length + subWord.Length - 1;
                    int maxCount = subWord.Length * 5;
                    for (int shuffeCount = minCount; shuffeCount <= maxCount; shuffeCount++)
                    {
                        for (int shuffeNumber = 0; shuffeNumber < (1 << shuffeCount); shuffeNumber++)
                        {
                            var shuffling = shuffeNumber.ToBinary(shuffeCount);
                            orirginalSpan.CopyTo(span);
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
                        Shuffle(orirginalSpan, minShuffling[j]);
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
        Span<char> copy = stackalloc char[letterSpan.Length];
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
        Span<char> copy = stackalloc char[letterSpan.Length];
        int half = copy.Length >> 1;
        letterSpan.CopyTo(copy);
        for (int i = 0; i < half; i++)
            letterSpan[i << 1] = copy[i];
        for (int i = half; i < copy.Length; i++)
            letterSpan[((i - half) << 1) + 1] = copy[i];
    }

}
