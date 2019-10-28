using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class PhoneCallReceiveOrdersList : UserControl
    {
        public PhoneCallReceiveOrdersList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public OrderListViewModel ViewModel
        {
            get { return (OrderListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderListViewModel), typeof(OrdersListColor), new PropertyMetadata(null));
        #endregion
    }
}
