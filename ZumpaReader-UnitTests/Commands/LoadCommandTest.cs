using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader;
using ZumpaReader.Commands;
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
            LoadCommand lc = new LoadCommand(mock.Object);
            Assert.IsTrue(lc.CanExecute(null));
        }

        [TestMethod]
        public void LoadCommandCantBeExecutedDuringLoading()
        {
            Mock<IWebService> mock = new Mock<IWebService>();
            LoadCommand lc = new LoadCommand(mock.Object);
            lc.Execute(null);
            Assert.IsFalse(lc.CanExecute(null));
        }

        [TestMethod]
        public void LoadCommandCallDownloadItemsWithParam()
        {
            string url = "http://test.com";
            Mock<IWebService> mock = new Mock<IWebService>();
            mock.Setup(e => e.DownloadItems(url)).Verifiable();
            LoadCommand lc = new LoadCommand(mock.Object);
            lc.LoadURL = url;
            lc.Execute(null);                                   
            mock.Verify();
        }

        [TestMethod]
        public void LoadCommandCallDownloadItemsAfterSuccessfulDownload()
        {
            Mock<IWebService> mock = new Mock<IWebService>();            
            LoadCommand lc = new LoadCommand(mock.Object);            
            lc.Execute(null);
            Assert.IsFalse(lc.CanExecute(null));
            
            mock.Raise( m => m.DownloadedItems += null, new WSDownloadEventArgs());
            
            Assert.IsTrue(lc.CanExecute(null));
        }

        [TestMethod]
        public void LoadCommandCallDownloadItemsAfterError()
        {
            Mock<IWebService> mock = new Mock<IWebService>();
            LoadCommand lc = new LoadCommand(mock.Object);
            lc.Execute(null);
            Assert.IsFalse(lc.CanExecute(null));

            mock.Raise(m => m.Error += null, new WSErrorEventArgs());

            Assert.IsTrue(lc.CanExecute(null));
        }
    }
}
