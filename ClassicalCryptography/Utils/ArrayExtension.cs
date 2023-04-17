using CommunityToolkit.HighPerformance;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Utils;

internal static class ArrayExtension
{
    /// <summary>
    /// 找到元素出现的所有位置
    /// </summary>
    public static List<int> FindAll<T>(this T[] array, T item) where T : notnull
    {
        var result = new List<int>();
        for (int i = 0; i < array.Length; i++)
            if (array[i].Equals(item))
                result.Add(i);
        return result;
    }

    /// <summary>
    /// 找到元素出现的所有位置
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this T[] array, T item) where T : notnull
    {
        return Array.IndexOf(array, item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
            action(item);
    }

    /// <summary>
    /// 获得子数组
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArraySegment<T> Subarray<T>(this T[] array, int start, int count)
    {
        return new ArraySegment<T>(array, start, count);
    }

    /// <summary>
    /// 数组段复制
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<T>(this ArraySegment<T> source, Span<T> dest)
    {
        source.CopyTo(dest, 0);
    }

    /// <summary>
    /// 数组段复制
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<T>(this ArraySegment<T> source, Span<T> dest, int start)
    {
        Guard.IsNotNull(source.Array);
        Span<T> span = source.Array.AsSpan();
        span.Slice(source.Offset, source.Count).CopyTo(dest[start..]);
    }

    /// <summary>
    /// 二维转一维
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] ToFlatArray<T>(this Span2D<T> span2D)
    {
        var array = new T[span2D.Length];
        span2D.CopyTo(array);
        return array;
    }
}
