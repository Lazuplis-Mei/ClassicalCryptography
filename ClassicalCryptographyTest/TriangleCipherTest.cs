using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class TriangleCipherTest
    {
        [TestMethod]
        [DataRow("012345", "41502`3``")]
        [DataRow("qwertyuiop", "pt`wy`qeu`ri`o``")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZQ`JR`EKS`BFLT`ACGMU`DHNV`IOW`PX`Y``")]
        [DataRow("When I look at myself,I feel I'm so cute that I can't divert my eyes.",
                 "eaycneeu'syett.osle ` ke  d`hI lIti`We af'hv`nlt,mae`o I tr`m s t`foI `  m`cy` ``")]
        public void TestTriangle(string plainText, string cipherText)
        {
            var cipher = new TriangleCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText)[..plainText.Length]);
        }
    }

    [TestClass]
    public class MagicSquareTest
    {
        [TestMethod]
        [DataRow("123456789", "816357492")]
        [DataRow("0123456789ABCDEF", "0ED3B56879A4C21F")]
        public void TestMagicSquare(string plainText, string cipherText)
        {
            var cipher = new MagicSquareCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText)[..plainText.Length]);
        }
    }
}