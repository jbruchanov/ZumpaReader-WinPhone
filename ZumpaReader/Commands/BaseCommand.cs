using Coding4Fun.Toolkit.Controls;
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

        /// <summary>
        /// Show toast with error message
        /// </summary>
        /// <param name="e"></param>
        public void ShowError(Exception e)
        {
            ShowToast(e.Message);
        }

        /// <summary>
        /// Shoe Simple toast with wrapped text
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        public void ShowToast(string msg, string title = null)
        {
            new ToastPrompt { Title = title, Message = msg, TextWrapping = TextWrapping.Wrap }.Show();
        }

        /// <summary>
        /// Check if user is logged in, otherwise throws exception
        /// </summary>
        protected void EnsureLoggedIn()
        {
            if (!AppSettings.IsLoggedIn)
            {
                throw new Exception(Resources.Labels.NotLoggedIn);
            }
        }

        /// <summary>
        /// Check internet connection, Exception thrown if there is no connection
        /// </summary>
        protected void EnsureInternet()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                throw new Exception(Resources.Labels.NoInternet);
            }
        }
    }
}
