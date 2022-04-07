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
    /// 代换密码
    /// </summary>
    Replacement,
    /// <summary>
    /// 计算的
    /// </summary>
    Calculation,
    /// <summary>
    /// 未定义的(如输出图像的)
    /// </summary>
    Undefined,
}
