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
    public class AffineCipherTest
    {
        [TestMethod]
        [DataRow("abcdefghijklmnopqrstuvwxyz", "insxchmrwbglqvafkpuzejotyd", 5, 8)]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "KRYFMTAHOVCJQXELSZGNUBIPWD", 7, 10)]
        public void TestAffineCipher(string plainText, string cipherText, int a, int b)
        {
            var cipher = new AffineCipher(a, b);
            Assert.AreEqual(CipherType.Substitution, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText));
        }
    }
}
