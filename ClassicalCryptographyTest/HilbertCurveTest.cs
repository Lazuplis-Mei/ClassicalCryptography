using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class HilbertCurveTest
    {
        [TestMethod]
        [DataRow("0123456789ABCDEF", "03451276ED89FCBA")]
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 
                 "01EFGJKL32DCHINM478BUTOP569AVSRQ````WZ``````XY``````````````````")]
        [DataRow("Is there a special providence in the fall of a cock robin by me?",
                 "Isciaprot epl ivhe si deera necnb bo e fynirthla mk  al ?ecoc fo")]
        public void TestHilbertCurve(string plainText, string cipherText)
        {
            var cipher = new HilbertCurveCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText)[..plainText.Length]);
        }
    }
}