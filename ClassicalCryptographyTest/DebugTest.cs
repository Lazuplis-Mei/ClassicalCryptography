using ClassicalCryptography.Calculation.CustomRSAPK;
using ClassicalCryptography.Encoder;
using ClassicalCryptography.Replacement;
using ClassicalCryptography.Sound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Numerics;
using static ClassicalCryptography.Encoder.PLEncodings.Constants;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class DebugTest
    {
        [TestMethod]
        public void Test()
        {
            var bf = PLEncoding.BrainfuckEncode("good job!");
            var str = PLEncoding.BrainfuckDecode(bf);

        }
    }
}
