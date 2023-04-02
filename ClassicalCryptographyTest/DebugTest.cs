using ClassicalCryptography.Calculation;
using ClassicalCryptography.Calculation.RSASteganograph;
using ClassicalCryptography.Calculation.ShortHide5;
using ClassicalCryptography.Encoder;
using ClassicalCryptography.Encoder.BaseEncodings;
using ClassicalCryptography.Encoder.PLEncodings;
using ClassicalCryptography.Image;
using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using ClassicalCryptography.Sound;
using ClassicalCryptography.Transposition2D;
using ClassicalCryptography.Undefined;
using ClassicalCryptography.Utils;
using Microsoft.International.Converters.PinYinConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class DebugTest
    {
        [TestMethod]
        [SupportedOSPlatform("windows")]
        public void Test()
        {
            var 明文 = "想直接看答案？不存在的！";
            var UTF8 = Encoding.UTF8.GetBytes(明文);
            var SHA256数据 = Convert.ToHexString(SHA256.HashData(UTF8));
            Debug.WriteLine(SHA256数据);//该数据可用于验证答案

            var 加密数据 = PascalianPuzzleCipher.Transform(UTF8);//加密算法，你可以通过查看代码去理解
            var Base64 = Convert.ToBase64String(加密数据);
            Debug.WriteLine(Base64);//你真正需要破解的

        }
    }
}
