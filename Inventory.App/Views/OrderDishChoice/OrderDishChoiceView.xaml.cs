using Inventory.Services;
using Inventory.ViewModels;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Inventory.Views
{
    public sealed partial class OrderDishChoiceView : Page
    {
        public OrderDishChoiceViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }

        private SystemNavigationManagerPreview NavigationManager => SystemNavigationManagerPreview.GetForCurrentView();

        public OrderDishChoiceView()
        {
            ViewModel = ServiceLocator.Current.GetService<OrderDishChoiceViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            await ViewModel.LoadAsync(e.Parameter as OrderDetailsArgs);
            NavigationManager.CloseRequested += OnCloseRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
            NavigationManager.CloseRequested -= OnCloseRequested;
        }

        private void OnCloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            if (ViewModel.IsMainView || !NavigationService.CanGoBack)
            {
                NavigationManager.CloseRequested -= OnCloseRequested;
                return;
            }

            var deferral = e.GetDeferral();
            e.Handled = true;
            deferral.Complete();

            NavigationService.GoBack();
        }
    }
}
