using static ClassicalCryptography.Replacement.CommonTables;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 敲击码，使用<see cref="TapCode"/>加密
/// </summary>
[Introduction("敲击码", "使用一系列的点击声音来编码，但通常以数字形式表示")]
public class TapCodeCipher : ICipher<string, string>
{
    /// <summary>
    /// 替换密码
    /// </summary>
    public CipherType Type => CipherType.Substitution;

    /// <summary>
    /// 解密敲击码
    /// </summary>
    [SkipLocalsInit]
    public string Decrypt(string cipherText)
    {
        int size = cipherText.Length / 2;
        Span<char> result = size.CanAllocString() ? stackalloc char[size] : new char[size];

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
        foreach (var character in plainText)
        {
            int si = TapCode.KeyValue.Alphabet.IndexOf(character.ToUpperAscii());
            if (character == 'k' || character == 'K')
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
