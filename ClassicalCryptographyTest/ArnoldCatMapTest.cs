using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class ArnoldCatMapTest
    {
        [TestMethod]
        [DataRow("0123456789ABCDEF", "0DA7B41E2F85963C")]
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                 "0VQLGBH61WRMSNC72X3YTID8E94ZOJPKFA5U")]
        [DataRow("WM.YE.FIFAI.RTCETYSAEOIKTMTTI.OA.LEA.YLNECHSDO.MUS.TLIO.V.HEENNE",
                 "W..S..IEKFMHTDYOLATA.ELOI.NTYIYEENOM.MS.AR.N.ELTETETFEUCSHAIOCIV")]
        public void TestArnoldCatMap(string plainText, string cipherText)
        {
            var cipher = new ArnoldCatMapCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText)[..plainText.Length]);
        }
    }
}