using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassicalCryptography.Utils.Globals;

namespace ClassicalCryptography.Replacement
{
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
            ReflectionCharSet = $"{ULetters[key..]}{ULetters[..key]}";
            ReflectionCharSet += $"{LLetters[key..]}{LLetters[..key]}";
        }
        /// <summary>
        /// 英文字母
        /// </summary>
        public override string SupposedCharSet => ULLetters;

        /// <summary>
        /// 位移后的字母表
        /// </summary>
        public override string ReflectionCharSet { get; }
    }
}
