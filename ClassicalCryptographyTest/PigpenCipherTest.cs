using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using ClassicalCryptography.Transposition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptographyTest
{
    [TestClass]
    [SupportedOSPlatform("windows")]
    public class PigpenCipherTest
    {
        [TestMethod]
        public void TestPigpenCipher()
        {
            var plainText = RandomString.GenerateUppers(100);

            var bitmap = PigpenCipher.Encrypt(plainText);
            var text = PigpenCipher.Decrypt(bitmap);
            
            Assert.AreEqual(plainText, text.Replace(Environment.NewLine, "").Trim());
        }
    }
}
