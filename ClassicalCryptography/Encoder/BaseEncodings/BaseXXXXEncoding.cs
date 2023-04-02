namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// 参考<see href="https://github.com/qntm/base32768"/>中的实现
/// </summary>
[ReferenceFrom("https://github.com/qntm/base32768", ProgramingLanguage.JavaScript, License.MIT)]
internal class BaseXXXXEncoding
{
    private readonly int BITS_PER_CHAR;
    private readonly int BITS_PER_BYTE;
    private readonly string[] pairStrings;
    private readonly Dictionary<int, List<char>> lookupE = new();
    private readonly Dictionary<int, (int, int)> lookupD = new();

    public BaseXXXXEncoding(int charBits, int byteBits, string[] pairStrs)
    {
        BITS_PER_CHAR = charBits;
        BITS_PER_BYTE = byteBits;
        pairStrings = pairStrs;
        for (int r = 0; r < pairStrings.Length; r++)
        {
            string pairString = pairStrings[r];
            var encodeRepertoire = new List<char>();
            for (int i = 0; i < pairString.Length; i += 2)
            {
                int first = pairString[i];
                int last = pairString[i + 1];
                for (int codePoint = first; codePoint <= last; codePoint++)
                    encodeRepertoire.Add((char)codePoint);
            }
            int numZBits = BITS_PER_CHAR - BITS_PER_BYTE * r;
            lookupE.Add(numZBits, encodeRepertoire);
            for (int z = 0; z < encodeRepertoire.Count; z++)
                lookupD[encodeRepertoire[z]] = (numZBits, z);
        }
    }

    public string Encode(byte[] bytes)
    {
        var result = new StringBuilder();
        int z = 0, numZBits = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            for (int j = BITS_PER_BYTE - 1; j >= 0; j--)
            {
                int bit = (bytes[i] >> j) & 1;
                z = (z << 1) + bit;
                numZBits++;

                if (numZBits == BITS_PER_CHAR)
                {
                    result.Append(lookupE[numZBits][z]);
                    z = numZBits = 0;
                }
            }
        }
        if (numZBits != 0)
        {
            while (!lookupE.ContainsKey(numZBits))
            {
                z = (z << 1) + 1;
                numZBits++;
            }

            result.Append(lookupE[numZBits][z]);
        }
        return result.ToString();
    }

    public byte[] Decode(string str)
    {
        int length = str.Length;

        var bytes = new byte[(length * BITS_PER_CHAR / BITS_PER_BYTE)];
        int byteCount = 0, bitsCount = 0;
        byte value = 0;

        for (int i = 0; i < length; i++)
        {
            if (!lookupD.ContainsKey(str[i]))
                throw new Exception($"无法识别的字符:{str[i]}");

            var (numZBits, z) = lookupD[str[i]];

            if (numZBits != BITS_PER_CHAR && i != length - 1)
                throw new Exception($"输入序列已结束:{i}");

            for (int j = numZBits - 1; j >= 0; j--)
            {
                int bit = z >> j & 1;

                value = (byte)((value << 1) + bit);
                bitsCount++;

                if (bitsCount == BITS_PER_BYTE)
                {
                    bytes[byteCount] = value;
                    byteCount++;
                    value = 0;
                    bitsCount = 0;
                }
            }
        }

        if (value != (1 << bitsCount) - 1)
            throw new Exception("填充不匹配");

        return bytes[..byteCount];
    }
}
