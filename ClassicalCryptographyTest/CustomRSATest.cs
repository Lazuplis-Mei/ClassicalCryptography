using ClassicalCryptography.Calculation.CustomRSAPrivateKey;
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
        [DataRow(100), DataRow(200)/*, DataRow(300), DataRow(400)*/]
        public void TestCustomRSA(int textCount)
        {
            var text = RandomString.Generate(textCount);
            var pemkey = CustomRSA.GenerateRSAPrivateKey(text, true);
            var result = CustomRSA.GetTextFrom(pemkey);
            Assert.AreEqual(text, result);
        }

        [TestMethod]
        public void TestCustomRSASpecific()
        {
            var rsaPrivateKey = """
            -----BEGIN RSA PRIVATE KEY-----
            MIICXAIBAAKBgQCmg27sTTszEx7syiTW2ePP2yCSYhGM9S2Zu+p2rgwNE8pPA6fy
            T7SR4L+lENL45Ox1V1jMCu98u2YegOW5KtxMqOLkFJFKCH/pnPkR+YLdnESf6+TP
            gfTt5KeTDCQsR+UA7ybCEvGe/yVPIWHI09aAVHeYCo10DvdIgBR3qLa5FQIDAQAB
            AoGAMWcFXe58BedCYZaH34a95ElrHIMkGeIUKWxrX9K7mJWqjE7VYTrso+s+cMbR
            ES86SIYlvkPwUd13qs0rWwiwW0L7Smemok0SzHCOzB6shnsocAn90dh1vfTGNbVx
            xdPRDuGMpT5JSiTOjo8GeW2tRty5NbzeopnOtsNXaLj8agECQQDlpKnnqbrkuI3k
            vJrkuIDnm7Tpg73mmbTmnJfnmoTvvIzlgbblsJTkvJrkuIvkupvpm6jmu7TvvIzk
            AA8+1NGhAkEAuZ/mnInlkLnotbfmmrTpo47pm6jnmoTml7blgJnvvIzmma/oibLk
            vJrmuJDmuJDnmoTmlLnlj5jjgIIAPSfa9QJAMIJ8Mae1ByPFNZBx1+bxs/s8WCew
            Mco+YIirvgzyEAJwcPrZu+N1UGxW/AHP611QGiT7pOH58rqNuIwarFcggQJAOjAl
            2mxfyj589HnxkA2mL99c267W61k979EloGqg/DovD35VJtUtXvIv4SuLJ9BEyetf
            XL/pdlo29tHJwWH9AQJBANlcEuCVpgBAnM4b4h6p/YaqMqWA1t6/QZU2q9fNt3kq
            etVm9ejNhwaPuCwnHiWS+9XHM21M10R9fkDqxxRFV7Q=
            -----END RSA PRIVATE KEY-----
            """;

            var text = CustomRSA.GetTextFrom(rsaPrivateKey);

            Assert.AreEqual("天空不会一直都晴朗的，偶尔会下些雨滴，也有吹起暴风雨的时候，景色会渐渐的改变。", text);
        }

    }
}
