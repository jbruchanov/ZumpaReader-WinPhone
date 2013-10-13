using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public abstract class BaseLoadCommand
    {
        public string LoadURL { get; set; }

        public IWebService WebService { get; private set; }

        private bool _canExecute = true;        
        public bool CanExecuteIt
        {
            get
            {
                return _canExecute;
            }
            protected set
            {
                _canExecute = value;
                NotifyCanExecuteChanged();
            }
        }

        public BaseLoadCommand(IWebService service)
        {
            WebService = service;
        }

        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return CanExecuteIt;
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
