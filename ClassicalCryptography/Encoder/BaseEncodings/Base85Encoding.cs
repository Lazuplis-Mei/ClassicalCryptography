using System.Text;

namespace ClassicalCryptography.Encoder.BaseEncodings;


/// <summary>
/// <para>Base85编码，代码请参考</para>
/// <see href="https://github.com/ikorin24/Base85/blob/master/Base85/Base85.cs"/>
/// </summary>
public class Base85Encoding
{
    /// <summary>
    /// Encode Base85
    /// </summary>
    public static string Encode(byte[] bytes, bool noHeadTail = true)
    {
        const int ASCII_OFFSET = 33;
        var loopCount = (bytes.Length + 3) >> 2;
        int paddingCount = (bytes.Length % 4 == 0) ? 0 : 4 - bytes.Length % 4;
        var result = new List<byte>(loopCount * 5 + 4 - paddingCount);
        if (!noHeadTail)
        {
            result.Add((byte)'<');
            result.Add((byte)'~');
        }

        for (int i = 0; i < loopCount; i++)
        {
            int j = i << 2;
            uint block;
            byte ans0, ans1;
            byte? ans2 = null, ans3 = null, ans4 = null;
            if (i != loopCount - 1)
            {
                block = (uint)((bytes[j] << 24) + (bytes[j + 1] << 16) + (bytes[j + 2] << 8) + bytes[j + 3]);
            }
            else
            {
                block = (paddingCount == 0) ? (uint)((bytes[j] << 24) + (bytes[j + 1] << 16) + (bytes[j + 2] << 8) + bytes[j + 3]) :
                        (paddingCount == 1) ? (uint)((bytes[j] << 24) + (bytes[j + 1] << 16) + (bytes[j + 2] << 8)) :
                        (paddingCount == 2) ? (uint)((bytes[j] << 24) + (bytes[j + 1] << 16)) :
                                              (uint)((bytes[j] << 24));
            }
            if (block == 0)
            {
                result.Add((byte)'z');
                continue;
            }
            uint tmp = (block / 85) * 85;
            if (i != loopCount - 1 || paddingCount < 1)
                ans4 = (byte)(block - tmp + ASCII_OFFSET);
            block = tmp / 85;
            tmp = (block / 85) * 85;
            if (i != loopCount - 1 || paddingCount < 2)
                ans3 = (byte)(block - tmp + ASCII_OFFSET);
            block = tmp / 85;
            tmp = (block / 85) * 85;
            if (i != loopCount - 1 || paddingCount < 3)
                ans2 = (byte)(block - tmp + ASCII_OFFSET);
            block = tmp / 85;
            tmp = (block / 85) * 85;
            ans1 = (byte)(block - tmp + ASCII_OFFSET);
            block = tmp / 85;
            tmp = (block / 85) * 85;
            ans0 = (byte)(block - tmp + ASCII_OFFSET);
            result.Add(ans0);
            result.Add(ans1);
            if (ans2 != null)
                result.Add(ans2.Value);
            if (ans3 != null)
                result.Add(ans3.Value);
            if (ans4 != null)
                result.Add(ans4.Value);
        }
        if (!noHeadTail)
        {
            result.Add((byte)'~');
            result.Add((byte)'>');
        }
        return Encoding.ASCII.GetString(result.ToArray());
    }

    /// <summary>
    /// Decode Base85 without Head and Tail
    /// </summary>
    /// <param name="text"></param>
    public static byte[] Decode(string text)
    {
        var ascii = Encoding.ASCII.GetBytes(text);
        const int ASCII_OFFSET = 33;
        const byte PADDING_ASCII = (byte)'u';
        var skipCount = 0;
        var result = new List<byte>(ascii.Length / 5 * 4);

        for (int i = 0; true; i++)
        {
            int j = i * 5 - skipCount * 4;
            if (j >= ascii.Length)
                break;
            if (ascii[j] == 'z')
            {
                result.Add(0);
                result.Add(0);
                result.Add(0);
                result.Add(0);
                skipCount++;
            }
            else
            {
                bool isLastLoop = (j >= ascii.Length - 5);
                uint block;
                int paddingCount = 0;
                if (!isLastLoop)
                {
                    block = (uint)(ascii[j] - ASCII_OFFSET) * 52200625 +
                            (uint)(ascii[j + 1] - ASCII_OFFSET) * 614125 +
                            (uint)(ascii[j + 2] - ASCII_OFFSET) * 7225 +
                            (uint)(ascii[j + 3] - ASCII_OFFSET) * 85 +
                            (uint)(ascii[j + 4] - ASCII_OFFSET);
                }
                else
                {
                    paddingCount = 5 - (ascii.Length - j);
                    block = (paddingCount == 0) ? (uint)(ascii[j] - ASCII_OFFSET) * 52200625 +
                        (uint)(ascii[j + 1] - ASCII_OFFSET) * 614125 + (uint)(ascii[j + 2] -
                        ASCII_OFFSET) * 7225 + (uint)(ascii[j + 3] - ASCII_OFFSET) * 85 +
                        (uint)(ascii[j + 4] - ASCII_OFFSET) :
                            (paddingCount == 1) ? (uint)(ascii[j] - ASCII_OFFSET) * 52200625 +
                            (uint)(ascii[j + 1] - ASCII_OFFSET) * 614125 + (uint)(ascii[j + 2] -
                            ASCII_OFFSET) * 7225 + (uint)(ascii[j + 3] - ASCII_OFFSET) * 85 +
                            (PADDING_ASCII - ASCII_OFFSET) :
                            (paddingCount == 2) ? (uint)(ascii[j] - ASCII_OFFSET) * 52200625 +
                            (uint)(ascii[j + 1] - ASCII_OFFSET) * 614125 + (uint)(ascii[j + 2] -
                            ASCII_OFFSET) * 7225 + (uint)(PADDING_ASCII - ASCII_OFFSET) * 85 +
                            (PADDING_ASCII - ASCII_OFFSET) :
                            (paddingCount == 3) ? (uint)(ascii[j] - ASCII_OFFSET) * 52200625 +
                            (uint)(ascii[j + 1] - ASCII_OFFSET) * 614125 + (uint)(PADDING_ASCII -
                            ASCII_OFFSET) * 7225 + (uint)(PADDING_ASCII - ASCII_OFFSET) * 85 +
                            (PADDING_ASCII - ASCII_OFFSET) :
                            (uint)(ascii[j] - ASCII_OFFSET) * 52200625 + (uint)(PADDING_ASCII -
                            ASCII_OFFSET) * 614125 + (uint)(PADDING_ASCII - ASCII_OFFSET) * 7225 + 
                            (uint)(PADDING_ASCII - ASCII_OFFSET) * 85 + (PADDING_ASCII - ASCII_OFFSET);
                }
                result.Add((byte)((block & 0xff000000) >> 24));
                if (!isLastLoop || paddingCount <= 2)
                    result.Add((byte)((block & 0x00ff0000) >> 16));
                if (!isLastLoop || paddingCount <= 1)
                    result.Add((byte)((block & 0x0000ff00) >> 8));
                if (!isLastLoop || paddingCount <= 0)
                    result.Add((byte)(block & 0x000000ff));
            }
        }

        return result.ToArray();
    }

}
