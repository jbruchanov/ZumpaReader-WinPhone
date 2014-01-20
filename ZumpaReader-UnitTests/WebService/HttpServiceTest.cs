using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZumpaReader;
using ZumpaReader.Model;
using ZumpaReader.WebService;
using ZWS = ZumpaReader.WebService.WebService;

namespace ZumpaReader_UnitTests.WebService
{
    [TestClass]
    public class HttpServiceTest : TestCase
    {
        [Asynchronous]
        [TestMethod]
        public void TestRealGetItems()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            client.DownloadItems().ContinueWith((e) =>
            {
                ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.NextPage.Length > 0);
                Assert.AreEqual(35, result.Context.Items.Count);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        [Asynchronous]
        [TestMethod]
        public void TestRealGetItemsNextPage()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
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
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            string username = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            client.Login(username, password).ContinueWith((e) =>
            {
                ZWS.ContextResult<LoginResult> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.Result);
                Assert.IsTrue(result.Context.Cookies.Length > 0);
                Assert.IsTrue(result.Context.UID.Length > 0);
                Assert.IsTrue(result.Context.ZumpaResult.Length > 0);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        [Asynchronous]
        [TestMethod]
        public void TestLoginIncorrect()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            client.Login("X", "X").ContinueWith((e) =>
            {
                ZWS.ContextResult<LoginResult> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsFalse(result.Context.Result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        [Asynchronous]
        [TestMethod]
        public void TestLogoutCorrect()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            string username = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            string cookie = null;
            client.Login(username, password).ContinueWith((e) =>
            {
                cookie = e.Result.Context.Cookies;
                Assert.IsTrue(e.Result.Context.Result);
                Assert.IsTrue(e.Result.Context.Cookies.Length > 0);
                Assert.IsTrue(e.Result.Context.UID.Length > 0);
                Assert.IsTrue(e.Result.Context.ZumpaResult.Length > 0);
                Assert.IsTrue(cookie.Contains("portal_lln"));
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);

            client.Config.Cookies = cookie;

            client.Logout().ContinueWith((e) =>
            {
                bool result = e.Result.Context;
                Assert.IsTrue(result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        [Asynchronous]
        [TestMethod]
        public void TestDownloadSubItems()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            client.DownloadThread("http://portal2.dkm.cz/phorum/read.php?f=2&i=1206682&t=1206682").ContinueWith((e) =>
            {
                ZWS.ContextResult<List<ZumpaSubItem>> result = e.Result;
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Context.Count >= 3);//3 saw on web
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        [Asynchronous]
        [TestMethod]
        [Ignore]//manual test
        public void TestPostMessageToThread()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            client.Config.NickName = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string username = client.Config.NickName;
            string password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            string cookie = null;
            client.Login(username, password).ContinueWith((e) =>
            {
                Assert.IsTrue(e.Result.Context.Result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);

            client.Config.Cookies = cookie;

            client.SendMessage("SubjTest", "MsgTest", null, "1200532").ContinueWith((e) =>
            {
                ZWS.ContextResult<bool> result = e.Result;
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Context);
                FinishWaiting();
            });

            TestWait(DEFAULT_TIMEOUT);

            client.Logout().ContinueWith((e) =>
            {
                bool result = e.Result.Context;
                Assert.IsTrue(result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        [Asynchronous]
        [TestMethod]
        [Ignore]//manual test
        public void TestVoteSurvey()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            client.Config.NickName = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string username = client.Config.NickName;
            string password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            string cookie = null;
            client.Login(username, password).ContinueWith((e) =>
            {
                Assert.IsTrue(e.Result.Context.Result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);

            client.Config.Cookies = cookie;

            int sId = 3939;
            int sVote = 1;
            client.VoteSurvey(sId, sVote).ContinueWith((e) =>
            {
                ZWS.ContextResult<Survey> result = e.Result;
                Assert.IsNotNull(result);
                Assert.AreEqual(sVote, result.Context.VotedItem);
                FinishWaiting();
            });

            TestWait(DEFAULT_TIMEOUT);

            client.Logout().ContinueWith((e) =>
            {
                bool result = e.Result.Context;
                Assert.IsTrue(result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        [Asynchronous]
        [TestMethod]
        [Ignore]//manual test
        public void TestUploadImage()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            client.Config.NickName = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string username = client.Config.NickName;
            string password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            string cookie = null;
            client.Login(username, password).ContinueWith((e) =>
            {
                Assert.IsTrue(e.Result.Context.Result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);

            client.Config.Cookies = cookie;

            byte[] data = GenerateSimpleImage();
            client.UploadImage(data).ContinueWith((e) =>
            {
                ZWS.ContextResult<string> result = e.Result;
                Assert.IsNotNull(result);
                Assert.IsFalse(String.IsNullOrEmpty(result.Context));
                Assert.IsTrue(result.Context.Contains("http://www.q3.cz/images/"));
                FinishWaiting();
            });

            TestWait(DEFAULT_TIMEOUT);

            client.Logout().ContinueWith((e) =>
            {
                bool result = e.Result.Context;
                Assert.IsTrue(result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }

        private byte[] GenerateSimpleImage()
        {
            byte[] data = null;
            RunInMainThread(() =>
            {
                WriteableBitmap wb = new WriteableBitmap(200, 200);
                MemoryStream mem = new MemoryStream();
                wb.SaveJpeg(mem, wb.PixelWidth, wb.PixelHeight, 0, 100);
                data = mem.ToArray();
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
            return data;
        }

        [Asynchronous]
        [TestMethod]
        [Ignore]//manual test
        public void TestPushURIRegistration()
        {
            HttpService client = new HttpService();
            string username = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
            string uid = "test";
            string uri = "test";
            client.RegisterPushURI(username, uid, uri).ContinueWith((e) =>
            {
                bool result = e.Result;
                Assert.IsTrue(result);
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }


        [Asynchronous]
        [TestMethod]
        public void TestGetConfig()
        {
            HttpService client = new HttpService();
            client.Config.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            client.GetConfig().ContinueWith((e) =>
            {
                ZumpaReader.WebService.WebService.ContextResult<Dictionary<String, Object>> result = e.Result;
                Assert.IsNotNull(e);
                Assert.IsTrue(result.Context.ContainsKey("ShowSettingsAutoLoadImages"));
                Assert.IsTrue(result.Context.ContainsKey("ShowImageAsButton"));
                FinishWaiting();
            });
            TestWait(DEFAULT_TIMEOUT);
        }
    }
}
