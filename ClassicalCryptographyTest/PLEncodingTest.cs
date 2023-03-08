using ClassicalCryptography.Encoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.Mime.MediaTypeNames;

namespace ClassicalCryptographyTest
{

    [TestClass]
    public class PLEncodingTest
    {
        [TestMethod]
        public void TestBrainfuck()
        {
            var text = "中文";
            string bfCode = PLEncoding.BrainfuckEncode(text);
            Assert.AreEqual(text, PLEncoding.BrainfuckDecode(bfCode));

            for (int i = 0; i < 10; i++)
            {
                text = RandomString.Generate(10);
                bfCode = PLEncoding.BrainfuckEncode(text);
                Assert.AreEqual(text, PLEncoding.BrainfuckDecode(bfCode));
            }
        }

        [TestMethod]
        public void TestPythonBytes()
        {
            for (int i = 0; i < 10; i++)
            {
                var text = RandomString.Generate(10);
                var pyBytes = PLEncoding.ToPythonBytes(text);
                Assert.AreEqual(text, PLEncoding.FromPythonBytes(pyBytes));
            }
        }
    }
}
