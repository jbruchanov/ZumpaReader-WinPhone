using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ZumpaReader.Model;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class LoadMainPageCommand : BaseLoadCommand
    {           
        private Action<ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult>> _callback;

        public LoadMainPageCommand(IWebService client, Action<ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult>> resultCallback) : base(client)
        {
            _callback = resultCallback;            
        }

        public override async void Execute(object parameter)
        {
            CanExecuteIt = false;
            ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult> result = null;
            try
            {
                EnsureInternet();
                result = await WebService.DownloadItems(LoadURL);
            }
            catch (Exception e)
            {
                ShowError(e);
            }

            CanExecuteIt = true;

            if (_callback != null && result != null)
            {
                _callback.Invoke(result);                
            }
               
        }
    }
}
