namespace ClassicalCryptography.Utils;

/// <summary>
/// 运时验证条件的辅助方法
/// </summary>
public static class GuardEx
{
    /// <summary>
    /// 枚举类型应是预定义的
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsDefined<T>(T enumValue) where T : struct, Enum
    {
        if (!Enum.IsDefined<T>(enumValue))
            throw new ArgumentException($"枚举类型<{enumValue}>未定义", nameof(enumValue));
    }

    /// <summary>
    /// 集和应属于某个集合的子集
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsSubsetOf<T>(HashSet<T> subset, IEnumerable<T> set) where T : notnull
    {
        if (!subset.IsSubsetOf(set))
            throw new ArgumentException($"<{nameof(subset)}>不是<{set}>的子集", nameof(subset));
    }

    /// <summary>
    /// 值<paramref name="number"/>应为正整数
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsPositive(BigInteger number)
    {
        if (number.IsZero || BigInteger.IsNegative(number))
            throw new ArgumentException($"<{number}>不是正整数", nameof(number));
    }

    /// <summary>
    /// 值<paramref name="value"/>应在<paramref name="span"/>中
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValueInSpan<T>(T value, Span<T> span) where T : IEquatable<T>
    {
        if (!span.Contains(value))
            throw new ArgumentException($"<{span.ToString()}>中不包含<{value}>", nameof(value));
    }

    /// <summary>
    /// 值<paramref name="value"/>不应在<paramref name="span"/>中
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValueNotInSpan<T>(T value, ReadOnlySpan<T> span) where T : IEquatable<T>
    {
        if (span.Contains(value))
            throw new ArgumentException($"<{span.ToString()}>中包含<{value}>", nameof(value));
    }

}
