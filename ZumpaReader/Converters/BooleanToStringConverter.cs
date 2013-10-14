using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ZumpaReader.Converters
{
    public class BooleanToStringConverter : DependencyObject,  IValueConverter
    {
        public string TrueValue
        {
            get { return (string)GetValue(TrueValueProperty); }
            set { SetValue(TrueValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrueValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrueValueProperty =
            DependencyProperty.Register("TrueValue", typeof(string), typeof(BooleanToStringConverter), new PropertyMetadata("True"));

        public string FalseValue
        {
            get { return (string)GetValue(FalseValueProperty); }
            set { SetValue(FalseValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FalseValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FalseValueProperty =
            DependencyProperty.Register("FalseValue", typeof(string), typeof(BooleanToStringConverter), new PropertyMetadata("False"));

        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
