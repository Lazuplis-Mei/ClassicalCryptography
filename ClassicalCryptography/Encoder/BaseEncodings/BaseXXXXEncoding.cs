using CommunityToolkit.HighPerformance.Buffers;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// 参考<see href="https://github.com/qntm/base32768"/>中的实现
/// </summary>
[ReferenceFrom("https://github.com/qntm/base32768", ProgramingLanguage.JavaScript, License.MIT)]
internal class BaseXXXXEncoding
{
    private const int BITS_PER_BYTE = 8;
    private readonly int BITS_PER_CHAR;
    private readonly Dictionary<int, char[]> lookupE;
    private readonly Dictionary<char, (int, int)> lookupD;

    public BaseXXXXEncoding(int bitsPerChar, string[] pairStrings)
    {
        BITS_PER_CHAR = bitsPerChar;
        lookupE = new(pairStrings.Length);
        int count = 0;
        for (int i = 0; i < pairStrings.Length; i++)
        {
            string pairString = pairStrings[i];
            int bitsCount = BITS_PER_CHAR - BITS_PER_BYTE * i, index = 0;
            var repertoire = new char[1 << bitsCount];
            for (int j = 0; j < pairString.Length; j++)
                for (char code = pairString[j++]; code <= pairString[j]; code++)
                    repertoire[index++] = code;
            lookupE.Add(bitsCount, repertoire);
            count += repertoire.Length;
        }
        lookupD = new(count);
        foreach (var (bitsCount, repertoire) in lookupE)
            for (int j = 0; j < repertoire.Length; j++)
                lookupD[repertoire[j]] = (bitsCount, j);
    }

    [SkipLocalsInit]
    public string Encode(byte[] bytes)
    {
        int size = (bytes.Length * BITS_PER_BYTE).DivCeil(BITS_PER_CHAR);
        Span<char> span = size.CanAllocString() ? stackalloc char[size] : new char[size];
        int value = 0, bitsCount = 0, index = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            for (int j = BITS_PER_BYTE - 1; j >= 0; j--)
            {
                value = (value << 1) + ((bytes[i] >> j) & 1);
                if (++bitsCount == BITS_PER_CHAR)
                {
                    span[index++] = lookupE[bitsCount][value];
                    bitsCount = value = 0;
                }
            }
        }
        if (bitsCount != 0)
        {
            while (!lookupE.ContainsKey(bitsCount))
            {
                value = (value << 1) + 1;
                bitsCount++;
            }
            span[index++] = lookupE[bitsCount][value];
        }
        return new(span);
    }

    public byte[] Decode(string encodeText)
    {
        int length = encodeText.Length;
        using var memory = MemoryOwner<byte>.Allocate(length * BITS_PER_CHAR / BITS_PER_BYTE);
        var bytes = memory.Span;

        int index = 0, byteBitsCount = 0;
        byte value = 0;
        for (int i = 0; i < length; i++)
        {
            if (!lookupD.ContainsKey(encodeText[i]))
                throw new ArgumentException($"无法识别的字符:`{encodeText[i]}`;位置`{i}`", nameof(encodeText));

            var (bitsCount, k) = lookupD[encodeText[i]];
            if (bitsCount != BITS_PER_CHAR && i != length - 1)
                throw new ArgumentException($"输入序列已结束;位置`{i}`", nameof(encodeText));

            for (int j = bitsCount - 1; j >= 0; j--)
            {
                value = (byte)((value << 1) + ((k >> j) & 1));
                if (++byteBitsCount == BITS_PER_BYTE)
                {
                    bytes[index++] = value;
                    byteBitsCount = value = 0;
                }
            }
        }

        if (value != (1 << byteBitsCount) - 1)
            throw new ArgumentException("填充不匹配", nameof(encodeText));
        return bytes[..index].ToArray();
    }
}
