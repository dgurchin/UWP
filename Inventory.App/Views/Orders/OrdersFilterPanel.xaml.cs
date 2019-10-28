using Inventory.Models;
using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Inventory.Views
{
    public sealed partial class OrdersFilterPanel : UserControl
    {
        private readonly Converters.OrderStatusColorConverter _statusColorConverter;

        public OrdersFilterPanel()
        {
            InitializeComponent();
            _statusColorConverter = new Converters.OrderStatusColorConverter();
        }

        #region ViewModel
        public OrdersFilterViewModel ViewModel
        {
            get { return (OrdersFilterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrdersFilterViewModel), typeof(OrdersFilterPanel), new PropertyMetadata(null));
        #endregion

        private void OrderStatusList_Loading(FrameworkElement sender, object args)
        {
            orderStatusList.ItemBackgroundRequest = ItemBackgroundOnRequest;
        }

        private Brush ItemBackgroundOnRequest(object item)
        {
            if (!(item is OrderStatusModel model))
            {
                return null;
            }

            Brush brush = (Brush)_statusColorConverter.Convert(model.Id, typeof(int), null, null);
            return brush;
        }
    }
}
