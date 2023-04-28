using CommunityToolkit.HighPerformance;
using System.Collections;

namespace ClassicalCryptography.Utils;

internal static class ArrayExtension
{
    public static IEnumerable<bool> EnumeratorUnBox(this BitArray array)
    {
        for (int i = 0; i < array.Count; i++)
            yield return array[i];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<T> RemoveLast<T>(this List<T> list, int count) where T : notnull
    {
        list.RemoveRange(list.Count - count, count);
        return list;
    }

    /// <summary>
    /// 找到元素出现的所有位置
    /// </summary>
    public static List<int> FindAll<T>(this IList<T> list, T item) where T : notnull
    {
        var result = new List<int>();
        for (int i = 0; i < list.Count; i++)
            if (list[i].Equals(item))
                result.Add(i);
        return result;
    }

    /// <inheritdoc cref="Array.IndexOf{T}(T[], T)"/>
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
}
