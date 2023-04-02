using ClassicalCryptography.Calculation.ShortHide5;
using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class ShortHide5Test
    {
        [TestMethod]
        [DataRow("D3r", "HERE")]
        [DataRow("Zh", "IS")]
        [DataRow("C4", "A")]
        [DataRow("S1kykW5f22", "QUESTION")]
        [DataRow("H246j5vidjU4rd21fftaT5z6swycuy", "When Pi is NOT 3,14")]
        public void TestShortHide5(string cipherText, string plainText)
        {
            Assert.AreEqual(CipherType.Calculation, ShortHide5.Type);
            Assert.AreEqual(plainText, ShortHide5.Decrypt(cipherText));
        }

        [TestMethod]
        public void TestShortHide5Generate()
        {
            for (int i = 0; i < 10; i++)
            {
                var text = RandomString.GenerateUppers(10);
                var cipherText = ShortHide5.Encrypt(text);
                var result = ShortHide5.Decrypt(cipherText);
                Assert.AreEqual(text, result);
            }
        }

        [TestMethod]
        public void TestShortHide5XXXXX()
        {
            var text = "XXXXX";
            var cipherText = ShortHide5.Encrypt(text);
            var result = ShortHide5.Decrypt(cipherText);
            Assert.AreEqual(text, result);
        }

    }
}