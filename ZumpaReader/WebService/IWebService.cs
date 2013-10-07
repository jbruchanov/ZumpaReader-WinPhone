using System;
using System.Threading.Tasks;
using ZumpaReader.Model;
namespace ZumpaReader.WebService
{
    public interface IWebService
    {
        Task<WebService.ContextResult<ZumpaItemsResult>> DownloadItems(string url = null);
        Task<WebService.ContextResult<string>> Login(string username, string password);
        Task<WebService.ContextResult<bool>> Logout();
    }
}
