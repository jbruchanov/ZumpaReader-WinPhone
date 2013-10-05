using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Phone.Testing;
using Moq;

namespace ZumpaReader_UnitTests
{
    [TestClass]
    public class ProjectTest : WorkItemTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string q = "Q";
            Assert.AreEqual("Q",q);
        }

        [TestMethod]
        public void TestMoq()
        {
            Mock<MockTestIface> mock = new Mock<MockTestIface>();
            
            mock.Setup(x => x.GetName()).Returns("Pokus");
            mock.Setup(x => x.GetParam(It.IsAny<string>())).Returns("Q");
            
            Assert.AreEqual("Pokus", mock.Object.GetName());
            Assert.AreEqual("Q", mock.Object.GetParam("Q"));
            mock.Verify( x=> x.GetName(), Times.Exactly(1));            
        }


        public interface MockTestIface
        {
            string GetName();
            string GetParam(string param);
        }
    }
}
