using ClassicalCryptography.Undefined;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace ClassicalCryptographyTest;

[TestClass]
public class StringArtCipherTest
{
    [TestMethod]
    [DataRow("NICE", "blabopamledm/mdefvadopqklobm/bdfvomkqobvqb/tsihtabvfebolkqops")]
    [DataRow("1234", "cugecnlg/gvaehrklprsg/histhfdbvfmoqkmdki/hedmlhtubctsi")]
    public void TestStringArtCipher(string text, string stringArt)
    {
        Assert.AreEqual(stringArt, StringArtCipher.Encrypt(text));
        Assert.AreEqual(text.ToLower(), StringArtCipher.Decrypt(stringArt));
    }
}
