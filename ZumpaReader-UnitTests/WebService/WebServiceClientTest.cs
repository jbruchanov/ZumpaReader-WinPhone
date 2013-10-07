using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZumpaReader;
using ZumpaReader.Model;
using ZumpaReader.WebService;
using ZWS = ZumpaReader.WebService.WebService;

namespace ZumpaReader_UnitTests.WebService
{
    [TestClass]
    public class WebServiceClientTest : TestCase
    {
        [Asynchronous]
        [TestMethod]
        public void TestRealGetItems()
        {            
            WebServiceClient client = new WebServiceClient();
            client.DownloadItems().ContinueWith ( (e) =>
            {                
                ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.NextPage.Length > 0);
                Assert.AreEqual(35, result.Context.Items.Count);
                FinishWaiting();
            });
            TestWait(5000);            
        }

        [Asynchronous]
        [TestMethod]
        public void TestRealGetItemsNextPage()
        {
            WebServiceClient client = new WebServiceClient();
            client.DownloadItems().ContinueWith((e) =>
            {
                ZWS.ContextResult<ZumpaItemsResult> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.NextPage.Length > 0);
                Assert.AreEqual(35, result.Context.Items.Count);
                client.DownloadItems(result.Context.NextPage).ContinueWith((e2) =>
                {
                    ZWS.ContextResult<ZumpaItemsResult> result2 = e2.Result;
                    Assert.IsNotNull(result2.Context.PreviousPage);
                    Assert.AreNotEqual(result.Context.NextPage, result2.Context.NextPage);
                    FinishWaiting();
                });
            });
            TestWait(10000);
        }

        [Asynchronous]
        [TestMethod]
        public void TestLoginCorrect()
        {
            WebServiceClient client = new WebServiceClient();
            string username = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            client.Login(username, password).ContinueWith((e) =>
            {
                ZWS.ContextResult<string> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.Length > 0);
                Assert.IsTrue(result.Context.Contains("portal_lln"));                
                FinishWaiting();
            });
            TestWait(5000);
        }

        [Asynchronous]
        [TestMethod]
        public void TestLoginIncorrect()
        {
            WebServiceClient client = new WebServiceClient();            
            client.Login("X", "X").ContinueWith((e) =>
            {
                ZWS.ContextResult<string> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.Length > 0);
                Assert.IsFalse(result.Context.Contains("portal_lln"));
                FinishWaiting();
            });
            TestWait(5000);
        }

        [Asynchronous]
        [TestMethod]
        public void TestLogoutCorrect()
        {
            WebServiceClient client = new WebServiceClient();
            string username = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            string cookie = null;
            client.Login(username, password).ContinueWith((e) =>
            {
                cookie = e.Result.Context;
                Assert.IsTrue(cookie.Contains("portal_lln"));
                FinishWaiting();
            });
            TestWait(5000);

            client = new WebServiceClient(cookie);
            client.Logout().ContinueWith((e) =>
            {
                bool result = e.Result.Context;
                Assert.IsTrue(result);
                FinishWaiting();
            });
            TestWait(5000);
        }

        [Asynchronous]
        [TestMethod]
        public void TestDownloadSubItems()
        {
            WebServiceClient client = new WebServiceClient();            
            client.DownloadThread("http://portal2.dkm.cz/phorum/read.php?f=2&i=1206682&t=1206682").ContinueWith((e) =>
            {
                ZWS.ContextResult<List<ZumpaSubItem>> result = e.Result;
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Context.Count >= 3);//3 saw on web
                FinishWaiting();
            });
            TestWait(5000);
        }
    }
}
