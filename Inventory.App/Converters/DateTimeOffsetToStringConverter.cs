using System;
using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    public class DateTimeOffsetToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null)
            {
                return null;
            }
            
            DateTimeOffset sourceTime = (DateTimeOffset)value;
            DateTime targetTime = sourceTime.DateTime;

            return targetTime.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToDateTime(value);
        }
    }
}
