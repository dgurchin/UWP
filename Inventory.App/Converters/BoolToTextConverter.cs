﻿using System;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Inventory.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public string TrueText { get; set; }
        public string FalseText { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = value is bool && (bool)value;

            return XamlBindingHelper.ConvertValue(targetType, boolValue ? TrueText : FalseText);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
