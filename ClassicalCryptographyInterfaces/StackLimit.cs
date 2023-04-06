namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 允许申请的最大栈空间
/// </summary>
public static class StackLimit
{
    /// <summary>
    /// 最大栈长度
    /// </summary>
    public const int MAX_STACK_SIZE = 1024;

    /// <summary>
    /// 最大栈字节长度
    /// </summary>
    public const int MAX_BYTE_SIZE = MAX_STACK_SIZE / sizeof(byte);

    /// <summary>
    /// 最大栈字符长度
    /// </summary>
    public const int MAX_CHAR_SIZE = MAX_STACK_SIZE / sizeof(char);

    /// <summary>
    /// 最大栈整数长度
    /// </summary>
    public const int MAX_INT_SIZE = MAX_STACK_SIZE / sizeof(int);

    /// <summary>
    /// 最大栈短整数长度
    /// </summary>
    public const int MAX_SHORT_SIZE = MAX_STACK_SIZE / sizeof(short);

    /// <summary>
    /// 是否能申请栈空间
    /// </summary>
    /// <param name="bytesCount">要申请的字节数</param>
    public static bool CanAlloc(this int bytesCount) => bytesCount <= MAX_BYTE_SIZE;

    /// <summary>
    /// 是否能申请栈空间
    /// </summary>
    /// <param name="bytesCount">要申请的字节数</param>
    public static bool CanAllocInt16(this int bytesCount) => bytesCount <= MAX_SHORT_SIZE;

    /// <summary>
    /// 是否能申请栈空间
    /// </summary>
    /// <param name="bytesCount">要申请的字节数</param>
    public static bool CanAllocInt32(this int bytesCount) => bytesCount <= MAX_INT_SIZE;

    /// <summary>
    /// 是否能申请栈空间
    /// </summary>
    /// <param name="stringLength">要申请的字符串长度</param>
    public static bool CanAllocString(this int stringLength) => stringLength <= MAX_CHAR_SIZE;
}
