using Inventory.Services;
using Linphone.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace Inventory.Views
{
    public sealed partial class PhoneCallConnectControl : UserControl
    {
        public PhoneCallViewModel ViewModel { get; set; }

        public PhoneCallConnectControl()
        {
            this.InitializeComponent();
        }

        private void PhoneSatus_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LinphoneManager.Instance.Core.RefreshRegisters();
        }

        #region PhoneVisible
        public bool PhoneVisible
        {
            get { return (bool)GetValue(PhoneVisibleProperty); }
            set
            {
                SetValue(PhoneVisibleProperty, value);
                ViewModel.PhoneVisible = value;
            }
        }

        public static readonly DependencyProperty PhoneVisibleProperty = DependencyProperty.Register(nameof(PhoneVisible), typeof(bool), typeof(PhoneCallConnectControl), new PropertyMetadata(null));
        #endregion
    }
}
