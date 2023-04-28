namespace ClassicalCryptography.Replacement;

/// <summary>
/// 凯撒密码
/// </summary>
[Introduction("凯撒密码", "又称字母表位移密码。")]
public class CeaserCipher : SingleReplacementCipher
{
    /// <summary>
    /// 凯撒密码(默认位移3)
    /// </summary>
    public CeaserCipher(int key = 3)
    {
        ReflectionCharSet = $"{U_Letters[key..]}{U_Letters[..key]}";
        ReflectionCharSet += $"{L_Letters[key..]}{L_Letters[..key]}";
        BuildMap();
    }

    /// <summary>
    /// 英文字母
    /// </summary>
    public override string SupposedCharSet => UL_Letters;

    /// <summary>
    /// 位移后的字母表
    /// </summary>
    public override string ReflectionCharSet { get; }
}
