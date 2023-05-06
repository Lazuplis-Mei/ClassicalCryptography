using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 一个将0~255对应到另一组对象的映射
/// </summary>
internal class ByteMap<T> : IReadOnlyDictionary<T, byte> where T : notnull
{
    private readonly T[] values;
    private readonly Dictionary<T, byte> map;

    public ByteMap(T[] values)
    {
        Guard.HasSizeEqualTo(values, 256);
        Guard.IsFalse(values[0] is byte);
        this.values = values;
        map = new(256);
        for (int i = 0; i < 256; i++)
            map.Add(values[i], (byte)i);
    }

    public T this[byte index] => values[index];

    public byte this[T key] => map[key];

    public IEnumerable<T> Keys => map.Keys;

    public IEnumerable<byte> Values => map.Values;

    public int Count => 256;

    public bool ContainsKey(T key) => map.ContainsKey(key);

    public IEnumerator<KeyValuePair<T, byte>> GetEnumerator() => map.GetEnumerator();

    public bool TryGetValue(T key, [MaybeNullWhen(false)] out byte value) => map.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => map.GetEnumerator();
}
