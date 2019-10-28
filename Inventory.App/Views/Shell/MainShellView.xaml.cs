#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Linq;

using Inventory.Services;
using Inventory.ViewModels;

using Windows.ApplicationModel.Core;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Inventory.Views
{
    public sealed partial class MainShellView : Page
    {
        private INavigationService _navigationService = null;

        private SystemNavigationManagerPreview NavigationManager => SystemNavigationManagerPreview.GetForCurrentView();

        public MainShellViewModel ViewModel { get; }

        public MainShellView()
        {
            ViewModel = ServiceLocator.Current.GetService<MainShellViewModel>();
            InitializeContext();
            InitializeComponent();
            InitializeNavigation();
        }

        private void InitializeContext()
        {
            var context = ServiceLocator.Current.GetService<IContextService>();
            context.Initialize(Dispatcher, ApplicationView.GetForCurrentView().Id, CoreApplication.GetCurrentView().IsMain);
        }

        private void InitializeNavigation()
        {
            _navigationService = ServiceLocator.Current.GetService<INavigationService>();
            _navigationService.Initialize(frame);
            frame.Navigated += OnFrameNavigated;
            NavigationManager.CloseRequested += NavigationManager_CloseRequested;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync(e.Parameter as ShellArgs);
            ViewModel.Subscribe();
            try
            {
                /*var currentView = SystemNavigationManager.GetForCurrentView();
                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;*/

                LinphoneManagerService.Instance.LinphoneManagerInit(Dispatcher);
            }
            catch (Exception ex)
            {
                var logService = ServiceLocator.Current.GetService<ILogService>();
                await logService.WriteAsync(Data.LogType.Error, "App", "LinphoneManager Activate", ex.Message, ex.ToString());
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }

        private async void NavigationManager_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            var dialogService = ServiceLocator.Current.GetService<IDialogService>();
            var deferral = e.GetDeferral();
            e.Handled = !await dialogService.ShowAsync("Подтверждение выхода", "Завершить работу программы?", "Да", "Нет");
            deferral.Complete();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationItem item)
            {
                ViewModel.NavigateTo(item.ViewModel);
            }
            else if (args.IsSettingsSelected)
            {
                ViewModel.NavigateTo(typeof(SettingsViewModel));
            }
            UpdateBackButton();
        }

        private void OnNavigationViewBackButton(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            var targetType = NavigationService.GetViewModel(e.SourcePageType);
            switch (targetType.Name)
            {
                case "SettingsViewModel":
                    ViewModel.SelectedItem = navigationView.SettingsItem;
                    break;
                default:
                    ViewModel.SelectedItem = ViewModel.Items.Where(r => r.ViewModel == targetType).FirstOrDefault();
                    break;
            }
            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            NavigationViewBackButton.IsEnabled = _navigationService.CanGoBack;
        }

        private async void OnLogoff(object sender, RoutedEventArgs e)
        {
            var dialogService = ServiceLocator.Current.GetService<IDialogService>();
            if (await dialogService.ShowAsync("Подтвердите выход", "Вы уверены, что хотите выйти?", "Ok", "Отменить"))
            {
                var loginService = ServiceLocator.Current.GetService<ILoginService>();
                loginService.Logoff();
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }
    }
}
