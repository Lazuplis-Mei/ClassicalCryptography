using ClassicalCryptography.Image;
using ClassicalCryptography.Replacement;
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

            var bitmap = ColorfulBarcode.Encode(plainText);
            var text = ColorfulBarcode.Recognize(bitmap);

            Assert.AreEqual(plainText, text);

            bitmap = ColorfulBarcode.EncodeSixColor(plainText);
            text = ColorfulBarcode.RecognizeSixColor(bitmap);

            Assert.AreEqual(plainText, text);

            //内部识别错误例`z3TlDHB8FXUQqoOE`
            plainText = "dsBp1IFXhf3zVNzo0LbYZmNMAEvkpG8KP9P2KykN/DK+r9EVz3TlDHB8FXUQqoOEcvuj7gt/PgJjBqRP0YUEO1P8ZuPJ/U9lnZ/E";
            bitmap = ColorfulBarcode.EncodeSixColor(plainText);
            text = ColorfulBarcode.RecognizeSixColor(bitmap);
            Assert.AreEqual(plainText, text);
        }
    }
}
