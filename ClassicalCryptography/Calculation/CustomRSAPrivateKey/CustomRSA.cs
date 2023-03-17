﻿using ClassicalCryptography.Interfaces;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ClassicalCryptography.Calculation.CustomRSAPrivateKey;

/// <summary>
/// 以指定的前缀字节生成质数并计算RSA私钥
/// </summary>
[Introduction("RSA隐写术", "以指定的前缀字节生成质数并计算RSA私钥")]
public static class CustomRSA
{
    /// <summary>
    /// RSA密钥长度
    /// </summary>
    public enum RSAKeySize
    {
        /// <summary>
        /// 1024位RSA
        /// </summary>
        RSA1024 = 64,
        /// <summary>
        /// 2048位RSA
        /// </summary>
        RSA2048 = 128,
        /// <summary>
        /// 3072位RSA
        /// </summary>
        RSA3072 = 192,
        /// <summary>
        /// 4096位RSA
        /// </summary>
        RSA4096 = 256,
    }

    #region 常数

    /// <summary>
    /// 短密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_SHORT = 1024;

    /// <summary>
    /// 中等密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_MEDIUM = 2048;

    /// <summary>
    /// 长密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_LONG = 3072;

    /// <summary>
    /// 最长密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_VERYLONG = 4096;

    /// <summary>
    /// 用于生成质数时预留的字节长度(只是粗略的估计)
    /// </summary>
    public const int REMAIN_BYTECOUNT = 5;

    /// <summary>
    /// 默认的RSA指数
    /// </summary>
    public const int RSA_EXPONENT = 65537;

    /// <summary>
    /// 默认的RSA指数字符串形式
    /// </summary>
    public const string RSA_EXPONENT_STRING = "AQAB";

    #endregion

    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    private static readonly RSACryptoServiceProvider rsa = new();

    /// <summary>
    /// 以指定前缀生成指定大小的质数
    /// </summary>
    /// <param name="prefix">前缀字节</param>
    /// <param name="rsaKeySize">密钥长度</param>
    /// <returns>质数</returns>
    [SkipLocalsInit]
    public static BigInteger GeneratePrime(Span<byte> prefix, RSAKeySize rsaKeySize)
    {
        Guard.IsTrue(Enum.IsDefined(rsaKeySize));
        int bytesCount = (int)rsaKeySize;
        Guard.HasSizeLessThanOrEqualTo(prefix, bytesCount - REMAIN_BYTECOUNT);
        Guard.IsFalse(prefix.Contains((byte)0));

        Span<byte> bigintBytes = bytesCount <= StackLimit.MaxByteSize
            ? stackalloc byte[bytesCount] : new byte[bytesCount];

        prefix.CopyTo(bigintBytes);
        bigintBytes[prefix.Length] = 0;//以0作为前缀字节的结尾标志
        Random.Shared.NextBytes(bigintBytes[(prefix.Length + 1)..]);
        bigintBytes[prefix.Length + 1] >>= 1;

        return new BigInteger(bigintBytes, true, true).NextPrime();
    }

    /// <summary>
    /// 将数值转换成base64编码
    /// </summary>
    /// <param name="number">要转换的数</param>
    /// <returns>base64编码</returns>
    public static string ToBase64(this BigInteger number)
    {
        return Convert.ToBase64String(number.ToByteArray(true, true));
    }

    /// <summary>
    /// 以指定的文本作为前缀生成RSA私钥
    /// </summary>
    /// <param name="text">作为前缀的文本</param>
    /// <param name="pemFormat">是否以pem格式导出</param>
    /// <returns>xml或pem格式的密钥</returns>
    public static string GenerateRSAPrivateKey(string text, bool pemFormat = false)
    {
        var prefix = Encoding.GetBytes(text).AsSpan();
        var prefix1 = prefix[..(prefix.Length >> 1)];
        var prefix2 = prefix[(prefix.Length >> 1)..];
        var xmlKey = GenerateRSAPrivateKey(prefix1, prefix2);
        return pemFormat ? XmlToPem(xmlKey) : xmlKey;
    }

    /// <summary>
    /// xml格式的转换为pem格式
    /// </summary>
    /// <param name="xmlKey">xml格式的密钥</param>
    public static string XmlToPem(string xmlKey)
    {
        rsa.FromXmlString(xmlKey);
        return rsa.ExportRSAPrivateKeyPem();
    }

    /// <summary>
    /// pem格式的转换为xml格式
    /// </summary>
    /// <param name="pemKey"></param>
    public static string PemToXml(string pemKey)
    {
        rsa.ImportFromPem(pemKey);
        return rsa.ToXmlString(true);
    }

