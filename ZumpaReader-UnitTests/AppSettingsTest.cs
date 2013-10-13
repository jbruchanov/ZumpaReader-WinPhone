using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader;

namespace ZumpaReader_UnitTests
{
    [TestClass]
    public class AppSettingsTest : TestCase
    {
        [TestMethod]
        public void TestUser()
        {
            AppSettings.Login = "Login";
            Assert.AreEqual("Login", AppSettings.Login);
        }

        [TestMethod]
        public void TestNick()
        {
            AppSettings.ResponseName = "ResponseName";
            Assert.AreEqual("ResponseName", AppSettings.ResponseName);
        }

        [TestMethod]
        public void TestPassword()
        {
            AppSettings.Password = "Password";
            Assert.AreEqual("Password", AppSettings.Password);
        }

        [TestMethod]
        public void TestCookieString()
        {
            AppSettings.CookieString = "CookieString";
            Assert.AreEqual("CookieString", AppSettings.CookieString);
        }

        [TestMethod]
        public void TestIsLoggedIn()
        {
            AppSettings.IsLoggedIn = true;
            Assert.IsTrue(AppSettings.IsLoggedIn);
        }

        [TestMethod]
        public void TestGetNickIfNorResponseName()
        {
            AppSettings.Login = "NickOrResponseName";
            AppSettings.ResponseName = "";
            Assert.AreEqual("NickOrResponseName", AppSettings.NickOrResponseName);
        }

        [TestMethod]
        public void TestGetNickIfResponseName()
        {
            AppSettings.Login = "NickOrResponseName";
            AppSettings.ResponseName = "ResponseName";
            Assert.AreEqual("ResponseName", AppSettings.NickOrResponseName);
        }
    }
}
