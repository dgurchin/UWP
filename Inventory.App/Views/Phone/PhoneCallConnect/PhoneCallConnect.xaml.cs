using Inventory.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Views
{
    public sealed partial class PhoneCallConnect : UserControl
    {
        public PhoneCallViewModel ViewModel { get; set; }
        public PhoneCallConnect()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                ViewModel = ServiceLocator.Current.GetService<PhoneCallViewModel>();
                ViewModel.CoreDispatcher = Dispatcher;
                ViewModel.Load();
            }
            this.InitializeComponent();
        }
    }
}
