using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ZumpaReader.Commands
{
    public class OpenLinkCommand : BaseCommand
    {
        private const string ZUMPA_PREFIX = "http://portal2.dkm.cz/phorum/read.php";

        public event EventHandler CanExecuteChanged;

        private NavigationService _navigationService;

        public OpenLinkCommand(NavigationService navService)
        {
            _navigationService = navService;
        }
        public override bool CanExecute(object parameter)
        {
            return !String.IsNullOrEmpty(parameter as string);
        }

        public override void Execute(object parameter)
        {
            if (parameter == null)
            {
                return;
            }
            try
            {
                EnsureInternet();
                string url = Convert.ToString(parameter);
                if (url.StartsWith(ZUMPA_PREFIX))
                {
                    url = String.Format("?url={0}", HttpUtility.UrlEncode(url));
                    _navigationService.Navigate(new Uri("/ZumpaReader;component/Pages/ThreadPage.xaml" + url, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    WebBrowserTask webBrowserTask = new WebBrowserTask();
                    webBrowserTask.Uri = new Uri(url, UriKind.Absolute);
                    webBrowserTask.Show();
                }
            }
            catch (Exception e)
            {
                ShowError(e);
            }                       
        }        
    }
}
