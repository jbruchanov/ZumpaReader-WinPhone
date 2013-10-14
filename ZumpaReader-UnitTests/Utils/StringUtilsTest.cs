using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader;
using ZumpaReader.Model;
using ZumpaReader.Utils;

namespace ZumpaReader_UnitTests.Utils
{
    [TestClass]
    public class StringUtilsTest : TestCase
    {
        [TestMethod]
        public void TestExtractID()
        {
            string url = "http://portal2.dkm.cz/phorum/read.php?f=2&i=1225378&t=1225378";
            string expected = "1225378";
            string value = StringUtils.ExtractThreadId(url);

            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void TestExtractIDNotLast()
        {
            string url = "http://portal2.dkm.cz/phorum/read.php?f=2&i=1225378&t=1225378&q=b";
            string expected = "1225378";
            string value = StringUtils.ExtractThreadId(url);

            Assert.AreEqual(expected, value);
        }
    }
}
