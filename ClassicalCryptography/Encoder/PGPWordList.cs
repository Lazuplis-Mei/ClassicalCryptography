namespace ClassicalCryptography.Encoder;

/// <summary>
/// <see href="https://en.wikipedia.org/wiki/PGP-word-list">PGP单词表</see>
/// </summary>
/// <remarks>
/// 参考资料:<see href="http://www.asel.udel.edu/icslp/cdrom/vol1/005/a005.pdf"/>
/// </remarks>
public class PGPWordList : IEncoding
{
    private static readonly BidirectionalDictionary<byte, string> EvenWord;
    private static readonly BidirectionalDictionary<byte, string> OddWord;

    static PGPWordList()
    {
        EvenWord = new();
        OddWord = new();
        using var reader = new StreamReader(GZip.DecompressToStream(Resources.PGPWordList));
        for (byte i = 0; !reader.EndOfStream; i++)
        {
            string line = reader.ReadLine()!;
            int si = line.IndexOf(' ');
            EvenWord.Add(i, line[..si]);
            OddWord.Add(i, line[(si + 1)..]);
        }
    }

    /// <inheritdoc/>
    public static string Encode(byte[] bytes)
    {
        var result = new StringBuilder(bytes.Length * 8);
        bool evenFlag = false;
        foreach (var value in bytes)
        {
            if (evenFlag ^= true)
                result.Append(EvenWord[value]);
            else
                result.Append(OddWord[value]);
            result.Append(' ');
        }
        return result.RemoveLast().ToString();
    }

    /// <inheritdoc/>
    public static byte[] Decode(string encodeText)
    {
        var words = encodeText.Split(' ');
        var bytes = new byte[words.Length];
        var span = bytes.AsSpan();
        bool evenFlag = false;
        for (int i = 0; i < words.Length; i++)
        {
            if (evenFlag ^= true)
                span[i] = EvenWord.Inverse[words[i]];
            else
                span[i] = OddWord.Inverse[words[i]];
        }
        return bytes;
    }

}
