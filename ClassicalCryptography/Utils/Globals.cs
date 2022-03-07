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
    /// 向上整除
    /// </summary>
    /// <param name="dividend">被除数</param>
    /// <param name="divisor">除数</param>
    public static int DivCeil(this int dividend, int divisor)
    {
        int quotient = dividend / divisor;
        quotient += dividend % divisor == 0 ? 0 : 1;
        return quotient;
    }

}
