namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// 中文拼音的声调
/// </summary>
public enum ChineseToneNote
{
    /// <summary>
    /// 没有声调
    /// </summary>
    None,

    /// <summary>
    /// 阴平
    /// </summary>
    LevelTone,

    /// <summary>
    /// 阳平
    /// </summary>
    RisingTone,

    /// <summary>
    /// 上声
    /// </summary>
    FallingRisingTone,

    /// <summary>
    /// 去声
    /// </summary>
    FallingTone,

    /// <summary>
    /// 轻声
    /// </summary>
    LightTone,
}
