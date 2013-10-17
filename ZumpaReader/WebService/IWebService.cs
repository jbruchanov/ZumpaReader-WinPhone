using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZumpaReader.Model;
namespace ZumpaReader.WebService
{
    public interface IWebService
    {
        Task<WebService.ContextResult<ZumpaItemsResult>> DownloadItems(string url = null);
        
        Task<WebService.ContextResult<LoginResult>> Login(string username, string password);
        
        Task<WebService.ContextResult<bool>> Logout();

        Task<WebService.ContextResult<List<ZumpaSubItem>>> DownloadThread(string url);

        Task<WebService.ContextResult<bool>> SendMessage(string subject, string message, string threadId = null);

        Task<WebService.ContextResult<Survey>> VoteSurvey(int id, int vote);

        Task<WebService.ContextResult<string>> UploadImage(byte[] data);

        Task<bool> RegisterPushURI(string username, string uid, string pushUrl);
    }
}
