using CommunityToolkit.HighPerformance;

namespace ClassicalCryptography.Utils;

internal static class ArrayExtension
{
    /// <summary>
    /// 找到元素出现的所有位置
    /// </summary>
    public static List<int> FindAll<T>(this T[] array, T item)
        where T : notnull
    {
        var result = new List<int>();
        for (int i = 0; i < array.Length; i++)
            if (array[i].Equals(item))
                result.Add(i);
        return result;
    }

    /// <summary>
    /// 获得子数组
    /// </summary>
    public static ArraySegment<T> Subarray<T>(this T[] array, int start, int count)
    {
        return new ArraySegment<T>(array, start, count);
    }

    /// <summary>
    /// 数组段复制
    /// </summary>
    public static void CopyTo<T>(this ArraySegment<T> source, Span<T> span)
    {
        Guard.IsNotNull(source.Array);
        var copy = source.Array.AsSpan();
        copy.Slice(source.Offset, source.Count).CopyTo(span);
    }

    /// <summary>
    /// 二维转一维
    /// </summary>
    public static Span<T> AsSpan<T>(this Span2D<T> span2D)
    {
        Span<T> span = new T[span2D.Length];
        span2D.CopyTo(span);
        return span;
    }
}
