using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
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
}