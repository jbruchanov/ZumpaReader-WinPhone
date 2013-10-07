using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Phone.Testing;
using Moq;
using ZumpaReader;

namespace ZumpaReader_UnitTests
{
    [TestClass]
    public class ZumpaReaderResourcesTest : WorkItemTest
    {
        [TestMethod]
        public void TestLoad()
        {
            Assert.IsNotNull(ZumpaReaderResources.Instance);
        }

        [TestMethod]
        public void TestGetURL()
        {
            Assert.IsNotNull(ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL]);
        }

        [TestMethod]
        public void TestGetCredentials()
        {
            Assert.IsNotNull(ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login]);
            Assert.IsNotNull(ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password]);
        }
    }
}
