using CommunityToolkit.HighPerformance;

namespace ClassicalCryptography.Utils;

internal static class SpanExtension
{
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

    /// <summary>
    /// 二维转一维
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> ToFlatSpan<T>(this Span2D<T> span2D)
    {
        if (span2D.TryGetSpan(out var span))
            return span;
        return Span<T>.Empty;
    }

    /// <summary>
    /// 从<paramref name="span"/>中读取一个<typeparamref name="T"/>类型的值并写入
    /// </summary>
    public static void WriteBigEndian<T>(this ref T value, Span<byte> span) where T : unmanaged
    {
        value = default;
        ref byte reference = ref Unsafe.As<T, byte>(ref value);
        int size = Unsafe.SizeOf<T>();
        reference = ref Unsafe.Add(ref reference, size - 1);
        int length = Math.Min(size, span.Length);
        for (int i = 0; i < length; i++)
        {
            reference = span[i];
            reference = ref Unsafe.Subtract(ref reference, 1);
        }
    }

    public static void WriteBigEndian<T>(this Span<byte> span, T value) where T : unmanaged
    {
        ref byte reference = ref Unsafe.As<T, byte>(ref value);
        int size = Unsafe.SizeOf<T>();
        reference = ref Unsafe.Add(ref reference, size - 1);
        int length = Math.Min(size, span.Length);
        for (int i = 0; i < length; i++)
        {
            span[i] = reference;
            reference = ref Unsafe.Subtract(ref reference, 1);
        }
    }
}
