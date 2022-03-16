using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptography.Utils;


/// <summary>
/// 全局表与工具类
/// </summary>
public static class Globals
{
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

}
