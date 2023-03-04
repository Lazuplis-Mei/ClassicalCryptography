using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptography.Interfaces
{
    /// <summary>
    /// 允许申请的最大栈空间
    /// </summary>
    public static class StackLimit
    {
        /// <summary>
        /// 最大栈长度
        /// </summary>
        public const int MaxSize = 1024;
        /// <summary>
        /// 最大栈字节长度
        /// </summary>
        public const int MaxByteSize = MaxSize / sizeof(byte);
        /// <summary>
        /// 最大栈字符长度
        /// </summary>
        public const int MaxCharSize = MaxSize / sizeof(char);
        /// <summary>
        /// 最大栈整数长度
        /// </summary>
        public const int MaxInt32Size = MaxSize / sizeof(int);
    }
}
