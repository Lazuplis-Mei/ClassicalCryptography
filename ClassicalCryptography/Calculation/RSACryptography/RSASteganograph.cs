using System.Xml;

namespace ClassicalCryptography.Calculation.RSACryptography;

/// <summary>
/// RSA隐写术
/// </summary>
/// <remarks>
/// 以指定的前缀字节生成质数并计算RSA私钥
/// </remarks>
[Introduction("RSA隐写术", "以指定的前缀字节生成质数并计算RSA私钥")]
public partial class RSASteganograph
{
    #region 常数

    /// <summary>
    /// 短密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_SHORT = 1024;

    /// <summary>
    /// 中等密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_MEDIUM = RSA_KEYSIZE_SHORT * 2;

    /// <summary>
    /// 长密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_LONG = RSA_KEYSIZE_SHORT * 3;

    /// <summary>
    /// 最长密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_VERYLONG = RSA_KEYSIZE_SHORT * 4;

    /// <summary>
    /// 用于生成质数时预留的字节长度
    /// </summary>
    public const int REMAIN_BYTESCOUNT = 5;

    /// <summary>
    /// 标记前缀字节的结尾
    /// </summary>
    public const byte PREFIX_END_FLAG = 0;

    /// <summary>
    /// 最大前缀字符字节数
    /// </summary>
    public const int MAX_PREFIX_SIZE = ((int)RSAKeySize.RSA4096 - REMAIN_BYTESCOUNT) * 2;

    #endregion 常数

    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 自定义算法
    /// </summary>
    public static CipherType Type => CipherType.Calculation;

    /// <summary>
    /// 以指定前缀生成指定RSA密钥长度的质数
    /// </summary>
    /// <remarks>
    /// 前缀字节中不能含有数值<see cref="PREFIX_END_FLAG"/>
    /// </remarks>
    /// <param name="prefix">前缀字节</param>
    /// <param name="keySize">密钥长度</param>
    [SkipLocalsInit]
    public static BigInteger GeneratePrime(ReadOnlySpan<byte> prefix, RSAKeySize keySize)
    {
        GuardEx.IsDefined(keySize);
        int size = (int)keySize;
        Guard.HasSizeLessThanOrEqualTo(prefix, size - REMAIN_BYTESCOUNT);
        GuardEx.ValueNotInSpan(PREFIX_END_FLAG, prefix);

        Span<byte> buffer = stackalloc byte[size];
        prefix.CopyTo(buffer);

        int prefixRegionLength = prefix.Length + 1;
        buffer[prefix.Length] = PREFIX_END_FLAG;
        Random.Shared.NextBytes(buffer[prefixRegionLength..]);

        //为了避免生成质数时，数值的增长覆盖了前缀字节结尾的标记，扩大了寻找质数的范围
        buffer[prefixRegionLength] /= 4;

        return new BigInteger(buffer, true, true).FindPrime();
    }

    /// <summary>
    /// 以指定的前缀字节生成RSA私钥
    /// </summary>
    /// <param name="prefixP">用于生成质数P的前缀字节</param>
    /// <param name="prefixQ">用于生成质数Q的前缀字节</param>
    /// <returns>xml格式的RSA私钥</returns>
    public static string GenerateRSAPrivateKey(ReadOnlySpan<byte> prefixP, ReadOnlySpan<byte> prefixQ)
    {
        int prefixRegionLength = Math.Max(prefixP.Length, prefixQ.Length) + REMAIN_BYTESCOUNT;
        var keySize = GetKeySize(prefixRegionLength);

        var p = GeneratePrime(prefixP, keySize);
        var q = GeneratePrime(prefixQ, keySize);
        return RSAHelper.GenerateRSAPrivateKey(p, q);
    }

    private static RSAKeySize GetKeySize(int size) => (size << 4) switch
    {
        <= RSA_KEYSIZE_SHORT => RSAKeySize.RSA1024,
        <= RSA_KEYSIZE_MEDIUM => RSAKeySize.RSA2048,
        <= RSA_KEYSIZE_LONG => RSAKeySize.RSA3072,
        <= RSA_KEYSIZE_VERYLONG => RSAKeySize.RSA4096,
        _ => throw new ArgumentOutOfRangeException(nameof(size), "前缀字节的长度过长"),
    };

