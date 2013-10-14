using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using ZumpaReader.Commands;
using ZumpaReader.Model;
using ZumpaReader.Pages;
using ZumpaReader.WebService;
using ZWS = ZumpaReader.WebService.WebService;

namespace ZumpaReader.ViewModel
{
    public class ThreadPageViewModel : BaseViewModel, ZumpaReader.Converters.BackgroundColorConverter.IGetIndexEvaluator
    {
        private const int RELOAD_INDEX = 0;
        private const int ADD_INDEX = 1;
        #region fields
        
        private string _pageTitle;

        public string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; NotifyPropertyChange(); }
        }
        
        private ObservableCollection<ZumpaSubItem> _dataItems;
        public ObservableCollection<ZumpaSubItem> DataItems
        {
            get { return _dataItems; }
            set { _dataItems = value; NotifyPropertyChange(); }
        }

        private bool _isProgressVisible;

        public bool IsProgressVisible
        {
            get { return _isProgressVisible; }
            set { _isProgressVisible = value; NotifyPropertyChange(); }
        }

        private HttpService _service;

        private string _threadUrl;

        public LoadThreadPageCommand LoadCommand { get; private set; }

        public ICommand OpenLinkCommand {get; private set;}
        #endregion

        public ThreadPageViewModel()
        {
            _service = HttpService.CreateInstance();
            
            OpenLinkCommand = new OpenLinkCommand();
            
            LoadCommand = new LoadThreadPageCommand(_service, (e) => Dispatcher.BeginInvoke(() => OnDownloadedPage(e.Context)));
            LoadCommand.CanExecuteChanged += (o, e) =>
            {
                bool can = LoadCommand.CanExecute(null);
                IsProgressVisible = !can;
                (Page.ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = can;
            };
            
            NotifyPropertyChange("BackColorConverter");
        }

        public override void OnPageAttached()
        {
            (Page.ApplicationBar.Buttons[RELOAD_INDEX] as ApplicationBarIconButton).Click += (o, e) => { LoadCommand.Execute(null); };
            (Page.ApplicationBar.Buttons[ADD_INDEX] as ApplicationBarIconButton).Click += (o, e) =>
            {
                Page.NavigationService.Navigate(new Uri("/ZumpaReader;component/Pages/SendPage.xaml", UriKind.RelativeOrAbsolute));
            };            
        }

        private void OnDownloadedPage(List<ZumpaSubItem> list)
        {
            DataItems = new ObservableCollection<ZumpaSubItem>(list);
        }

        public override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Bind();            
            (Page.ApplicationBar.Buttons[ADD_INDEX] as ApplicationBarIconButton).IsEnabled = AppSettings.IsLoggedIn;            
        }

        private void Bind()
        {            
            string title = null;
            if (Page.NavigationContext.QueryString.TryGetValue("title", out title))
            {
                PageTitle = title;
            }

            if (Page.NavigationContext.QueryString.TryGetValue("url", out _threadUrl))
            {
                _threadUrl = HttpUtility.UrlDecode(_threadUrl);
                LoadCommand.LoadURL = _threadUrl;
                LoadCommand.Execute(null);
            }        
        }

        public int GetIndex(object o)
        {            
            return DataItems == null ? 0 : DataItems.IndexOf(o as ZumpaSubItem);
        }
    }
}
