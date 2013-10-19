using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using ZumpaReader.Model;

namespace ZumpaReader.Converters
{
    public class StripColorConverter : IValueConverter
    {

        private SolidColorBrush _transparent = new SolidColorBrush(Colors.Transparent);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ZumpaItem item = value as ZumpaItem;
            if (String.Equals(item.Author, AppSettings.Login))
            {
                return App.Current.Resources["StripOwnThread"];
            }
            else if (item.IsFavourite)
            {
                return App.Current.Resources["StripFavThread"];
            }
            else if (item.HasResponseForYou)
            {
                return App.Current.Resources["StripResponseThread"];
            }
            else
            {
                return _transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
