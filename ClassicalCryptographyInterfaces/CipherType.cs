namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 密码类型
/// </summary>
public enum CipherType
{
    /// <summary>
    /// 换位密码
    /// </summary>
    Transposition,

    /// <summary>
    /// 替换/代换密码
    /// </summary>
    Substitution,

    /// <summary>
    /// 计算的
    /// </summary>
    Calculation,

    /// <summary>
    /// 图形相关的
    /// </summary>
    Image,

    /// <summary>
    /// 声音相关的
    /// </summary>
    Sound,

    /// <summary>
    /// 未定义的
    /// </summary>
    Undefined,
}
