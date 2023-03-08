using ClassicalCryptography.Interfaces;
using System.Text;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <see href="https://github.com/qntm/base32768"/>
/// </summary>
[Introduction("Base32768编码", "https://github.com/qntm/base32768")]
[TranslatedFrom("JavaScript")]
class BaseXXXXEncoding
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
                {
                    encodeRepertoire.Add((char)codePoint);
                }
            }
            int numZBits = BITS_PER_CHAR - BITS_PER_BYTE * r;
            lookupE.Add(numZBits, encodeRepertoire);
            for (int z = 0; z < encodeRepertoire.Count; z++)
            {
                char chr = encodeRepertoire[z];
                lookupD[chr] = (numZBits, z);
            }
        }
    }

    public string Encode(byte[] uint8Array)
    {
        int length = uint8Array.Length;
        var str = new StringBuilder();
        int z = 0;
        int numZBits = 0;
        for (int i = 0; i < length; i++)
        {
            byte uint8 = uint8Array[i];

            // Take most significant bit first
            for (int j = BITS_PER_BYTE - 1; j >= 0; j--)
            {
                int bit = uint8 >> j & 1;

                z = (z << 1) + bit;
                numZBits++;

                if (numZBits == BITS_PER_CHAR)
                {
                    str.Append(lookupE[numZBits][z]);
                    z = 0;
                    numZBits = 0;
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

            str.Append(lookupE[numZBits][z]);
        }
        return str.ToString();
    }

    public byte[] Decode(string str)
    {
        int length = str.Length;

        // This length is a guess. There's a chance we allocate one more byte here
        // than we actually need. But we can count and slice it off later
        var uint8Array = new byte[(length * BITS_PER_CHAR / BITS_PER_BYTE)];
        int numUint8s = 0;
        byte uint8 = 0;
        int numUint8Bits = 0;

        for (int i = 0; i < length; i++)
        {
            char chr = str[i];

            if (!lookupD.ContainsKey(chr))
            {
                throw new Exception($"Unrecognised {GetType().Name} character: ${chr}");
            }

            var (numZBits, z) = lookupD[chr];

            if (numZBits != BITS_PER_CHAR && i != length - 1)
            {
                throw new Exception("Secondary character found before end of input at position " + i.ToString());
            }

            // Take most significant bit first
            for (int j = numZBits - 1; j >= 0; j--)
            {
                int bit = z >> j & 1;

                uint8 = (byte)((uint8 << 1) + bit);
                numUint8Bits++;

                if (numUint8Bits == BITS_PER_BYTE)
                {
                    uint8Array[numUint8s] = uint8;
                    numUint8s++;
                    uint8 = 0;
                    numUint8Bits = 0;
                }
            }
        }

        // Final padding bits! Requires special consideration!
        // Remember how we always pad with 1s?
        // Note: there could be 0 such bits, check still works though
        if (uint8 != (1 << numUint8Bits) - 1)
        {
            throw new Exception("Padding mismatch");
        }

        return uint8Array[..numUint8s];
    }
}
