using Inventory.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Views
{
    public sealed partial class PhoneCallConnectOrder : UserControl
    {
        public PhoneCallViewModel ViewModel { get; set; }
        public PhoneCallConnectOrder()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                ViewModel = ServiceLocator.Current.GetService<PhoneCallViewModel>();
                ViewModel.CoreDispatcher = Dispatcher;
                ViewModel.Load();
            }
            this.InitializeComponent();
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

        public static readonly DependencyProperty PhoneVisibleProperty = DependencyProperty.Register(nameof(PhoneVisible), typeof(bool), typeof(PhoneCallConnectOrder), new PropertyMetadata(null));
        #endregion
    }
}
