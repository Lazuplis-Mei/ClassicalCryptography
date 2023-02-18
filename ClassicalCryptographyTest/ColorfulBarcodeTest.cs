using ClassicalCryptography.Replacement;
using ClassicalCryptography.Undefined;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

            var bitmap = ColorfulBarcode.Encrypt(plainText);
            var text = ColorfulBarcode.Decrypt(bitmap);

            Assert.AreEqual(plainText, text);
        }
    }
}
