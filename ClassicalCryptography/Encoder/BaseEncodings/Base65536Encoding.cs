using System.Runtime.InteropServices;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <see href="https://github.com/qntm/base65536">Base65536编码</see>
/// </summary>
[Introduction("Base65536编码", "https://github.com/qntm/base65536")]
[ReferenceFrom("https://github.com/cyberdot/base65536", ProgramingLanguage.CSharp, License.MIT)]
public class Base65536Encoding : IEncoding
{
    private const int PaddingBlockStart = 5376;
    private const int BmpThreshold = 0x10000;
    private static readonly BidirectionalDictionary<byte, int> Map;

    static Base65536Encoding()
    {
        Map = new(256);
        var numbers = MemoryMarshal.Cast<byte, int>(Resources.Base65536);
        for (int i = 0; i < 256; i++)
            Map.Add((byte)i, numbers[i]);
    }

    /// <inheritdoc/>
    public static string Encode(byte[] bytes)
    {
        var result = new StringBuilder(bytes.Length);
        for (var i = 0; i < bytes.Length; i += 2)
        {
            var blockStart = i + 1 < bytes.Length ? Map[bytes[i + 1]] : PaddingBlockStart;
            result.Append(char.ConvertFromUtf32(blockStart + bytes[i]));
        }
        return result.ToString();
    }

    /*
    //我知道原始代码的作者想正确的处理unicode代理项但无此必要
    //而且这里的逻辑也存在问题
    //代理项在正常情况下应该使用ConvertToUtf32，这里直接使用text[position]是为了解码Base65536的特殊情况
    //而非代理项就可以直接text[position]，总而言之这个方法被弃用了
    private static int ConvertToCodePoint(string text, int position)
    {
        if (char.IsHighSurrogate(text[position]) || char.IsLowSurrogate(text[position]))
            return text[position];

        return char.ConvertToUtf32(text, position);
    }

    public static IEnumerable<int> ToCodePoints(string text)
    {
        Guard.IsNotNull(text);

        for (int i = 0; i < text.Length;)
        {
            int code = char.ConvertToUtf32(text, i++);
            if (code >= BmpThreshold)
                i++;
            yield return code;
        }
    }
    */

    /// <inheritdoc/>
    public static byte[] Decode(string encodeText) => Decode(encodeText, false);

    /// <summary>
    /// 字符串解码为字节数组
    /// </summary>
    /// <param name="encodeText">编码的字符串</param>
    /// <param name="ignoreGarbage">忽略错误字符</param>
    public static byte[] Decode(string encodeText, bool ignoreGarbage)
    {
        bool sequenceEnded = false;
        var bytes = new List<byte>(encodeText.Length * 2);
        for (int i = 0; i < encodeText.Length;)
        {
            int codePoint = char.ConvertToUtf32(encodeText, i++);
            if (codePoint >= BmpThreshold) i++;

            var point1 = (byte)codePoint;
            var blockStart = codePoint - point1;

            if (blockStart == PaddingBlockStart)
            {
                if (sequenceEnded)
                    throw new ArgumentException("Base65536序列已结束");

                bytes.Add(point1);
                sequenceEnded = true;
                if (ignoreGarbage) break;
            }
            else
            {
                if (Map.Inverse.TryGetValue(blockStart, out byte point2))
                {
                    if (sequenceEnded)
                        throw new ArgumentException("Base65536序列已结束");

                    bytes.Add(point1);
                    bytes.Add(point2);
                }
                else if (!ignoreGarbage)
                {
                    throw new ArgumentException($"不正确的Base65536字符:`{codePoint}`");
                }
            }
        }

        return bytes.ToArray();
    }
}
