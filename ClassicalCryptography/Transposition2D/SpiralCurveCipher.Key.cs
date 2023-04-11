namespace ClassicalCryptography.Transposition2D;

public partial class SpiralCurveCipher
{
    /// <summary>
    /// 螺旋曲线密码的密钥
    /// </summary>
    [Introduction("螺旋曲线密码的密钥", "文本表示成指定宽度的矩形。")]
    public class Key : IKey<int>
    {
        /// <summary>
        /// 宽度分割密钥
        /// </summary>
        public Key(int width) => KeyValue = width;

        /// <inheritdoc/>
        public int KeyValue { get; }

        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;

        /// <summary>
        /// 密钥不可逆，将始终为null
        /// </summary>
        public IKey<int>? InversedKey => null;

        /// <inheritdoc/>
        public static IKey<int> FromString(string strKey)
        {
            return new Key(ushort.Parse(strKey));
        }

        /// <inheritdoc/>
        public static IKey<int> GenerateKey(int textLength)
        {
            int width = Random.Shared.Next(1, textLength.DivCeil(2));
            return new Key(width + 1);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength) => textLength >> 1;

        /// <inheritdoc/>
        public string GetString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override string ToString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => KeyValue.GetHashCode();
    }
}
