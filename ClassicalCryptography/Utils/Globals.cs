[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ClassicalCryptographyTest")]

namespace ClassicalCryptography.Utils;

/// <summary>
/// 全局的字符表
/// </summary>
public static class GlobalTables
{
    /// <summary>
    /// 数字
    /// </summary>
    public static readonly string Digits = "0123456789";

    /// <summary>
    /// 小写16进制字符
    /// </summary>
    public static readonly string HexString = "0123456789abcdef";

    /// <summary>
    /// 大写16进制字符
    /// </summary>
    public static readonly string UHexString = "0123456789ABCDEF";

    /// <summary>
    /// 日语片假名
    /// </summary>
    public static readonly string Hiragana = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん";

    /// <summary>
    /// 64进制的字符
    /// </summary>
    public static readonly string VChar64 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";
    
    /// <summary>
    /// base64字符
    /// </summary>
    public static readonly string Base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    /// <summary>
    /// 数字和小写字母组成的36进制
    /// </summary>
    public static readonly string Base36 = "0123456789abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26个大写字母
    /// </summary>
    public static readonly string U_Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// 26个小写字母
    /// </summary>
    public static readonly string L_Letters = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26个大写字母和26个小写字母
    /// </summary>
    public static readonly string UL_Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26个大写字母和26个小写字母和数字
    /// </summary>
    public static readonly string UL_Letter_Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// 可打印ascii字符(不包括空格)
    /// </summary>
    public static readonly string PrintableAsciis = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";


    #region 这些相对来说更不常用

    /// <summary>
    /// Base32 RFC3548
    /// </summary>
    public static readonly string Base32_RFC3548 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    /// <summary>
    /// Base32 RFC4648
    /// </summary>
    public static readonly string Base32_RFC4648 = "0123456789ABCDEFGHIJKLMNOPQRSTUV";

    /// <summary>
    /// z-base-32
    /// </summary>
    public static readonly string Base32_Z = "ybndrfg8ejkmcpqxot1uwisza345h769";

    /// <summary>
    /// crockford-base32
    /// </summary>
    public static readonly string Base32_Crockford = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    /// <summary>
    /// Base36 但先字母后数字
    /// </summary>
    public static readonly string Base36_LD = "abcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// Base58
    /// </summary>
    public static readonly string Base58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

    /// <summary>
    /// Base45
    /// </summary>
    public static readonly string Base45 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";

    /// <summary>
    /// Base62
    /// </summary>
    public static readonly string Base62 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Base69
    /// </summary>
    public static readonly string Base69 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/-*<>|";
    
    /// <summary>
    /// Ascii85
    /// </summary>
    public static readonly string Ascii85 = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstu";

    /// <summary>
    /// Ascii85_Z85
    /// </summary>
    public static readonly string Ascii85_Z85 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";

    /// <summary>
    /// Ascii85_IPv6
    /// </summary>
    public static readonly string Ascii85_IPv6 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`!#$%&()*+-;<=>?@^_ {|}~";


    #endregion
}
