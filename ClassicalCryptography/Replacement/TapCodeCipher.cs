using ClassicalCryptography.Interfaces;
using System.Text;
using static ClassicalCryptography.Replacement.CommonTables;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 敲击码，使用<see cref="TapCode"/>加密
/// </summary>
public class TapCodeCipher : ICipher<string, string>
{
    /// <summary>
    /// 替换密码
    /// </summary>
    public CipherType Type => CipherType.Substitution;
 
    /// <summary>
    /// 解密敲击码
    /// </summary>
    public string Decrypt(string cipherText)
    {
        Span<char> result = stackalloc char[cipherText.Length >> 1];

        for (int i = 0; i < cipherText.Length; i++)
        {
            var row = TapCode.KeyValue.Padding.IndexOf(cipherText[i++]);
            var col = TapCode.KeyValue.Padding.IndexOf(cipherText[i]);
            result[i >> 1] = TapCode.KeyValue.Alphabet[row * 5 + col];
        }
        return new string(result);
    }

    /// <summary>
    /// 加密敲击码
    /// </summary>
    public string Encrypt(string plainText)
    {
        var strBuilder = new StringBuilder();
        foreach (var c in plainText)
        {
            int si = TapCode.KeyValue.Alphabet.IndexOf(char.ToUpper(c));
            if (c == 'k' || c == 'K')
                si = 2;
            if (si != -1)
            {
                strBuilder.Append(TapCode.KeyValue.Padding[si / 5]);
                strBuilder.Append(TapCode.KeyValue.Padding[si % 5]);
            }
        }
        return strBuilder.ToString();
    }
}
