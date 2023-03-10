using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using ClassicalCryptography.Image;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;

namespace ClassicalCryptographyTest
{
    [TestClass]
    [SupportedOSPlatform("windows")]
    public class WeaveCipherTest
    {
        [TestMethod]
        public void TestWeaveCipher()
        {
            var text = "Good night";
            var bitmap = WeaveCipher.Encrypt(text);
            Assert.AreEqual(text, WeaveCipher.Decrypt(bitmap));
        }
    }
}
