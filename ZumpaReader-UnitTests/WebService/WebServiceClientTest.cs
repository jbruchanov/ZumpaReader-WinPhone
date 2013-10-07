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
            Exception ex = null;
            WebServiceClient client = new WebServiceClient();
            client.Error += (o, e) =>
            {
                ex = e.Error;
                FinishWaiting();
            };
            client.DownloadedItems += (object sender, WSDownloadEventArgs e) =>
            {
                Assert.IsNotNull(e.Result);
                Assert.IsTrue(e.Result.NextPage.Length > 0);
                Assert.AreEqual(35, e.Result.Items.Count);
                FinishWaiting();
            };
            client.DownloadItems();
            TestWait(5000);
            if (ex != null)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Asynchronous]
        [TestMethod]
        public void TestRealGetItemsNextPage()
        {
            Exception ex = null;
            WebServiceClient client = new WebServiceClient();
            client.Error += (o, e) =>
            {
                ex = e.Error;
                FinishWaiting();
            };
            int testStep = 1;
            ZumpaItemsResult first = null;
            client.DownloadedItems += (object sender, WSDownloadEventArgs e) =>
            {
                Assert.IsNotNull(e.Result);
                Assert.IsNotNull(e.Result.NextPage);

                if (testStep == 1)
                {
                    Assert.IsNull(e.Result.PreviousPage);
                    first = e.Result;
                    client.DownloadItems(e.Result.NextPage);
                    testStep++;
                }
                else if (testStep == 2)
                {
                    Assert.IsNotNull(e.Result.PreviousPage);
                    Assert.AreNotEqual(first.NextPage, e.Result.NextPage);
                    FinishWaiting();
                }
            };
            client.DownloadItems();
            TestWait(10000);
            if (ex != null)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
