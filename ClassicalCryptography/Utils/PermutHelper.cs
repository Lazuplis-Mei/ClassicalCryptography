using static ClassicalCryptography.Utils.MathExtension;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 组合工具类
/// </summary>
internal static class PermutHelper
{

    /// <summary>
    /// 获得<paramref name="count"/>个数的第n个排列
    /// </summary>
    /// <param name="n">第n个排列</param>
    /// <param name="count">排列的数的个数</param>
    public static ushort[] GetPermutation(BigInteger n, int count)
    {
        var result = new ushort[count];
        var array = new ushort[count + 1];
        array.FillOrder();
        var list = array.ToList();
        for (int i = 0; i < count; i++)
        {
            result[i] = list[(int)BigInteger.DivRem(n, Factorial(count - i - 1), out n)];
            list.Remove(result[i]);
        }
        return result;
    }

    /// <summary>
    /// 填充span为第n个排列
    /// </summary>
    public static void FillPermutation(this Span<ushort> span, BigInteger number)
    {
        var arr = new ushort[span.Length + 1];
        arr.FillOrder();
        var list = arr.ToList();
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = list[(int)BigInteger.DivRem(number, Factorial(span.Length - i - 1), out number)];
            list.Remove(span[i]);
        }
    }

    /// <summary>
    /// 获得一个排列的序号
    /// </summary>
    public static BigInteger GetPermutIndex(ushort[] permute)
    {
        var result = BigInteger.Zero;
        for (int i = 0; i < permute.Length - 1; i++)
        {
            int t = permute.Length - i - 1;
            result += permute.TakeLast(t).Count(j => j < permute[i]) * Factorial(t);
        }
        return result;
    }

}
