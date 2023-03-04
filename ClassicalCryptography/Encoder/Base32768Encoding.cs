using System.Text;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// encoding optimised for UTF-16-encoded text <see href="https://github.com/qntm/base32768">Base32768</see>
/// </summary>
public static class Base32768Encoding
{
    private const int BITS_PER_CHAR = 15; // Base32768 is a 15-bit encoding
    private const int BITS_PER_BYTE = 8;
    private static readonly string[] pairStrings =
    {
        "ҠҿԀԟڀڿݠޟ߀ߟကဟႠႿᄀᅟᆀᆟᇠሿበቿዠዿጠጿᎠᏟᐠᙟᚠᛟកសᠠᡟᣀᣟᦀᦟ᧠᧿ᨠᨿᯀᯟᰀᰟᴀᴟ⇠⇿⋀⋟⍀⏟␀␟─❟➀➿⠀⥿⦠⦿⨠⩟⪀⪿⫠⭟ⰀⰟⲀⳟⴀⴟⵀⵟ⺠⻟㇀㇟㐀䶟䷀龿ꀀꑿ꒠꒿ꔀꗿꙀꙟꚠꛟ꜀ꝟꞀꞟꡀꡟ",
        "ƀƟɀʟ"
    };

    private static readonly BaseXXXXEncoding base32768 = new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);


    /// <summary>
    /// encode Base32768
    /// </summary>
    public static string Encode(byte[] uint8Array) => base32768.Encode(uint8Array);


    /// <summary>
    /// decode Base32768
    /// </summary>
    public static byte[] Decode(string str) => base32768.Decode(str);
    
}
