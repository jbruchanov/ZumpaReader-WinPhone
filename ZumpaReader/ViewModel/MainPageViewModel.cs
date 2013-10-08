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

namespace ZumpaReader.ViewModel
{
    public class MainPageViewModel : BaseViewModel, ZumpaReader.Converters.BackgroundColorConverter.IGetIndexEvaluator
    {
        #region Fields and properties
        
        private IWebService _client;

        public LoadCommand LoadCommand { get; private set; }
        
        private ZumpaItemsResult _lastResult;

        private ObservableCollection<ZumpaItem> _dataItems;
        public ObservableCollection<ZumpaItem> DataItems {
            get {return _dataItems;}
            set {_dataItems = value; NotifyPropertyChange();}
        }

        #endregion

        public MainPageViewModel()
        {
            _client = new HttpService();
            LoadCommand = new LoadCommand(_client, (e) => Dispatcher.BeginInvoke( () => OnDownloadedPage(e.Context)));
            NotifyPropertyChange("BackColorConverter");
            test();
        }  
        
        private void test()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }      

        public int GetIndex(object o)
        {
            return _dataItems == null ? 0 : _dataItems.IndexOf(o as ZumpaItem);            
        }

        public virtual void OnDownloadedPage(ZumpaItemsResult zumpaItemsResult)
        {
            _lastResult = zumpaItemsResult;
            DataItems = new ObservableCollection<ZumpaItem>(zumpaItemsResult.Items);
        }
    }
}
