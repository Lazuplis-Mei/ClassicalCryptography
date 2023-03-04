using ClassicalCryptography.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 盲文编码
/// </summary>
public class BrailleEncoding : SingleReplacementCipher
{
    private static readonly string encodechar = "⠀⢀⠠⢠⠐⢐⠰⢰⠈⢈⠨⢨⠘⢘⠸⢸" +
        "⡀⣀⡠⣠⡐⣐⡰⣰⡈⣈⡨⣨⡘⣘⡸⣸⠄⢄⠤⢤⠔⢔⠴⢴⠌⢌⠬⢬⠜⢜⠼⢼⡄⣄⡤⣤⡔⣔⡴⣴⡌⣌⡬⣬⡜⣜⡼⣼" +
        "⠂⢂⠢⢢⠒⢒⠲⢲⠊⢊⠪⢪⠚⢚⠺⢺⡂⣂⡢⣢⡒⣒⡲⣲⡊⣊⡪⣪⡚⣚⡺⣺⠆⢆⠦⢦⠖⢖⠶⢶⠎⢎⠮⢮⠞⢞⠾⢾" +
        "⡆⣆⡦⣦⡖⣖⡶⣶⡎⣎⡮⣮⡞⣞⡾⣾⠁⢁⠡⢡⠑⢑⠱⢱⠉⢉⠩⢩⠙⢙⠹⢹⡁⣁⡡⣡⡑⣑⡱⣱⡉⣉⡩⣩⡙⣙⡹⣹" +
        "⠅⢅⠥⢥⠕⢕⠵⢵⠍⢍⠭⢭⠝⢝⠽⢽⡅⣅⡥⣥⡕⣕⡵⣵⡍⣍⡭⣭⡝⣝⡽⣽⠃⢃⠣⢣⠓⢓⠳⢳⠋⢋⠫⢫⠛⢛⠻⢻" +
        "⡃⣃⡣⣣⡓⣓⡳⣳⡋⣋⡫⣫⡛⣛⡻⣻⠇⢇⠧⢧⠗⢗⠷⢷⠏⢏⠯⢯⠟⢟⠿⢿⡇⣇⡧⣧⡗⣗⡷⣷⡏⣏⡯⣯⡟⣟⡿⣿";

    private static readonly Dictionary<char, byte> decodechar = new();
    static BrailleEncoding()
    {
        for (int i = 0; i < encodechar.Length; i++)
        {
            decodechar.Add(encodechar[i], (byte)i);
        }
    }

    /// <summary>
    /// encode braille
    /// </summary>
    [SkipLocalsInit]
    public static string Encode(byte[] bytes)
    {
        Span<char> span = bytes.Length <= StackLimit.MaxCharSize
            ? stackalloc char[bytes.Length] : new char[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
            span[i] = encodechar[bytes[i]];
        return new(span);
    }

    /// <summary>
    /// decode braille
    /// </summary>
    [SkipLocalsInit]
    public static byte[] Decode(string str)
    {
        var bytes = new byte[str.Length];
        for (int i = 0; i < str.Length; i++)
            bytes[i] = decodechar[str[i]];
        return bytes;
    }
}