using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZumpaReader;
using ZumpaReader.Commands;
using ZumpaReader.Model;
using ZumpaReader.WebService;
using ZumpaReader_UnitTests.WebService;

namespace ZumpaReader_UnitTests.Commands
{
    [TestClass]
    public class LoadCommandTest : TestCase
    {
        [TestMethod]
        public void LoadCommandCanBeExecutedOnStart()
        {
            Mock<IWebService> mock = new Mock<IWebService>();            
            LoadCommand lc = new LoadCommand(mock.Object, null);
            Assert.IsTrue(lc.CanExecute(null));
        }

        [TestMethod]
        public void LoadCommandCantBeExecutedDuringLoading()
        {
            Mock<IWebService> mock = new Mock<IWebService>();
            mock.Setup(e => e.DownloadItems(It.IsAny<string>())).Returns(() =>
            {
                return new Task<ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult>>(() => null);
            });
            LoadCommand lc = new LoadCommand(mock.Object, (e) => {});
            lc.Execute(null);
            Assert.IsFalse(lc.CanExecute(null));
        }

        [TestMethod]
        public void LoadCommandCallDownloadItemsWithParam()
        {
            string url = "http://test.com";
            Mock<IWebService> mock = new Mock<IWebService>();
            mock.Setup(e => e.DownloadItems(It.IsAny<string>())).Returns(() =>
            {
                return new Task<ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult>>(() => null);
            }).Verifiable();
            LoadCommand lc = new LoadCommand(mock.Object, (e) => { });
            lc.LoadURL = url;
            lc.Execute(null);                                   
            mock.Verify( e=> e.DownloadItems(url));
        }

        [TestMethod]
        public void LoadCommandCallDownloadItemsAfterSuccessfulDownload()
        {
            var mock = new Mock<IWebService>();            
            mock.Setup(e => e.DownloadItems(null)).Returns(() =>
            {                
                var t = new Task<ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult>>( () => 
                {
                    Thread.Sleep(500);
                    return null;
                });
                t.Start();
                return t;
            });
            LoadCommand lc = new LoadCommand(mock.Object, (e) => {FinishWaiting();});            
            lc.Execute(null);
            Assert.IsFalse(lc.CanExecute(null));
            TestWait(DEFAULT_TIMEOUT);
            Assert.IsTrue(lc.CanExecute(null));
        }
    }
}
