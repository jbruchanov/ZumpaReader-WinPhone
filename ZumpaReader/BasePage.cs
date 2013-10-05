using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZumpaReader.ViewModel;

namespace ZumpaReader
{
    public abstract class BasePage : PhoneApplicationPage
    {
        public BaseViewModel ViewModel { get; private set; }

        public BasePage()
        {
            ViewModel = OnCreateViewModel();
        }

        public abstract BaseViewModel OnCreateViewModel();

    }
}
