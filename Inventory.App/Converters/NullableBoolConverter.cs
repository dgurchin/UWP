using System;

using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    public sealed class NullableBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            return null;
        }
    }
}
