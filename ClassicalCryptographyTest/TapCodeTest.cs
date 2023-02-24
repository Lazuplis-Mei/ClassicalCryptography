using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class TapCodeTest
    {
        [TestMethod]
        [DataRow("TESTMETHOD", "44154344321544233414")]
        public void TestTapCode(string plainText, string cipherText)
        {
            var cipher = new TapCodeCipher();
            Assert.AreEqual(CipherType.Substitution, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText));
        }

        [TestMethod]
        [DataRow("KKKK", "13131313")]
        public void TestTapCodeK(string plainText, string cipherText)
        {
            var cipher = new TapCodeCipher();
            Assert.AreEqual(CipherType.Substitution, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual("CCCC", cipher.Decrypt(cipherText));
        }
    }
}
