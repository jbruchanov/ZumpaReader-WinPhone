using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using ZumpaReader.Model;

namespace ZumpaReader.Converters
{
    public class SurveyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (targetType == typeof(Visibility))
            {
                var item = value as ZumpaSubItem;
                return item != null && item.Survey != null ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (targetType == typeof(Brush))
            {
                var item = value as ZumpaReader.Model.Survey.SurveyVoteItem;
                return App.Current.Resources["Survey" + item.Index];
            }
            else if (targetType == typeof(double))
            {
                var item = (int)value;
                return item / (float)100 * 320;//320  width
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static float _scaleFactor = 0f;
        public static float GetScaleFactor()
        {
            if (_scaleFactor != 0)
            {
                return _scaleFactor;
            }
            if (System.Environment.OSVersion.Version.Major >= 8)
            {
                try
                {
                    object dc = Application.Current.Host.Content;
                    PropertyInfo pi = dc.GetType().GetProperty("ScaleFactor");
                    int v = (int)pi.GetValue(dc, null);
                    _scaleFactor = v / 100f;
                }
                catch (Exception e) {/* ignore it */}
            }
            else
            {
                _scaleFactor = 1f;
            }
            return _scaleFactor;
        }
    }
}
