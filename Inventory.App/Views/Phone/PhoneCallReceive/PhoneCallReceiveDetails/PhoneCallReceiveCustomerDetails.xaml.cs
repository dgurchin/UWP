using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class PhoneCallReceiveCustomerDetails : UserControl
    {
        public PhoneCallReceiveCustomerDetails()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public CustomerDetailsViewModel ViewModel
        {
            get { return (CustomerDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerDetailsViewModel), typeof(CustomerDetails), new PropertyMetadata(null));
        #endregion

        public void SetFocus()
        {
            details.SetFocus();
        }

        private void ToggleSwitch_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!ViewModel.IsEditMode)
            {
                ViewModel.IsEditMode = true;
            }
        }
    }
}

