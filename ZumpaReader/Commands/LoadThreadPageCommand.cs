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
    public class LoadThreadPageCommand : BaseLoadCommand
    {           
        private Action<ZumpaReader.WebService.WebService.ContextResult<List<ZumpaSubItem>>> _callback;

        public LoadThreadPageCommand(IWebService client, Action<ZumpaReader.WebService.WebService.ContextResult<List<ZumpaSubItem>>> resultCallback)
            : base(client)
        {
            _callback = resultCallback;            
        }

        public async void Execute(object parameter)
        {
            CanExecuteIt = false;            
            var result = await WebService.DownloadThread(LoadURL);
            CanExecuteIt = true;        

            if (_callback != null)
            {
                _callback.Invoke(result);                
            }
               
        }
    }
}
