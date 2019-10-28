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
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Services;
using Inventory.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Inventory.Views
{
    public sealed partial class OrderView : Page
    {
        private static Dictionary<Guid, OrderDetailsWithItemsViewModel> ViewModels { get; }

        public OrderDetailsWithItemsViewModel ViewModel { get; private set; }
        public INavigationService NavigationService { get; }

        static OrderView()
        {
            ViewModels = new Dictionary<Guid, OrderDetailsWithItemsViewModel>();
        }

        public OrderView()
        {
            ViewModel = ServiceLocator.Current.GetService<OrderDetailsWithItemsViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsRestoredFromMemory(e))
            {
                FocusDetails();
                return;
            }

            Loaded += OrderView_Loaded;
            ViewModel.Subscribe();
            await ViewModel.LoadAsync(e.Parameter as OrderDetailsArgs);
            FocusDetails();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (IsNavigatingToDishChoiceView(e))
            {
                StoreViewModel();
            }
            else
            {
                Loaded -= OrderView_Loaded;
                ViewModel.Unload();
                ViewModel.Unsubscribe();
            }
        }

        private void OrderView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.OrderDetails?.NotifyChanges();
        }

        private bool IsNavigatingToDishChoiceView(NavigationEventArgs e)
        {
            return e.NavigationMode == NavigationMode.New
                && e.SourcePageType == typeof(OrderDishChoiceView)
                && ViewModel.OrderDetails.ItemIsNew;
        }

        private void StoreViewModel()
        {
            Guid orderGuid = ViewModel.OrderDetails.ViewModelArgs.OrderGuid;
            if (!ViewModels.ContainsKey(orderGuid))
            {
                ViewModels.Add(orderGuid, ViewModel);
            }
        }

        private bool IsRestoredFromMemory(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                return false;
            }

            var detailArgs = e.Parameter as OrderDetailsArgs;
            Guid orderGuid = detailArgs.OrderGuid;
            if (ViewModels.ContainsKey(orderGuid))
            {
                ViewModel = ViewModels[orderGuid];
                ViewModels.Remove(orderGuid);
                return true;
            }

            return false;
        }

        private async void FocusDetails()
        {
            if (ViewModel.OrderDetails.IsEditMode)
            {
                await Task.Delay(100);
                details.SetFocus();
            }
        }
    }
}
