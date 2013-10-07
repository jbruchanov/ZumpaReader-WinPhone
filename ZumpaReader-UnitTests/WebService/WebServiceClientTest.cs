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
                ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.NextPage.Length > 0);
                Assert.AreEqual(35, result.Context.Items.Count);
                client.DownloadItems(result.Context.NextPage).ContinueWith((e2) =>
                {
                    ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult> result2 = e2.Result;
                    Assert.IsNotNull(result2.Context.PreviousPage);
                    Assert.AreNotEqual(result.Context.NextPage, result2.Context.NextPage);
                    FinishWaiting();
                });
            });
            TestWait(10000);
        }
    }
}
