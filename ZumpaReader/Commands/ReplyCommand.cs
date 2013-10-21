using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using System.Windows.Navigation;
using ZumpaReader.Model;
using ZumpaReader.ViewModel;

namespace ZumpaReader.Commands
{
    public class ReplyCommand : BaseCommand
    {

        private NavigationService _navigationService;
        private string _subject;
        private string _threadId;

        public ReplyCommand(string threadId, string subject, NavigationService service)
        {
            _threadId = threadId;
            _subject = subject;
            _navigationService = service;
            CanExecuteIt = true;
        }

        public override void Execute(object parameter)
        {
            var item = parameter as ZumpaSubItem;
            try
            {
                EnsureLoggedIn();
                EnsureInternet();
                string url = String.Format("?{0}={1}&{2}={3}", PostPageViewModel.THREAD_ID, _threadId, PostPageViewModel.SUBJECT, HttpUtility.UrlEncode(_subject));
                if (item != null)
                {
                    url = url + String.Format("&{0}={1}", PostPageViewModel.REPLY_TO, HttpUtility.UrlEncode(item.AuthorReal));
                }
                _navigationService.Navigate(new Uri("/ZumpaReader;component/Pages/PostPage.xaml" + url, UriKind.RelativeOrAbsolute));
            }
            catch (Exception e)
            {
                ShowError(e);
            }
        }
    }
}
