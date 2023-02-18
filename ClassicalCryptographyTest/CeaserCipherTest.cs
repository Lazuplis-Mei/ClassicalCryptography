using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class CeaserCipherTest
    {
        [TestMethod]
        [DataRow("ABCDEFGHIJKL  MNOPQRSTUVWXYZ", "IJKLMNOPQRST  UVWXYZABCDEFGH", 8)]
        [DataRow("abcdefghijkl  mnopqrstuvwxyz", "ijklmnopqrst  uvwxyzabcdefgh", 8)]
        public void TestCeaserCipher(string plainText, string cipherText,int key)
        {
            var cipher = new CeaserCipher(key);
            Assert.AreEqual(CipherType.Substitution, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText));
        }

        [TestMethod]
        [DataRow("ROT5/13/18 is the easiest and yet powerful cipher!",
            "EBG5/13/18 vf gur rnfvrfg naq lrg cbjreshy pvcure!")]
        public void TestRot13(string plainText, string cipherText)
        {
            var cipher = CommonTables.Rot13;
            Assert.AreEqual(CipherType.Substitution, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText));
        }
    }
}
