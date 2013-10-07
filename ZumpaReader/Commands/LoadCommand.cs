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
    public class LoadCommand : ICommand
    {   
        public string LoadURL {get;set;}

        private IWebService _client;

        private bool _canExecute = true;

        private Action<ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult>> _callback;

        public LoadCommand(IWebService client, Action<ZumpaReader.WebService.WebService.ContextResult<ZumpaItemsResult>> resultCallback)
        {
            _client = client;
            _callback = resultCallback;            
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _canExecute = false;            

            _client.DownloadItems(LoadURL).ContinueWith( e =>
            {
                _canExecute = true;
                if(_callback != null){
                    _callback.Invoke(e.Result);
                }
            });
        }

        private void NotifyCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => CanExecuteChanged.Invoke(this, EventArgs.Empty));
            }
        }

    }
}
