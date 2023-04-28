using ClassicalCryptography.Encoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class PGPWordListTest
    {
        [TestMethod]
        [DataRow("E58294F2E9A227486E8B061B31CC528FD7FA3F19",
            "topmost Istanbul Pluto vagabond treadmill Pacific brackish dictator goldfish Medusa afflict bravado chatter revolver Dupont midsummer stopwatch whimsical cowbell bottomless")]
        public void TestPGPWordList(string hex, string words)
        {
            var bytes = Convert.FromHexString(hex);
            var pgpWords = PGPWordList.Encode(bytes);
            var pgpBytes = PGPWordList.Decode(words);

            Assert.AreEqual(words, pgpWords);
            Assert.IsTrue(bytes.SequenceEqual(pgpBytes));
        }
    }
}
