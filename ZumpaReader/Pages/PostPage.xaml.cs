using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZumpaReader.ViewModel;
using System.Windows.Media;

namespace ZumpaReader.Pages
{
    public partial class PostPage : PhoneApplicationPage
    {
        private const string ViewModel = "ViewModel";

        public PostPage()
        {
            InitializeComponent();
            ApplicationBar.BackgroundColor = (App.Current.Resources["ApplicationBarBackground"] as SolidColorBrush).Color;

            BaseViewModel model = Resources[ViewModel] as BaseViewModel;
            if (model != null)
            {
                model.Page = this;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            BaseViewModel model = Resources[ViewModel] as BaseViewModel;
            if (model != null)
            {
                model.OnNavigatedTo(e);
            }
        }
    }
}