using System.Security.Cryptography;
using System.Xml;

namespace ClassicalCryptography.Calculation.RSACryptography;

/// <summary>
/// 生成RSA密钥的方法
/// </summary>
public static class RSAHelper
{
    /// <summary>
    /// 默认的RSA指数
    /// </summary>
    public const int RSA_EXPONENT = 65537;

    /// <summary>
    /// 默认的RSA指数字符串形式
    /// </summary>
    public const string RSA_EXPONENT_STRING = "AQAB";

    private static readonly RSACryptoServiceProvider RSA_CSP = new();
    private static readonly XmlWriterSettings settings = new() { Indent = true, OmitXmlDeclaration = true };

    /// <summary>
    /// 生成可被破解的弱RSA私钥
    /// </summary>
    /// <param name="keySize">密钥长度</param>
    /// <returns>xml格式的RSA私钥</returns>
    [SkipLocalsInit]
    public static string GenerateWeakRSAPrivateKey(RSAKeySize keySize)
    {
        GuardEx.IsDefined(keySize);
        
        int size = (int)keySize;
        Span<byte> buffer = stackalloc byte[size];
        Random.Shared.NextBytes(buffer);
        var p = new BigInteger(buffer, true, true);
        Random.Shared.NextBytes(buffer[^(size / 4)..]);
        var q = new BigInteger(buffer, true, true);

        return GenerateRSAPrivateKey(p.FindPrime(), q.FindPrime());
    }

    /// <summary>
    /// 使用指定的质数生成RSA私钥
    /// </summary>
    /// <param name="p">质数P</param>
    /// <param name="q">质数Q</param>
    /// <returns>xml格式的RSA私钥</returns>
    public static string GenerateRSAPrivateKey(BigInteger p, BigInteger q)
    {
        var n = p * q;
        var d = MathEx.ModularInverse(RSA_EXPONENT, n - (p + q - 1));
        var xml = new StringBuilder(1000);
        using (var writer = XmlWriter.Create(xml, settings))
        {
            writer.WriteStartElement("RSAKeyValue");

            writer.WriteElement("Modulus", n);
            writer.WriteElement("Exponent", RSA_EXPONENT_STRING);
            writer.WriteElement("D", d);
            writer.WriteElement("P", p);
            writer.WriteElement("Q", q);
            writer.WriteElement("DP", d % (p - 1));
            writer.WriteElement("DQ", d % (q - 1));
            writer.WriteElement("InverseQ", MathEx.ModularInverse(q, p));

            writer.WriteEndElement();
        }
        return xml.ToString();
    }

    /// <summary>
    /// pem格式的转换为xml格式
    /// </summary>
    /// <param name="pemKey">pem格式的密钥</param>
    /// <returns>xml格式的密钥</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string PemToXml(this string pemKey)
    {
        RSA_CSP.ImportFromPem(pemKey);
        return RSA_CSP.ToXmlString(true);
    }

    /// <summary>
    /// xml格式的转换为pem格式
    /// </summary>
    /// <param name="xmlKey">xml格式的密钥</param>
    /// <param name="PKCS8Format">是否使用PKCS8格式</param>
    /// <returns>pem格式的密钥</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string XmlToPem(this string xmlKey, bool PKCS8Format = false)
    {
        RSA_CSP.FromXmlString(xmlKey);
        return PKCS8Format ? RSA_CSP.ExportPkcs8PrivateKeyPem() : RSA_CSP.ExportRSAPrivateKeyPem();
    }
}
