[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ClassicalCryptographyTest")]

namespace ClassicalCryptography.Utils;

/// <summary>
/// 全局的字符表
/// </summary>
public static class GlobalTables
{
    /// <summary>
    /// 数字:<c>0123456789</c>
    /// </summary>
    public static readonly string Digits = "0123456789";

    /// <summary>
    /// 数字:<c>1234567890</c>
    /// </summary>
    public static readonly string VDigits = "1234567890";

    /// <summary>
    /// 小写16进制字符:<c>0123456789abcdef</c>
    /// </summary>
    public static readonly string HexString = "0123456789abcdef";

    /// <summary>
    /// 大写16进制字符:<c>0123456789ABCDEF</c>
    /// </summary>
    public static readonly string UHexString = "0123456789ABCDEF";

    /// <summary>
    /// 日语片假名
    /// </summary>
    public static readonly string Hiragana = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん";

    /// <summary>
    /// 64进制的字符:<c>0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/</c>
    /// </summary>
    public static readonly string VChar64 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";

    /// <summary>
    /// Base64字符:<c>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=</c>
    /// </summary>
    public static readonly string Base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    /// <summary>
    /// 36进制字符:<c>0123456789abcdefghijklmnopqrstuvwxyz</c>
    /// </summary>
    public static readonly string Base36 = "0123456789abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26个大写字母:<c>ABCDEFGHIJKLMNOPQRSTUVWXYZ</c>
    /// </summary>
    public static readonly string U_Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// 26个小写字母:<c>abcdefghijklmnopqrstuvwxyz</c>
    /// </summary>
    public static readonly string L_Letters = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 大写和小写字母:<c>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz</c>
    /// </summary>
    public static readonly string UL_Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 大写小写字母和数字:<c>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789</c>
    /// </summary>
    public static readonly string UL_Letter_Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// 可打印ascii字符(不包括空格)
    /// </summary>
    public static readonly string PrintableAsciis = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

    #region 这些相对来说更不常用

    /// <summary>
    /// Base32 RFC3548:<c>ABCDEFGHIJKLMNOPQRSTUVWXYZ234567</c>
    /// </summary>
    public static readonly string Base32_RFC3548 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    /// <summary>
    /// Base32 RFC4648:<c>0123456789ABCDEFGHIJKLMNOPQRSTUV</c>
    /// </summary>
    public static readonly string Base32_RFC4648 = "0123456789ABCDEFGHIJKLMNOPQRSTUV";

    /// <summary>
    /// z-base-32:<c>ybndrfg8ejkmcpqxot1uwisza345h769</c>
    /// </summary>
    public static readonly string Base32_Z = "ybndrfg8ejkmcpqxot1uwisza345h769";

    /// <summary>
    /// crockford-base32:<c>0123456789ABCDEFGHJKMNPQRSTVWXYZ</c>
    /// </summary>
    public static readonly string Base32_Crockford = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    /// <summary>
    /// Base36LD:<c>abcdefghijklmnopqrstuvwxyz0123456789</c>
    /// </summary>
    public static readonly string Base36_LD = "abcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// Base58:<c>123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz</c>
    /// </summary>
    public static readonly string Base58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

    /// <summary>
    /// Base45:<c>0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:</c>
    /// </summary>
    public static readonly string Base45 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";

    /// <summary>
    /// Base62:<c>0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz</c>
    /// </summary>
    public static readonly string Base62 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Base69
    /// </summary>
    public static readonly string Base69 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/-*<>|";

    /// <summary>
    /// Adobe ASCII85
    /// </summary>
    public static readonly string Ascii85 = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstu";

    /// <summary>
    /// <see href="https://rfc.zeromq.org/spec/32/">Ascii85_Z85</see>
    /// </summary>
    public static readonly string Ascii85_Z85 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";

    /// <summary>
    /// <see href="https://www.rfc-editor.org/rfc/rfc1924">Ascii85_IPv6(RFC1924)</see>
    /// </summary>
    public static readonly string Ascii85_IPv6 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!#$%&()*+-;<=>?@^_`{|}~";

    #endregion 这些相对来说更不常用
}
