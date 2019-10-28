using System;

using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    /// <summary>
    /// Implements a databind converter to convert from DateTime to TimeSpan and vice-versa.
    /// </summary>
    public sealed class DateTimeToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime dt = (DateTime)value;
                TimeSpan? ts = Extensions.DateTimeExtensions.DateTimeToTimeSpan(dt);
                return ts.GetValueOrDefault(TimeSpan.MinValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return TimeSpan.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                TimeSpan ts = (TimeSpan)value;
                DateTime? dt = Extensions.DateTimeExtensions.TimeSpanToDateTime(ts);
                return dt.GetValueOrDefault(DateTime.MinValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return DateTime.MinValue;
            }
        }
    }
}
