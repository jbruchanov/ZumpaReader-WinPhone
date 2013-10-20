using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZumpaReader.WebService;
using ZumpaReader.ViewModel;
using System.Windows.Media;

namespace ZumpaReader.Pages
{
    public partial class ThreadPage : PhoneApplicationPage
    {
        private const string ViewModel = "ViewModel";
        public ThreadPage()
        {
            InitializeComponent();
            ApplicationBar.BackgroundColor = (App.Current.Resources["ApplicationBarBackground"] as SolidColorBrush).Color;

            BaseViewModel model = Resources[ViewModel] as BaseViewModel;
            if (model != null)
            {
                model.Page = this;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            BaseViewModel model = Resources[ViewModel] as BaseViewModel;
            if (model != null)
            {
                model.OnNavigatedTo(e);
            }         
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ThreadPageViewModel model = Resources[ViewModel] as ThreadPageViewModel;
            if (model != null)
            {
                model.ReloadDataNavigationBack = !"app://external/".Equals(e.Uri.ToString());
            }                 
        }
    }
}