using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Inventory.Views
{
    public sealed partial class OrdersOrderStatusHistory : UserControl
    {
        public OrdersOrderStatusHistory()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderStatusHistoryListViewModel ViewModel
        {
            get { return (OrderStatusHistoryListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderStatusHistoryListViewModel), typeof(OrdersOrderStatusHistory), new PropertyMetadata(null));
        #endregion
    }
}
