using ClassicalCryptography.Calculation.CustomRSAPK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class CustomRSATest
    {
        [TestMethod]
        public void TestCustomRSA()
        {
            for (int i = 0; i < 10; i++)
            {
                var text = RandomString.Generate(100);
                var pemkey = CustomRSA.FromStartText(text, true);
                var result = CustomRSA.GetTextFrom(pemkey, true);
                Assert.AreEqual(text, result);
            }
        }
    }
}
