namespace ClassicalCryptography.Utils;

/// <summary>
/// 组合工具类
/// </summary>
public static class PermutHelper
{

    /// <summary>
    /// 使用康托展开获得<paramref name="count"/>个数的第n个排列
    /// </summary>
    /// <param name="n">第n个排列</param>
    /// <param name="count">排列的数的个数</param>
    public static ushort[] GetPermutation(BigInteger n, int count)
    {
        var result = new ushort[count];
        var list = new List<ushort>(count + 1).FillOrder(count + 1);
        for (int i = 0; i < count; i++)
        {
            result[i] = list[(int)BigInteger.DivRem(n, MathEx.Factorial(count - i - 1), out n)];
            list.Remove(result[i]);
        }
        return result;
    }

    /// <summary>
    /// 使用康托展开填充span为第n个排列
    /// </summary>
    public static void FillPermutation(this Span<ushort> span, BigInteger number)
    {
        var list = new List<ushort>(span.Length + 1).FillOrder(span.Length + 1);
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = list[(int)BigInteger.DivRem(number, MathEx.Factorial(span.Length - i - 1), out number)];
            list.Remove(span[i]);
        }
    }

    /// <summary>
    /// 使用逆康托展开获得一个排列的序号
    /// </summary>
    public static BigInteger GetPermutIndex(ushort[] permute)
    {
        var result = BigInteger.Zero;
        for (int i = 0; i < permute.Length - 1; i++)
        {
            int t = permute.Length - i - 1;
            result += permute.TakeLast(t).Count(j => j < permute[i]) * MathEx.Factorial(t);
        }
        return result;
    }

    /// <summary>
    /// 将单词转换成排列
    /// </summary>
    /// <param name="word">英文单词</param>
    /// <param name="options">转换选项</param>
    /// <returns>
    /// 一个合法的排列，如果<paramref name="options"/>不为<see cref="WordToPermutationOptions.Myszkowski"/><br/>
    /// 否则为可能存在重复数值的排列，用于特定的情况
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1854", Justification = "<挂起>")]
    public static string WordToPermutation(string word, WordToPermutationOptions options = WordToPermutationOptions.IgnoreDuplicated)
    {
        Guard.HasSizeGreaterThan(word, 1);
        Guard.IsTrue(Enum.IsDefined(options));
        Span<char> span = word.ToCharArray();
        span.Sort();
        var result = new StringBuilder();
        if (options == WordToPermutationOptions.IgnoreDuplicated)
        {
            FilterWord(ref span);
            var hashset = new HashSet<int>();
            foreach (char character in word)
            {
                if (hashset.Add(character))
                {
                    result.Append(span.IndexOf(character) + 1);
                    result.Append(',');
                }
            }
        }
        else if (options == WordToPermutationOptions.AllowDuplicated)
        {
            var dict = new Dictionary<char, int>();
            foreach (char character in word)
            {
                if (dict.ContainsKey(character))
                    result.Append(span.IndexOf(character) + ++dict[character]);
                else
                    result.Append(span.IndexOf(character) + (dict[character] = 1));
                result.Append(',');
            }
        }
        else if (options == WordToPermutationOptions.Myszkowski)
        {
            FilterWord(ref span);
            foreach (char character in word)
            {
                result.Append(span.IndexOf(character) + 1);
                result.Append(',');
            }
        }
        return result.RemoveLast().ToString();

        static void FilterWord(ref Span<char> span)
        {
            for (int i = 0; i < span.Length - 1; i++)
            {
                if (span[i] == span[i + 1])
                {
                    for (int j = i + 1; j < span.Length - 1; j++)
                        span[j] = span[j + 1];
                    span = span[..^1];
                    i--;
                }
            }
        }
    }

}