    /// <summary>
    /// 以指定的文本作为前缀生成RSA私钥
    /// </summary>
    /// <remarks>
    /// <paramref name="text"/>使用<see cref="Encoding"/>转换成字节。<br/>
    /// 再对半分为两部分，分别作为质数P和Q的前缀字节。
    /// </remarks>
    /// <param name="text">作为前缀的文本</param>
    /// <param name="pemFormat">是否以pem格式导出</param>
    /// <returns>xml或pem格式的密钥</returns>
    [SkipLocalsInit]
    public static string GenerateRSAPrivateKey(string text, bool pemFormat = false)
    {
        int byteCount = Encoding.GetByteCount(text);
        Guard.IsLessThanOrEqualTo(byteCount, MAX_PREFIX_SIZE);

        Span<byte> prefix = stackalloc byte[byteCount];
        Encoding.GetBytes(text, prefix);
        int half = prefix.Length / 2;
        var xmlKey = GenerateRSAPrivateKey(prefix[..half], prefix[half..]);
        return pemFormat ? xmlKey.XmlToPem() : xmlKey;
    }

    /// <summary>
    /// 从密钥中获得前缀字节
    /// </summary>
    /// <remarks>
    /// 值得注意的是，如果前缀字节中包含<see cref="PREFIX_END_FLAG"/>，则获取的内容会提前截断。<br/>
    /// 如果整体都不包含<see cref="PREFIX_END_FLAG"/>，获取的内容按最长的可能截断。<br/>
    /// 你可以通过<see cref="ArraySegment{T}.Array"/>获得完整的数据。
    /// </remarks>
    /// <param name="privateKey">RSA私钥</param>
    /// <returns>质数P和Q中的前缀字节</returns>
    public static (ArraySegment<byte>, ArraySegment<byte>) GetPrifix(string privateKey)
    {
        Guard.IsNotNullOrWhiteSpace(privateKey);

        var xmlDocument = new XmlDocument();
        if (privateKey[0] == '<')
            xmlDocument.LoadXml(privateKey);
        else if (privateKey[0] == '-')
            xmlDocument.LoadXml(privateKey.PemToXml());
        else
        {
            try
            {
                xmlDocument.LoadXml(privateKey);
            }
            catch (XmlException)
            {
                xmlDocument.LoadXml(privateKey.PemToXml());
            }
        }

        var keyValueNode = xmlDocument.FirstChild;
        Guard.IsNotNull(keyValueNode);
        Guard.IsEqualTo(keyValueNode.Name, "RSAKeyValue");

        string? base64P = keyValueNode.SelectSingleNode("P")?.InnerText;
        Guard.IsNotNull(base64P);
        string? base64Q = keyValueNode.SelectSingleNode("Q")?.InnerText;
        Guard.IsNotNull(base64Q);

        byte[] pBytes = K4os.Text.BaseX.Base64.FromBase64(base64P);
        byte[] qBytes = K4os.Text.BaseX.Base64.FromBase64(base64Q);

        int prifixLengthP = pBytes.IndexOf(PREFIX_END_FLAG);
        if (prifixLengthP == -1)
            prifixLengthP = pBytes.Length - REMAIN_BYTESCOUNT;
        int prifixLengthQ = qBytes.IndexOf(PREFIX_END_FLAG);
        if (prifixLengthQ == -1)
            prifixLengthQ = qBytes.Length - REMAIN_BYTESCOUNT;

        return (pBytes.Subarray(0, prifixLengthP), qBytes.Subarray(0, prifixLengthQ));
    }

    /// <summary>
    /// 从密钥中获得前缀文本
    /// </summary>
    /// <remarks>
    /// 从<paramref name="privateKey"/>中获取前缀字节并组合。<br/>
    /// 再使用<see cref="Encoding"/>转换成文本。
    /// </remarks>
    /// <param name="privateKey">RSA私钥</param>
    /// <returns>P和Q中前缀字节组合而成的文本</returns>
    [SkipLocalsInit]
    public static string GetTextFrom(string privateKey)
    {
        var (prifixP, prifixQ) = GetPrifix(privateKey);
        int length = prifixP.Count + prifixQ.Count;
        using var memory = length.TryAlloc();
        Span<byte> prifix = length.CanAlloc() ? stackalloc byte[length] : memory.Span;
        prifixP.CopyTo(prifix);
        prifixQ.CopyTo(prifix, prifixP.Count);
        return Encoding.GetString(prifix);
    }
}
