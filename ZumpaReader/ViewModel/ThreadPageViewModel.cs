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
using ZumpaReader.Utils;
using ZumpaReader.WebService;
using ZWS = ZumpaReader.WebService.WebService;

namespace ZumpaReader.ViewModel
{
    public class ThreadPageViewModel : BaseViewModel, ZumpaReader.Converters.BackgroundColorConverter.IGetIndexEvaluator
    {
        private const int FAV_INDEX = 0;
        private const int RELOAD_INDEX = 1;
        private const int ADD_INDEX = 2;
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
        private string _threadId;

        public LoadThreadPageCommand LoadCommand { get; private set; }

        public ICommand OpenLinkCommand { get; private set; }

        public ReplyCommand ReplyCommand { get; private set; }

        private SwitchFavoriteThreadCommand _switchFavoriteThreadCommand;

        private bool _isFavorite = false;
        #endregion

        public ThreadPageViewModel()
        {
            _service = HttpService.CreateInstance();

            _switchFavoriteThreadCommand = new SwitchFavoriteThreadCommand(_service, (result) => { if (result) { _isFavorite = !_isFavorite; ReinitFavoriteButton(); } });
            _switchFavoriteThreadCommand.CanExecuteChanged += (o, e) => { 
                IsProgressVisible = !_switchFavoriteThreadCommand.CanExecuteIt;
                (Page.ApplicationBar.Buttons[FAV_INDEX] as ApplicationBarIconButton).IsEnabled = _switchFavoriteThreadCommand.CanExecuteIt;
            };

            LoadCommand = new LoadThreadPageCommand(_service, (e) => Dispatcher.BeginInvoke(() => OnDownloadedPage(e.Context)));
            LoadCommand.CanExecuteChanged += (o, e) =>
            {
                IsProgressVisible = !LoadCommand.CanExecuteIt;
                (Page.ApplicationBar.Buttons[RELOAD_INDEX] as ApplicationBarIconButton).IsEnabled = LoadCommand.CanExecuteIt;
            };

            NotifyPropertyChange("BackColorConverter");
        }

        public override void OnPageAttached()
        {
            (Page.ApplicationBar.Buttons[RELOAD_INDEX] as ApplicationBarIconButton).Click += (o, e) => { LoadCommand.Execute(null); };
            (Page.ApplicationBar.Buttons[ADD_INDEX] as ApplicationBarIconButton).Click += (o, e) =>
            {
                ReplyCommand.Execute(null);
            };
        }

        private void OnDownloadedPage(List<ZumpaSubItem> list)
        {
            DataItems = new ObservableCollection<ZumpaSubItem>(list);
        }

        public override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Bind();
            OpenLinkCommand = new OpenLinkCommand(Page.NavigationService);
            (Page.ApplicationBar.Buttons[FAV_INDEX] as ApplicationBarIconButton).IsEnabled = AppSettings.IsLoggedIn;
            (Page.ApplicationBar.Buttons[ADD_INDEX] as ApplicationBarIconButton).IsEnabled = AppSettings.IsLoggedIn;
        }

        private void Bind()
        {
            string title = null;
            if (Page.NavigationContext.QueryString.TryGetValue("title", out title))
            {
                PageTitle = title;
            }
            else
            {
                PageTitle = Resources.Labels.AppName;
            }

            if (Page.NavigationContext.QueryString.TryGetValue("url", out _threadUrl))
            {
                _threadUrl = HttpUtility.UrlDecode(_threadUrl);
                LoadCommand.LoadURL = _threadUrl;
                LoadCommand.Execute(null);
            }

            string fav = null;
            if (Page.NavigationContext.QueryString.TryGetValue("favorite", out fav))
            {
                _isFavorite = Convert.ToBoolean(fav);
            }

            if (Page.NavigationContext.QueryString.TryGetValue("ThreadID", out _threadId)) //push style notification
            {
                _threadUrl = String.Format("http://portal2.dkm.cz/phorum/read.php?f=2&i={0}&t={0}", _threadId);
                LoadCommand.LoadURL = _threadUrl;
                LoadCommand.Execute(null);
            }
            else
            {
                _threadId = Utils.StringUtils.ExtractThreadId(_threadUrl);
            }

            var favButton = (Page.ApplicationBar.Buttons[FAV_INDEX] as ApplicationBarIconButton);
            ReinitFavoriteButton();
            favButton.Click += (o, e) => { _switchFavoriteThreadCommand.Execute(_threadId); };

            ReplyCommand = new ReplyCommand(StringUtils.ExtractThreadId(_threadUrl), PageTitle, Page.NavigationService);
        }

        private void ReinitFavoriteButton()
        {
            var favButton = (Page.ApplicationBar.Buttons[FAV_INDEX] as ApplicationBarIconButton);
            favButton.IconUri = _isFavorite
                                ? new Uri("/Images/appbar.favs.rest.png", UriKind.RelativeOrAbsolute)
                                : new Uri("/Images/appbar.favs.addto.rest.png", UriKind.RelativeOrAbsolute);
        }

        public int GetIndex(object o)
        {
            return DataItems == null ? 0 : DataItems.IndexOf(o as ZumpaSubItem);
        }
    }
}
