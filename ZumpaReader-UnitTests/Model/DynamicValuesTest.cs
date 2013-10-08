using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader;
using ZumpaReader.Model;

namespace ZumpaReader_UnitTests.Model
{
    [TestClass]
    public class DynamicValuesTest : TestCase
    {
        [TestMethod]
        public void TestFullDate()
        {
            ZumpaItem zi=  new ZumpaItem();
            zi.Time = 1381275411000L;

            string v = zi.ReadableDateTime;
            Assert.AreEqual("8.10.2013 00:36.51",v);
        }

        [TestMethod]
        public void TestShortDate()
        {
            ZumpaItem zi = new ZumpaItem();
            zi.Time = 76440000;

            string v = zi.ReadableDateTime;
            Assert.AreEqual("22:14", v);
        }

        [TestMethod]
        public void TestShortDateNegativeValue()
        {
            ZumpaItem zi = new ZumpaItem();
            zi.Time = -1440000;//36m -1H

            string v = zi.ReadableDateTime;
            Assert.AreEqual("00:36", v);
        }
    }
}
