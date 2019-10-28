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

using Inventory.Models;
using Inventory.Services;
using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Inventory.Views
{
    public sealed partial class DishesView : Page
    {
        public DishesView()
        {
            ViewModel = ServiceLocator.Current.GetService<DishesViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            InitializeComponent();
        }

        public DishesViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            DishListArgs productArgs = (DishListArgs)e.Parameter;
            ViewModel.Subscribe();
            await ViewModel.LoadAsync(productArgs);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            await NavigationService.CreateNewViewAsync<DishesViewModel>(ViewModel.DishList.CreateArgs());
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.DishDetails.CancelEdit();
            await NavigationService.CreateNewViewAsync<DishDetailsViewModel>(ViewModel.DishDetails.CreateArgs());
        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }

        private void BtnToogleFilterPanel_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ToogleFilterBarCommand.CanExecute(this))
            {
                ViewModel.ToogleFilterBarCommand.Execute(this);
                UpdateToogleFilterButtonIcon();
            }
        }

        private void UpdateToogleFilterButtonIcon()
        {
            Symbol toogleSymbol = ViewModel.IsFilterPanelOpen ? Symbol.Back : Symbol.Forward;
            btnToogleFilterPanel.Icon = new SymbolIcon(toogleSymbol);
        }

        /// <summary>
        /// Выбор узла (включается отбор по узлу и ниже)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TreeCategory_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem is MenuFolderTreeModel)
            {
                var node = args.InvokedItem as MenuFolderTreeModel;
                if (node is MenuFolderTreeModel item)
                {
                    ViewModel.CategoryFilterChanged(item);
                }
            }
        }
    }
}
