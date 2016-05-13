using System;
using System.Globalization;
using Xamarin.Forms;

namespace DateTimePickerSample
{
    public class DateTimeToTicksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var date = (DateTime)value;
                return date.Ticks;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var ticks = (long)value;
                return new DateTime(ticks);
            }

            return value;
        }
    }
}
