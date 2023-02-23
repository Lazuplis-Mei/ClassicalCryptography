using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using static ClassicalCryptography.Utils.Globals;

namespace ClassicalCryptography.Replacement
{
    /// <summary>
    /// 常见的单表和方阵
    /// </summary>
    public static partial class CommonTables
    {
        /// <summary>
        /// 敲击码(字母表没有K)
        /// </summary>
        public static readonly PolybiusSquare TapCode = new("12345", "ABCDEFGHIJLMNOPQRSTUVWXYZ");

        /// <summary>
        /// ADFGX(字母表没有J)
        /// </summary>
        public static readonly PolybiusSquare ADFGX = new("ADFGX", "BTALPDHOZKQFVSNGICUXMREWY");

    }
}
