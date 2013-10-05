using System;
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
using ViewModel;

namespace ZumpaReader
{
    public partial class MainPage : BasePage
    {
        // Constructor
        public MainPage() : base()
        {
            InitializeComponent();            
            ViewModel.OnAttachPage(this);
        }

        public override ViewModel.BaseViewModel OnCreateViewModel()
        {
            return new MainPageViewModel();
        }
    }
}