using System;

using Inventory.Models.Enums;

using Windows.UI.Xaml.Data;

namespace Inventory.Converters
{
    public class CommunicationTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((CommunicationTypeEnum)value)
            {
                case CommunicationTypeEnum.Email:
                    return "Email";
                case CommunicationTypeEnum.Mobile:
                    return "Мобильный";
                case CommunicationTypeEnum.Phone:
                    return "Телефон";
                case CommunicationTypeEnum.Skype:
                    return "Skype";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
