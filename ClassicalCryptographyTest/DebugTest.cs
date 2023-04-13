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
using ClassicalCryptography.Transposition;
using ClassicalCryptography.Transposition2D;
using ClassicalCryptography.Undefined;
using ClassicalCryptography.Utils;
using CommunityToolkit.HighPerformance;
using Microsoft.International.Converters.PinYinConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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
            //Stereogram.MakeWithPattern(new("E:/shark.png"),new("E:/jellybeans2.jpg")).Save("E:/stereogram.png");

        }
    }
}
