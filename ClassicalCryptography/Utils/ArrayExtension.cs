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
    /// 获得子数组
    /// </summary>
    public static ArraySegment<T> SubArray<T>(this T[] array, int start, int count)
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
}