    /// <summary>
    /// 从密钥中获得前缀字节
    /// </summary>
    /// <param name="privateKey">RSA私钥</param>
    public static (byte[], byte[]) GetPrifix(string privateKey)
    {
        var xmlDocument = new XmlDocument();
        try
        {
            xmlDocument.LoadXml(privateKey);
        }
        catch (XmlException)
        {
            xmlDocument.LoadXml(PemToXml(privateKey));
        }

        var keyValueNode = xmlDocument.FirstChild;
        Guard.IsNotNull(keyValueNode);
        Guard.IsEqualTo(keyValueNode.Name, "RSAKeyValue");

        string? base64P = keyValueNode.SelectSingleNode("P")?.InnerText;
        string? base64Q = keyValueNode.SelectSingleNode("Q")?.InnerText;
        Guard.IsNotNull(base64P);
        Guard.IsNotNull(base64Q);

        byte[] pBytes = Convert.FromBase64String(base64P);
        byte[] qBytes = Convert.FromBase64String(base64Q);

        int prifixEndofP = Array.IndexOf(pBytes, (byte)0);
        if (prifixEndofP == -1)
            prifixEndofP = pBytes.Length - REMAIN_BYTECOUNT;
        int prifixEndofQ = Array.IndexOf(qBytes, (byte)0);
        if (prifixEndofQ == -1)
            prifixEndofQ = qBytes.Length - REMAIN_BYTECOUNT;

        return (pBytes[..prifixEndofP], qBytes[..prifixEndofQ]);
    }

    /// <summary>
    /// 从密钥中获得文本前缀
    /// </summary>
    /// <param name="privateKey">RSA私钥</param>
    [SkipLocalsInit]
    public static string GetTextFrom(string privateKey)
    {
        var (prifixP, prifixQ) = GetPrifix(privateKey);
        int length = prifixP.Length + prifixQ.Length;
        Span<byte> prifix = length <= StackLimit.MaxByteSize
            ? stackalloc byte[length] : new byte[length];
        prifixP.CopyTo(prifix);
        prifixQ.CopyTo(prifix[prifixP.Length..]);
        return Encoding.GetString(prifix);
    }

    /// <summary>
    /// 以指定的前缀字节生成xml格式的RSA私钥
    /// </summary>
    /// <param name="prefix1">前缀字节1</param>
    /// <param name="prefix2">前缀字节1</param>
    public static string GenerateRSAPrivateKey(Span<byte> prefix1, Span<byte> prefix2)
    {
        int length = Math.Max(prefix1.Length, prefix2.Length) + REMAIN_BYTECOUNT;
        var rsaKeySize = (length << 4) switch
        {
            <= RSA_KEYSIZE_SHORT => RSAKeySize.RSA1024,
            > RSA_KEYSIZE_SHORT and <= RSA_KEYSIZE_MEDIUM => RSAKeySize.RSA2048,
            > RSA_KEYSIZE_MEDIUM and <= RSA_KEYSIZE_LONG => RSAKeySize.RSA3072,
            > RSA_KEYSIZE_LONG and <= RSA_KEYSIZE_VERYLONG => RSAKeySize.RSA4096,
            _ => throw new ArgumentException("前缀长度过长"),
        };

        var P = GeneratePrime(prefix1, rsaKeySize);
        var Q = GeneratePrime(prefix2, rsaKeySize);
        var Modulus = P * Q;
        var Exponent = RSA_EXPONENT;
        var D = ModularInverse(Exponent, Modulus - (P + Q - 1));
        var DP = D % (P - 1);
        var DQ = D % (Q - 1);
        var InverseQ = ModularInverse(Q, P);

        var xmlString = new StringBuilder();
        using (var writer = XmlWriter.Create(xmlString, new()
        {
            Indent = true,
            OmitXmlDeclaration = true
        }))
        {
            writer.WriteStartElement("RSAKeyValue");

            writer.WriteElement(nameof(Modulus), Modulus.ToBase64());
            writer.WriteElement(nameof(Exponent), RSA_EXPONENT_STRING);
            writer.WriteElement(nameof(D), D.ToBase64());
            writer.WriteElement(nameof(P), P.ToBase64());
            writer.WriteElement(nameof(Q), Q.ToBase64());
            writer.WriteElement(nameof(DP), DP.ToBase64());
            writer.WriteElement(nameof(DQ), DQ.ToBase64());
            writer.WriteElement(nameof(InverseQ), InverseQ.ToBase64());

            writer.WriteEndElement();
        }
        return xmlString.ToString();
    }

    private static void WriteElement(this XmlWriter writer, string name, string text)
    {
        writer.WriteStartElement(name);
        writer.WriteString(text);
        writer.WriteEndElement();
    }

    /// <summary>
    /// <para>详情请参考</para>
    /// <see href="https://www.geeksforgeeks.org/multiplicative-inverse-under-modulo-m/"/>
    /// </summary>
    private static BigInteger ModularInverse(BigInteger a, BigInteger n)
    {
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

}
