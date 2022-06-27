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
    /// 替换/代换密码(也包括输出图像)
    /// </summary>
    Substitution,
    /// <summary>
    /// 计算的
    /// </summary>
    Calculation,
    /// <summary>
    /// 未定义的
    /// </summary>
    Undefined,
}
