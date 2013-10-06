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
        public interface IGetIndexEvaluator
        {
            int GetIndex(object o);
        }

        public BackgroundColorConverter()
        {

        }

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            IGetIndexEvaluator indexer = parameter as IGetIndexEvaluator;
            if (indexer == null)
            {
                return new SolidColorBrush(Colors.LightGray);
            }

            int index = indexer.GetIndex(value);
            if (index % 2 == 0)
            {
                return new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                return new SolidColorBrush(Colors.DarkGray);
            }
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
	}
}
