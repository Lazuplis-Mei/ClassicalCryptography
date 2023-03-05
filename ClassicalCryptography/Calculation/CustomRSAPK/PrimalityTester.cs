using ClassicalCryptography.Utils;
using System.Numerics;

namespace ClassicalCryptography.Calculation.CustomRSAPK;

static class PrimalityTester
{
    /// <summary>
    /// Primality test
    /// </summary>
    public static bool IsPrime(this BigInteger num)
    {
        return IsPrime(num, 23) switch
        {

            0 => false,
            1 => true,//probability 1 - 1 / Math.Pow(4, 23)
            2 => true,
            _ => false,
        };
    }

    /// <summary>
    /// Primality test
    /// </summary>
    /// <param name="num">data</param>
    /// <param name="reps">Miller-Rabin loop</param>
    /// <returns>
    /// <para>0,n is not prime.</para>
    /// <para>1,n is 'probably' prime.</para>
    /// <para>2,n is surely prime.</para>
    /// </returns>
    private static int IsPrime(BigInteger num, int reps)
    {
        // Handle small and negative number
        if (num < 0)
            throw new ArgumentException("please input a positive integer!");
        if (num < 1000000UL)
            return IsPrimeUInt64((ulong)num) ? 2 : 0;

        // If number is now even, it is not a prime. 
        if (num.IsEven)
            return 0;

        // Check if n has small factors.
        if (num % 3 == 0 || num % 5 == 0 || num % 7 == 0
            || num % 11 == 0 || num % 13 == 0 || num % 17 == 0
            || num % 19 == 0 || num % 23 == 0 || num % 29 == 0
            || num % 31 == 0 || num % 37 == 0 || num % 41 == 0
            || num % 43 == 0 || num % 47 == 0 || num % 53 == 0)
            return 0;

        // Perform a number of Miller-Rabin tests.
        return MillerRabin(num, reps);
    }

    /// <summary>
    /// Primality test for number ∈[0, 1000000]
    /// </summary>
    private static bool IsPrimeUInt64(ulong n)
    {
        ulong q, r, d;

        // filter negative, even and 0,1,2
        if (n < 3 || (n & 1) == 0)
            return n == 2;

        for (d = 3, r = 1; r != 0; d += 2)
        {
            q = n / d;
            r = n - q * d;
            if (q < d)
                return true;
        }
        return false;
    }

    /// <summary>
    /// An implementation of the miller-rabin probabilistic primality test.
    /// </summary>
    /// <param name="n">input number</param>
    /// <param name="reps">loop count,default 23</param>
    /// <returns>
    /// <para>0,n is not prime.</para>
    /// <para>1,n is 'probably' prime.</para>
    /// </returns>
    private static int MillerRabin(BigInteger n, int reps)
    {
        BigInteger k, q, x, y, m = n - 1;

        // Perform a Fermat test. 210 = 2*3*5*7, magic ???
        y = BigInteger.ModPow(210, m, n);
        if (!y.IsOne)
            return 0; // n is not prime.

        // Find q and k, where q is odd and n - 1 = 2^k * q.
        for (k = 0, q = m; q.IsEven; k++, q >>= 1) ;

        int is_prime = 1;
        for (int r = 0; (r < reps) && (is_prime > 0); r++)
        {
            x = RandomHelper.RandomBigInteger(1, m);
            is_prime = MillerRabinInternal(n, x, q, k);
        }
        return is_prime;
    }

    private static int MillerRabinInternal(BigInteger n, BigInteger x, BigInteger q, BigInteger k)
    {
        BigInteger m = n - 1, y = BigInteger.ModPow(x, q, n);

        if (y == 1 || y == m)
            return 1;

        for (var i = 1; i < k; i++)
        {
            y = BigInteger.ModPow(y, 2, n);
            if (y == m)
                return 1;
            if (y == 1)
                return 0;
        }
        return 0;
    }

}
