using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ZumpaReader.Commands
{
    public class OpenLinkCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return !String.IsNullOrEmpty(parameter as string);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                return;
            }
            string url = Convert.ToString(parameter);
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(url, UriKind.Absolute);
            webBrowserTask.Show();
        }
    }
}
