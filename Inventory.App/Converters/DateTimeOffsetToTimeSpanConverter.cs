using System;
using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    public sealed class DateTimeOffsetToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (value is DateTimeOffset dto)
                {
                    return dto.TimeOfDay;
                }
            }
            return DateTimeOffset.MinValue.TimeOfDay;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (value is TimeSpan)
                {
                    // TODO: Указывать в параметр дату к которой будет применено время
                    var dateTime = Extensions.DateTimeExtensions.TimeSpanToDateTime((TimeSpan)value);
                    return Extensions.DateTimeExtensions.DateTimeToDateTimeOffSet(dateTime.Value);
                }
            }
            return Extensions.DateTimeExtensions.DateTimeToDateTimeOffSet(DateTime.MinValue);
        }
    }
}
