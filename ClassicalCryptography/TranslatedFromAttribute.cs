namespace ClassicalCryptography;

/// <summary>
/// 指示代码是由其他语言翻译而来的，可能不符合当前项目代码的编写规范
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class TranslatedFromAttribute : Attribute
{
    /// <summary>
    /// 代码的原始编程语言
    /// </summary>
    public string ProgramingLanguage { get; }

    /// <summary>
    /// 指示代码是由其他语言翻译而来的
    /// </summary>
    /// <param name="programingLanguage">代码的原始编程语言</param>
    public TranslatedFromAttribute(string programingLanguage)
    {
        ProgramingLanguage = programingLanguage;
    }
}