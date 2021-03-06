﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using ZumpaReader.ViewModel;
using System.Diagnostics;
using ZumpaReader.Utils;

namespace ZumpaReader
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string ViewModel = "ViewModel";
        // Constructor
        public MainPage() : base()
        {
            InitializeComponent();

            ApplicationBar.BackgroundColor = (App.Current.Resources["ApplicationBarBackground"] as SolidColorBrush).Color;

            BaseViewModel model = Resources[ViewModel] as BaseViewModel;
            if (model != null)
            {
                model.Page = this;
            }
        }

        private void Footer_Loaded(object sender, RoutedEventArgs e)
        {
            MainPageViewModel model = Resources[ViewModel] as MainPageViewModel;
            if (model != null)
            {
                model.LoadNextPage();
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