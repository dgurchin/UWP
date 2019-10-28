using Inventory.Models;
using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Inventory.Views
{
    public sealed partial class OrdersListColor : UserControl
    {
        private readonly Converters.OrderStatusColorConverter _statusColorConverter;

        public OrdersListColor()
        {
            InitializeComponent();
            _statusColorConverter = new Converters.OrderStatusColorConverter();
        }

        #region ViewModel
        public OrderListViewModel ViewModel
        {
            get { return (OrderListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderListViewModel), typeof(OrdersListColor), new PropertyMetadata(null));
        #endregion

        private void OrderList_Loading(FrameworkElement sender, object args)
        {
            orderList.ItemBackgroundRequest = ItemBackgroundOnRequest;
        }

        private Brush ItemBackgroundOnRequest(object item)
        {
            if (!(item is OrderModel model))
            {
                return null;
            }

            Brush brush = (Brush)_statusColorConverter.Convert(model.StatusId, typeof(int), null, null);
            return brush;
        }
    }
}
