using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace ZumpaReader.ViewModel
{
    public class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {

        private PhoneApplicationPage _page;


        public PhoneApplicationPage Page
        {
            get { return (PhoneApplicationPage)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); OnPageAttached(); }
        }

        // Using a DependencyProperty as the backing store for ParentView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(PhoneApplicationPage), typeof(BaseViewModel), null);


        public void NotifyPropertyChange([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual void OnPageAttached() {}

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            
        }
    }
}
