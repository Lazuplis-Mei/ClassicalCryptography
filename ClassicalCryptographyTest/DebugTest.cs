using ClassicalCryptography.Calculation.RSACryptography;
using ClassicalCryptography.Calculation.ShortHide5;
using ClassicalCryptography.Encoder;
using ClassicalCryptography.Encoder.BaseEncodings;
using ClassicalCryptography.Encoder.Chinese;
using ClassicalCryptography.Encoder.Japanese;
using ClassicalCryptography.Encoder.PLEncodings;
using ClassicalCryptography.Image;
using ClassicalCryptography.Sound;
using ClassicalCryptography.Transposition;
using ClassicalCryptography.Utils;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.International.Converters.PinYinConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class DebugTest
    {
        [TestMethod]
        public void Test()
        {
            //Stereogram.MakeWithPattern(new("E:/shark.png"),new("E:/jellybeans2.jpg")).Save("E:/stereogram.png");
            PhantomTank.Mirage(new Bitmap(@"D:\Foreground.png"), new Bitmap(@"D:\Background.png"), true).Save(@"D:\MirageTankGrayscale.png");
        }
    }
}
