using System;
namespace ZumpaReader.WebService
{
    public interface IWebService
    {
        void DownloadItems(string url = null);
        event EventHandler<WSDownloadEventArgs> DownloadedItems;
        event EventHandler<WSErrorEventArgs> Error;
    }
}
