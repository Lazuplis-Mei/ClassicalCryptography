namespace ClassicalCryptography.Utils;

/// <summary>
/// 一些数学计算的方法
/// </summary>
internal static class MathExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte MakeByte(byte high, byte low) => (byte)((high << 4) | low);

    /// <summary>
    /// 求模逆元
    /// </summary>
    /// <remarks>
    /// <see href="https://www.geeksforgeeks.org/multiplicative-inverse-under-modulo-m/">geeksforgeeks/multiplicative-inverse-under-modulo-m</see>
    /// </remarks>
    public static BigInteger ModularInverse(BigInteger a, BigInteger n)
    {
        GuardEx.IsPositive(a);
        GuardEx.IsPositive(n);

        BigInteger q, t, m = n, x = 1, y = 0;
        if (n.IsOne)
            return 0;
        while (a > 1)
        {
            q = a / n;
            t = n;
            n = a % n;
            a = t;
            t = y;
            y = x - q * y;
            x = t;
        }
        if (BigInteger.IsNegative(x))
            x += m;
        return x;
    }

    /// <summary>
    /// 向上整除
    /// </summary>
    /// <param name="dividend">被除数</param>
    /// <param name="divisor">除数</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DivCeil(this int dividend, int divisor)
    {
        var (quotient, remainder) = Math.DivRem(dividend, divisor);
        return remainder == 0 ? quotient : quotient + 1;
    }

    /// <summary>
    /// 距离整除还差多少
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DivPadding(this int dividend, int divisor)
    {
        int modulus = dividend % divisor, padding = 0;
        if (modulus != 0)
            padding = divisor - modulus;
        return padding;
    }

    /// <summary>
    /// 向上取平方根
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SqrtCeil(this int number)
    {
        return (int)Math.Ceiling(Math.Sqrt(number));
    }

    /// <summary>
    /// x的y次方
    /// </summary>
    public static ulong Power(uint x, int y)
    {
        Guard.IsGreaterThanOrEqualTo(y, 0);
        if (y == 0)
            return 1;

        ulong result = x;
        for (int i = 1; i < y; i++)
            result *= x;
        return result;
    }

    /// <summary>
    /// 整数N的分拆
    /// </summary>
    public static long PartitionsP(int n)
    {
        var arr = new long[n];
        arr[0] = 1;
        for (int i = 1; i < arr.Length; i++)
        {
            for (int j = 1, ri = 0; ; j++)
            {
                ri += 2 * j - 1;
                if (i < ri)
                    break;
                arr[i] += (j % 2 == 0) ? -arr[i - ri] : arr[i - ri];
                ri += j;
                if (i < ri)
                    break;
                arr[i] += (j % 2 == 0) ? -arr[i - ri] : arr[i - ri];
            }
        }
        return arr[n - 1];
    }

    /// <summary>
    /// 计算x的阶乘(朴素方法)
    /// </summary>
    public static BigInteger Factorial(int x)
    {
        var result = BigInteger.One;
        for (int i = 2; i <= x; i++)
            result *= i;
        return result;
    }
}
