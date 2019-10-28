using Inventory.Services;
using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Inventory.Views
{
    public sealed partial class PhoneCallReceiveView : Page
    {
        public PhoneCallReceiveView()
        {
            ViewModel = ServiceLocator.Current.GetService<PhoneCallReceiveViewModel>();
            ViewModel.CoreDispatcher = Dispatcher;
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
        }

        public PhoneCallReceiveViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.OrdersView.Map.InitializeAsync(mapControl);
            await ViewModel.LoadAsync(e.Parameter as PhoneCallArgs);
            phoneView.ViewModel = ViewModel.PhoneCall;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.OrdersView.Map.Cleanup();
            ViewModel.Unload();
            ViewModel.OrdersView.Unsubscribe();
            ViewModel.Unsubscribe();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            await NavigationService.CreateNewViewAsync<OrdersViewModel>(ViewModel.OrdersView.OrderList.CreateArgs());
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.OrdersView.OrderDetails.CancelEdit();
            if (pivot.SelectedIndex == 0)
            {
                await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(ViewModel.OrdersView.OrderDetails.CreateArgs());
            }
            else
            {
                await NavigationService.CreateNewViewAsync<OrderDishesViewModel>(ViewModel.OrdersView.OrderDishList.CreateArgs());
            }
        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }
    }
}
