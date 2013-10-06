using System;
namespace ZumpaReader.WebService
{
    public interface IWebService
    {
        void DownloadItems(string url = null);
        event EventHandler<ZumpaReader_UnitTests.WebService.WSDownloadEventArgs> OnDownloadedItems;
        event EventHandler<ZumpaReader_UnitTests.WebService.WSErrorEventArgs> OnError;
    }
}
