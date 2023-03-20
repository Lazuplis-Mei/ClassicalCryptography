using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Replacement
{
    /// <summary>
    /// 仿射密码
    /// </summary>
    [Introduction("仿射密码", "替换密码的一种，使用(a x + b) mod 26变换后的字母表")]
    public class AffineCipher : SingleReplacementCipher
    {
        /// <summary>
        /// 仿射密码
        /// </summary>
        public AffineCipher(int a, int b)
        {
            if (a <= 0 || a >= 26 || b < 0 || b >= 26)
                throw new ArgumentException("a和b的范围应该是[0,26)");
            if (a % 2 == 0 || a == 13)
                throw new ArgumentException("a应该是{1,3,5,7,9,11,15,17,19,21,23,25}中的一个", nameof(a));

            var arr = new ushort[26];
            for (int i = 0; i < 26; i++)
                arr[i] = (ushort)((a * i + b) % 26);
            ReflectionCharSet = arr.AssembleText(U_Letters);
            ReflectionCharSet += arr.AssembleText(L_Letters);
            BuildMap();
        }
        /// <summary>
        /// 英文字母
        /// </summary>
        public override string SupposedCharSet => UL_Letters;

        /// <summary>
        /// 变化后的字母表
        /// </summary>
        public override string ReflectionCharSet { get; }
    }
}
