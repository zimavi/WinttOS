using WinttOS.wSystem.Networking;

namespace WinttOSTests
{
    [TestClass]
    public class HttpsTests
    {

        string url = "http://winttos.localto.net/packages/hash.cexe";


        [TestMethod]
        public void ExtractDomainNameFromUrlTest()
        {
            string parsed = Http.ExtractDomainNameFromUrl(url);

            Console.WriteLine(parsed);

            Assert.AreEqual("winttos.localto.net", parsed);
        }

        [TestMethod]
        public void ExtractPathFromUrlTest()
        {
            string parsed = Http.ExtractPathFromUrl(url);

            Console.WriteLine(parsed);

            Assert.AreEqual("/packages/hash.cexe", parsed);
        }
    }
}