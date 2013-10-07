using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ZumpaReader.Converters
{
    public class BackgroundColorConverter : IValueConverter
    {

        private Brush _even;
        private Brush _odd;

        public interface IGetIndexEvaluator
        {
            int GetIndex(object o);
        }

        public BackgroundColorConverter()
        {
            _even = App.Current.Resources["RowBackgroundEven"] as Brush;
            _odd = App.Current.Resources["RowBackgroundOdd"] as Brush;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IGetIndexEvaluator indexer = parameter as IGetIndexEvaluator;
            if (indexer == null)
                return _even;

            int index = indexer.GetIndex(value);
            return (index % 2 == 0) ? _even : _odd;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
