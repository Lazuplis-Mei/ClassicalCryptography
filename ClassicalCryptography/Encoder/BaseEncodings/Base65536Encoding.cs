using ClassicalCryptography.Interfaces;
using System.Text;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <para>Base65536编码</para>
/// <seealso href="https://github.com/qntm/base65536"/>
/// <para>代码参考</para>
/// <see href="https://github.com/cyberdot/base65536"/>
/// </summary>
[Introduction("Base65536编码", "https://github.com/qntm/base65536")]
public static partial class Base65536Encoding
{
    private const int PaddingBlockStart = 5376;
    private const int BmpThreshold = 0x10000;
    private const int High = 0xD800;
    private const int Low = 0xDC00;
    private const int Offset = 0x400;

    /// <summary>
    /// encode Base65536
    /// </summary>
    public static string Encode(byte[] bytes)
    {
        var result = new StringBuilder(bytes.Length);

        for (var i = 0; i < bytes.Length; i += 2)
        {
            var blockStart = i + 1 < bytes.Length ? Map[bytes[i + 1]] : PaddingBlockStart;
            var codePoint = blockStart + bytes[i];

            if (codePoint < BmpThreshold)
            {
                result.Append(char.ConvertFromUtf32(codePoint));
            }
            else
            {
                var first = High + (codePoint - BmpThreshold) / Offset;
                var second = Low + codePoint % Offset;
                result.Append((char)first).Append((char)second);
            }
        }
        return result.ToString();
    }

    private static int ConvertToCodePoint(string str, int i)
    {
        if (char.IsHighSurrogate(str[i]) || char.IsLowSurrogate(str[i]))
            return str[i];

        return char.ConvertToUtf32(str, i);
    }
    /// <summary>
    /// ToCodePoints
    /// </summary>
    public static IEnumerable<int> ToCodePoints(this string str)
    {
        Guard.IsNotNull(str);

        for (int i = 0; i < str.Length;)
        {
            var first = ConvertToCodePoint(str, i++);

            if (first is >= High and < (High + Offset))
            {
                var snd = ConvertToCodePoint(str, i++);

                if (Low <= snd && snd < Low + Offset)
                    yield return (first - High) * Offset + (snd - Low) + BmpThreshold;
                else
                    throw new ArgumentException("Invalid UTF 16");
            }
            else
            {
                yield return first;
            }
        }
    }

    /// <summary>
    /// decode Base65536
    /// </summary>
    public static byte[] Decode(string data, bool ignoreGarbage = false)
    {
        var done = false;
        var bytes = new List<byte>();

        foreach (var codePoint in data.ToCodePoints())
        {
            var point1 = (byte)codePoint;
            var blockStart = codePoint - point1;

            if (blockStart == PaddingBlockStart)
            {
                if (done) throw new ArgumentException("Base65536序列已结束");

                bytes.Add(point1);
                done = true;
            }
            else
            {
                if (Map.Inverse.TryGetValue(blockStart, out int point2))
                {
                    if (done) throw new ArgumentException("Base65536序列已结束");

                    bytes.Add(point1);
                    bytes.Add((byte)point2);
                }
                else if (!ignoreGarbage)
                {
                    throw new ArgumentException("不正确的Base65536字符" + codePoint);
                }
            }
        }
        return bytes.ToArray();
    }

}
