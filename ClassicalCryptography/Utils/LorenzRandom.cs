namespace ClassicalCryptography.Utils;

/// <summary>
/// 洛伦兹混沌随机数
/// </summary>
public class LorenzRandom : Random
{
    private readonly double rayleigh;
    private readonly double prandtl;
    private readonly double beta;
    private readonly double dt;
    private double x, y, z;

    /// <inheritdoc/>
    public LorenzRandom(int seed)
    {
        var random = new Random(seed);
        x = random.NextDouble();
        y = random.NextDouble();
        z = random.NextDouble();
        rayleigh = 10.0;
        prandtl = 28.0;
        beta = 8.0 / 3.0;
        dt = 0.005;
    }

    /// <inheritdoc/>
    public override int Next() => (int)(Sample() * int.MaxValue);

    /// <inheritdoc/>
    public override int Next(int maxValue)
    {
        Guard.IsGreaterThan(maxValue, 0);
        return (int)(Sample() * maxValue);
    }

    /// <inheritdoc/>
    public override int Next(int minValue, int maxValue)
    {
        Guard.IsTrue(minValue < maxValue);
        long range = (long)maxValue - minValue;
        return (int)(Sample() * range + minValue);
    }

    /// <inheritdoc/>
    public override void NextBytes(byte[] buffer)
    {
        for (int i = 0; i < buffer.Length; i++)
            buffer[i] = (byte)Next(256);
    }

    /// <inheritdoc/>
    public override double NextDouble() => Sample();

    /// <inheritdoc/>
    public override void NextBytes(Span<byte> buffer)
    {
        for (int i = 0; i < buffer.Length; i++)
            buffer[i] = (byte)Next(256);
    }

    /// <inheritdoc/>
    public override float NextSingle() => (float)Sample();

    /// <inheritdoc/>
    public override long NextInt64() => (long)(Sample() * long.MaxValue);

    /// <inheritdoc/>
    public override long NextInt64(long maxValue)
    {
        Guard.IsGreaterThan(maxValue, 0);
        return (long)(Sample() * maxValue);
    }

    /// <inheritdoc/>
    public override long NextInt64(long minValue, long maxValue)
    {
        Guard.IsTrue(minValue < maxValue);
        long range = maxValue - minValue;
        return (long)(Sample() * range + minValue);
    }

    /// <inheritdoc/>
    protected override double Sample()
    {
        double dx = rayleigh * (y - x);
        double dy = x * (prandtl - z) - y;
        double dz = x * y - beta * z;
        x += dx * dt;
        y += dy * dt;
        z += dz * dt;
        return x - Math.Floor(x);
    }
}
