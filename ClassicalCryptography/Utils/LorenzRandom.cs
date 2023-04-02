namespace ClassicalCryptography.Utils;

internal class LorenzRandom : Random
{
    private readonly double rayleigh;
    private readonly double prandtl;
    private readonly double beta;
    private readonly double dt;
    private double x, y, z;
    public LorenzRandom(int seed)
    {
        var random = new Random(seed);
        x = random.NextDouble();
        y = random.NextDouble();
        z = random.NextDouble();
        rayleigh = 10.0;
        prandtl = 28.0;
        beta = 8.0 / 3.0;
        dt = 0.01;
    }

    public override int Next()
    {
        return (int)(Sample() * int.MaxValue);
    }

    public override int Next(int maxValue)
    {
        Guard.IsGreaterThan(maxValue, 0);
        return (int)(Sample() * maxValue);
    }

    public override int Next(int minValue, int maxValue)
    {
        Guard.IsTrue(minValue < maxValue);
        long range = (long)maxValue - minValue;
        return (int)(Sample() * range + minValue);
    }

    public override void NextBytes(byte[] buffer)
    {
        for (int i = 0; i < buffer.Length; i++)
            buffer[i] = (byte)Next(256);
    }

    public override double NextDouble()
    {
        return Sample();
    }

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
