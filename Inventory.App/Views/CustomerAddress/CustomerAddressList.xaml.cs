using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Views
{
    public sealed partial class CustomerAddressList : UserControl
    {
        public CustomerAddressList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public CustomerAddressListViewModel ViewModel
        {
            get { return (CustomerAddressListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerAddressListViewModel), typeof(CustomerAddressList), new PropertyMetadata(null));
        #endregion
    }
}
