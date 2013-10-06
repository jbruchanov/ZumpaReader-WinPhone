using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class LoadCommand : ICommand
    {   
        public string LoadURL {get;set;}

        private IWebService _client;

        private bool _canExecute = true;

        public LoadCommand(IWebService client)
        {
            _client = client;
            _client.DownloadedItems += (o,e) => {_canExecute = true;};
            _client.Error += (o, e) => { _canExecute = true; };
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _canExecute = false;
            _client.DownloadItems(LoadURL);
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
