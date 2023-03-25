namespace ClassicalCryptography.Encoder.BaseEncodings;


/// <summary>
/// <a href="https://en.wikipedia.org/wiki/Ascii85">Base85编码，代码请参考</a>
/// </summary>
[ReferenceFrom("https://github.com/ikorin24/Base85/blob/master/Base85/Base85.cs", ProgramingLanguage.CSharp, License.Apache2)]
public class Base85Encoding : IEncoding
{
    /// <inheritdoc/>
    public static string Encode(byte[] bytes) => Encode(bytes, true);

    /// <summary>
    /// 编码字节数组为字符串
    /// </summary>
    /// <param name="bytes">要编码的字节</param>
    /// <param name="noHeadTail">是否不添加头尾字符</param>
    public static string Encode(byte[] bytes, bool noHeadTail)
    {
        const int ASCII_OFFSET = 33;
        var loopCount = (bytes.Length + 3) >> 2;
        int paddingCount = bytes.Length & 0B11;
        if (paddingCount != 0)
            paddingCount = 4 - paddingCount;

        int length = loopCount;
        length += (length << 2) - paddingCount + 4;
        var result = new List<byte>(length);
        if (!noHeadTail)
        {
            result.Add((byte)'<');
            result.Add((byte)'~');
        }

        for (int i = 0; i < loopCount; i++)
        {
            int j = i << 2;
            uint block;
            byte byte0, byte1;
            byte? byte2 = null, byte3 = null, byte4 = null;
            if (i != loopCount - 1 || paddingCount == 0)
            {
                block = (uint)((bytes[j] << 24) + (bytes[j + 1] << 16) + (bytes[j + 2] << 8) + bytes[j + 3]);
            }
            else
            {
                block = paddingCount switch
                {
                    1 => (uint)((bytes[j] << 24) + (bytes[j + 1] << 16) + (bytes[j + 2] << 8)),
                    2 => (uint)((bytes[j] << 24) + (bytes[j + 1] << 16)),
                    3 => (uint)((bytes[j] << 24)),
                    _ => 0,
                };
            }
            if (block == 0)
            {
                result.Add((byte)'z');
                continue;
            }

            uint temp = block - block % 85;
            if (i != loopCount - 1 || paddingCount < 1)
                byte4 = (byte)(block - temp + ASCII_OFFSET);
            block = temp / 85;

            temp = block - block % 85;
            if (i != loopCount - 1 || paddingCount < 2)
                byte3 = (byte)(block - temp + ASCII_OFFSET);
            block = temp / 85;

            temp = block - block % 85;
            if (i != loopCount - 1 || paddingCount < 3)
                byte2 = (byte)(block - temp + ASCII_OFFSET);
            block = temp / 85;

            temp = block - block % 85;
            byte1 = (byte)(block - temp + ASCII_OFFSET);
            block = temp / 85;

            temp = block - block % 85;
            byte0 = (byte)(block - temp + ASCII_OFFSET);

            result.Add(byte0);
            result.Add(byte1);
            if (byte2.HasValue)
                result.Add(byte2.Value);
            if (byte3.HasValue)
                result.Add(byte3.Value);
            if (byte4.HasValue)
                result.Add(byte4.Value);
        }
        if (!noHeadTail)
        {
            result.Add((byte)'~');
            result.Add((byte)'>');
        }
        return Encoding.ASCII.GetString(result.ToArray());
    }

    /// <inheritdoc/>
    public static byte[] Decode(string encodeText)
    {
        const int ASCII_OFFSET = 33;
        const byte PADDING_ASCII = (byte)'u';

        if (encodeText[0] == '<' && encodeText[1] == '~' &&
            encodeText[^2] == '~' && encodeText[^1] == '>')
            encodeText = encodeText[2..^2];

        var chars = Encoding.ASCII.GetBytes(encodeText);
        var skipCount = 0;
        var result = new List<byte>((chars.Length / 5) << 2);

        for (int i = 0; ; i++)
        {
            int j = i;
            j += (j << 2) + (skipCount << 2);

            if (j >= chars.Length)
                break;
            if (chars[j] == 'z')
            {
                result.Add(0);
                result.Add(0);
                result.Add(0);
                result.Add(0);
                skipCount++;
            }
            else
            {
                bool isLastLoop = j >= chars.Length - 5;
                uint block;
                int paddingCount = 0;
                if (!isLastLoop)
                {
                    block = (uint)(chars[j] - ASCII_OFFSET) * 52200625 +
                            (uint)(chars[j + 1] - ASCII_OFFSET) * 614125 +
                            (uint)(chars[j + 2] - ASCII_OFFSET) * 7225 +
                            (uint)(chars[j + 3] - ASCII_OFFSET) * 85 +
                            (uint)(chars[j + 4] - ASCII_OFFSET);
                }
                else
                {
                    paddingCount = 5 - (chars.Length - j);
                    block = paddingCount switch
                    {
                        0 => (uint)(chars[j] - ASCII_OFFSET) * 52200625 +
                             (uint)(chars[j + 1] - ASCII_OFFSET) * 614125 +
                             (uint)(chars[j + 2] - ASCII_OFFSET) * 7225 +
                             (uint)(chars[j + 3] - ASCII_OFFSET) * 85 +
                             (uint)(chars[j + 4] - ASCII_OFFSET),
                        1 => (uint)(chars[j] - ASCII_OFFSET) * 52200625 +
                             (uint)(chars[j + 1] - ASCII_OFFSET) * 614125 +
                             (uint)(chars[j + 2] - ASCII_OFFSET) * 7225 +
                             (uint)(chars[j + 3] - ASCII_OFFSET) * 85 +
                             (PADDING_ASCII - ASCII_OFFSET),
                        2 => (uint)(chars[j] - ASCII_OFFSET) * 52200625 +
                             (uint)(chars[j + 1] - ASCII_OFFSET) * 614125 +
                             (uint)(chars[j + 2] - ASCII_OFFSET) * 7225 +
                             (uint)(PADDING_ASCII - ASCII_OFFSET) * 85 +
                             (PADDING_ASCII - ASCII_OFFSET),
                        3 => (uint)(chars[j] - ASCII_OFFSET) * 52200625 +
                             (uint)(chars[j + 1] - ASCII_OFFSET) * 614125 +
                             (uint)(PADDING_ASCII - ASCII_OFFSET) * 7225 +
                             (uint)(PADDING_ASCII - ASCII_OFFSET) * 85 +
                             (PADDING_ASCII - ASCII_OFFSET),
                        _ => (uint)(chars[j] - ASCII_OFFSET) * 52200625 +
                             (uint)(PADDING_ASCII - ASCII_OFFSET) * 614125 +
                             (uint)(PADDING_ASCII - ASCII_OFFSET) * 7225 +
                             (uint)(PADDING_ASCII - ASCII_OFFSET) * 85 +
                             (PADDING_ASCII - ASCII_OFFSET),
                    };
                }
                result.Add((byte)(block >> 24));
                if (!isLastLoop || paddingCount <= 2)
                    result.Add((byte)((block & 0x00ff0000) >> 16));
                if (!isLastLoop || paddingCount <= 1)
                    result.Add((byte)((block & 0x0000ff00) >> 8));
                if (!isLastLoop || paddingCount <= 0)
                    result.Add((byte)block);
            }
        }

        return result.ToArray();
    }
}
