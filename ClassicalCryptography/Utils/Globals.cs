[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ClassicalCryptographyTest")]

namespace ClassicalCryptography.Utils;

/// <summary>
/// 全局表与工具类
/// </summary>
public static class Globals
{
    /// <summary>
    /// 数字
    /// </summary>
    public static readonly string Digits = "0123456789";

    /// <summary>
    /// 日语片假名
    /// </summary>
    public static readonly string Hiragana = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん";

    /// <summary>
    /// 非标准的Base64形式，64进制
    /// </summary>
    public static readonly string VBase64 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";

    /// <summary>
    /// 数字和小写字母组成的36进制
    /// </summary>
    public static readonly string Base36 = "0123456789abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26个大写字母
    /// </summary>
    public static readonly string ULetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// 26个小写字母
    /// </summary>
    public static readonly string LLetters = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26个大写字母和26个小写字母
    /// </summary>
    public static readonly string ULLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26个大写字母和26个小写字母和数字
    /// </summary>
    public static readonly string ULLetterDigits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// 可打印ascii字符
    /// </summary>
    public static readonly string PrintableAsciis = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

}
