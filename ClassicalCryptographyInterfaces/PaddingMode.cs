namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 当字符不足时，采用何种模式补充
/// </summary>
public enum PaddingMode
{
    /// <summary>
    /// 使用<see cref="TranspositionHelper.PaddingChar"/>
    /// </summary>
    SingleChar,
    /// <summary>
    /// 随机的10进制数字
    /// </summary>
    Digit,
    /// <summary>
    /// 随机的小写字母
    /// </summary>
    LowerLetter,
    /// <summary>
    /// 随机的大写字母
    /// </summary>
    UpperLetter
}
