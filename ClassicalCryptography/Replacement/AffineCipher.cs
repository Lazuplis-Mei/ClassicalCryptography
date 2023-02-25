using ClassicalCryptography.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassicalCryptography.Utils.Globals;

namespace ClassicalCryptography.Replacement
{
    /// <summary>
    /// 仿射密码
    /// </summary>
    [Introduction("仿射密码", "(a x + b) mod 26")]
    public class AffineCipher : SingleReplacementCipher
    {
        /// <summary>
        /// 仿射密码
        /// </summary>
        public AffineCipher(int a, int b)
        {
            if (a <= 0 || a >= 26 || b < 0 || b >= 26)
                throw new ArgumentException("参数范围不正确");
            if (a % 2 == 0 || a == 13)
                throw new ArgumentException("参数范围不正确", nameof(a));

            var arr = new ushort[26];
            for (int i = 0; i < 26; i++)
                arr[i] = (ushort)((a * i + b) % 26);
            ReflectionCharSet = arr.AssembleText(ULetters);
            ReflectionCharSet += arr.AssembleText(LLetters);
        }
        /// <summary>
        /// 英文字母
        /// </summary>
        public override string SupposedCharSet => ULLetters;

        /// <summary>
        /// 变化后的字母表
        /// </summary>
        public override string ReflectionCharSet { get; }
    }
}
