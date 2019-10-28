using System;

using Inventory.Models.Enums;

using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Inventory.Converters
{
    public class OrderStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // TODO: Брать цветовые настройки из БД
            OrderStatusEnum enumValue = value == null ? OrderStatusEnum.None : (OrderStatusEnum)value;
            switch (enumValue)
            {
                case OrderStatusEnum.NewFromSite:
                    return new SolidColorBrush(Colors.Gold);
                case OrderStatusEnum.Clarification:
                    return new SolidColorBrush(Colors.BurlyWood);
                case OrderStatusEnum.Registration:
                    return new SolidColorBrush(Colors.Plum);
                case OrderStatusEnum.Waiting:
                    return new SolidColorBrush(Colors.MediumPurple);
                case OrderStatusEnum.Accepted:
                    return new SolidColorBrush(Colors.Khaki);
                case OrderStatusEnum.Timer:
                    return new SolidColorBrush(Colors.SteelBlue);
                case OrderStatusEnum.InProcess:
                    return new SolidColorBrush(Colors.MediumSeaGreen);
                case OrderStatusEnum.Gathered:
                    return new SolidColorBrush(Colors.YellowGreen);
                case OrderStatusEnum.OnRoad:
                    return new SolidColorBrush(Colors.Gray);
                case OrderStatusEnum.Delivered:
                    return new SolidColorBrush(Colors.Transparent);
                case OrderStatusEnum.Closed:
                    return new SolidColorBrush(Colors.Transparent);
                case OrderStatusEnum.Canceled:
                    return new SolidColorBrush(Colors.IndianRed);
                case OrderStatusEnum.Basket:
                    return new SolidColorBrush(Colors.Transparent);
                case OrderStatusEnum.None:
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
