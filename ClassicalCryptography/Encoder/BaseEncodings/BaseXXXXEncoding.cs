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
        using var memory = size.TryAllocString();
        Span<char> span = size.CanAllocString() ? stackalloc char[size] : memory.Span;

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
        int size = length * BITS_PER_CHAR / BITS_PER_BYTE;
        using var memory = size.TryAlloc();
        Span<byte> span = size.CanAlloc() ? stackalloc byte[size] : memory.Span;

        int index = 0, byteBitsCount = 0;
        byte value = 0;
        for (int i = 0; i < length; i++)
        {
            var character = encodeText[i];
            if (!lookupD.ContainsKey(character))
                throw new ArgumentException($"无法识别的字符:`{character}`;位置`{i}`", nameof(encodeText));

            var (bitsCount, k) = lookupD[character];
            if (bitsCount != BITS_PER_CHAR && i != length - 1)
                throw new ArgumentException($"输入序列已结束;位置`{i}`", nameof(encodeText));

            for (int j = bitsCount - 1; j >= 0; j--)
            {
                value = (byte)((value << 1) + ((k >> j) & 1));
                if (++byteBitsCount == BITS_PER_BYTE)
                {
                    span[index++] = value;
                    byteBitsCount = value = 0;
                }
            }
        }

        if (value != (1 << byteBitsCount) - 1)
            throw new ArgumentException("填充不匹配", nameof(encodeText));
        return span[..index].ToArray();
    }
}
