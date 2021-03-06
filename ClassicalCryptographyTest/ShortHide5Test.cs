using ClassicalCryptography.Calculation.ShortHide5;
using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var cipher = new ShortHide5();
            Assert.AreEqual(CipherType.Calculation, cipher.Type);

            Assert.AreEqual(plainText, cipher.Decrypt(cipherText));
        }
    }
}