using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ZumpaReader.Commands
{
    public abstract class BaseCommand : ICommand
    {
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

        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
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

        public abstract void Execute(object parameter);        
    }
}
