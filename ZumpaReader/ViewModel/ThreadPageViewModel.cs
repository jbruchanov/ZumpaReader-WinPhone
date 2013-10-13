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

        public ICommand OpenLinkCommand {get; private set;}
        #endregion

        public ThreadPageViewModel()
        {
            WebService.WebService.WebServiceConfig c = new WebService.WebService.WebServiceConfig();
            c.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            _service = new HttpService(c);
            OpenLinkCommand = new OpenLinkCommand();
            NotifyPropertyChange("BackColorConverter");
        }

        public override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Bind();
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
                Load();
            }        
        }

        public async void Load()
        {
            IsProgressVisible = true;
            ZWS.ContextResult<List<ZumpaSubItem>> items = await _service.DownloadThread(_threadUrl);
            DataItems = new ObservableCollection<ZumpaSubItem>(items.Context);
            IsProgressVisible = false;
        }

        public int GetIndex(object o)
        {
            return DataItems.IndexOf(o as ZumpaSubItem);
        }
    }
}
