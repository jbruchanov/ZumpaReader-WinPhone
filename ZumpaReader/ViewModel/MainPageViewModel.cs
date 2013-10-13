using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ZumpaReader;
using ZumpaReader.ViewModel;
using System.Collections.ObjectModel;
using ZumpaReader.Model;
using ZumpaReader.WebService;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
using ZumpaReader.Converters;
using ZumpaReader.Commands;
using System.Windows.Controls;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using System.Net;
using Microsoft.Phone.Controls;
using ZumpaReader.Utils;

namespace ZumpaReader.ViewModel
{
    public class MainPageViewModel : BaseViewModel, ZumpaReader.Converters.BackgroundColorConverter.IGetIndexEvaluator
    {
        #region Fields and properties

        private const int SETTINGS_INDEX = 0;
        private const int RELOAD_INDEX = 1;

        private IWebService _client;

        public LoadMainPageCommand LoadCommand { get; private set; }

        private ZumpaItemsResult _lastResult;

        private ObservableCollection<ZumpaItem> _dataItems;
        public ObservableCollection<ZumpaItem> DataItems
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


        #endregion

        public MainPageViewModel()
        {
            WebService.WebService.WebServiceConfig c = new WebService.WebService.WebServiceConfig();
            c.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            c.LastAnswerAuthor = true;
            _client = new HttpService(c);


            LoadCommand = new LoadMainPageCommand(_client, (e) => Dispatcher.BeginInvoke(() => OnDownloadedPage(e.Context)));
            LoadCommand.CanExecuteChanged += (o, e) =>
            {
                bool can = LoadCommand.CanExecute(null);
                IsProgressVisible = !can;
                (Page.ApplicationBar.Buttons[RELOAD_INDEX] as ApplicationBarIconButton).IsEnabled = can;
            };

            NotifyPropertyChange("BackColorConverter");

        }

        public int GetIndex(object o)
        {
            return _dataItems == null ? 0 : _dataItems.IndexOf(o as ZumpaItem);
        }

        public override void OnPageAttached()
        {
            (Page.ApplicationBar.Buttons[RELOAD_INDEX] as ApplicationBarIconButton).Click += (o, e) =>
            { 
                DataItems.Clear();
                LoadCommand.LoadURL = null;
                _lastResult = null;
                LoadCommand.Execute(null); 
            };

            (Page.ApplicationBar.Buttons[SETTINGS_INDEX] as ApplicationBarIconButton).Click += (o, e) =>
            {
                Page.NavigationService.Navigate(new Uri("/ZumpaReader;component/Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
            };
            LoadCommand.Execute(null);
        }

        public virtual void OnDownloadedPage(ZumpaItemsResult zumpaItemsResult)
        {
            _lastResult = zumpaItemsResult;
            if (DataItems != null)
            {
                foreach (var item in zumpaItemsResult.Items)
                {
                    DataItems.Add(item);
                }
            }
            else
            {
                DataItems = new ObservableCollection<ZumpaItem>(zumpaItemsResult.Items);
            }
            Bind();
        }

        private void Bind()
        {
            MainPage page = (Page as MainPage);
            page.ListBox.SelectionChanged += (o, e) => {
                if (e.AddedItems.Count > 0)
                {
                    ZumpaItem item = e.AddedItems[0] as ZumpaItem;
                    OnItemClick(item);
                }
                
            };
                        
            TiltEffect.SetIsTiltEnabled(page, true);
        }

        public void OnItemClick(ZumpaItem item)
        {
            if (item == null) { return; }
            String url = String.Format("?url={0}&title={1}", HttpUtility.UrlEncode(item.ItemsUrl), HttpUtility.UrlEncode(item.Subject));
            Page.NavigationService.Navigate(new Uri("/ZumpaReader;component/Pages/ThreadPage.xaml" + url, UriKind.RelativeOrAbsolute));
            (Page as MainPage).ListBox.SelectedItem = null;
        }

        public void LoadNextPage()
        {
            if (null != _lastResult && LoadCommand.CanExecuteIt) { 
                LoadCommand.LoadURL = _lastResult.NextPage;
                LoadCommand.Execute(null);
            }
        }
    }
}
