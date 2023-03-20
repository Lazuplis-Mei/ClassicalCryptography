using ClassicalCryptography.Calculation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class PerfectShuffleTest
    {
        [TestMethod]
        [DataRow("ABCDEFG")]
        [DataRow("TTTTTTTTTT")]
        [DataRow("ABCDE")]
        [DataRow("ABCDEEEF")]
        [DataRow("ABBBCDEEEFGGHIJJ")]
        public void TestPerfectShuffle(string text)
        {
            var encryptText = PerfectShuffle.Encrypt(text);
            Assert.AreEqual(text, PerfectShuffle.Decrypt(encryptText));
            encryptText = PerfectShuffle.Encrypt(text, true);
            Assert.AreEqual(text, PerfectShuffle.Decrypt(encryptText));
            encryptText = PerfectShuffle.EncryptShort(text);
            Assert.AreEqual(text, PerfectShuffle.Decrypt(encryptText));
        }

        [TestMethod]
        public void TestIt()
        {
            var encryptText = "..-/...--/---.-/---.-/---/---.-/.----/---/.-..-/-..-/---/----/";
            Assert.AreEqual("WHYTHISWORLD", PerfectShuffle.Decrypt(encryptText));
        }
    }
}
