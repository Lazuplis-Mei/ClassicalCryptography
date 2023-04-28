using ClassicalCryptography.Image;
using ClassicalCryptography.Replacement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptographyTest
{
    [TestClass]
    [SupportedOSPlatform("windows")]
    public class ColorfulBarcodeTest
    {
        [TestMethod]
        public void TestColorfulBarcode()
        {
            var plainText = RandomString.Generate(100);
            Debug.WriteLine(plainText);

            var bitmap = ColorfulBarcode.Encode(plainText);
            var text = ColorfulBarcode.Recognize(bitmap);

            Assert.AreEqual(plainText, text);

            bitmap = ColorfulBarcode.EncodeSixColor(plainText);
            text = ColorfulBarcode.RecognizeSixColor(bitmap);

            Assert.AreEqual(plainText, text);

        }
    }
}
