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

using Inventory.Common;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class DishesViewModel : ViewModelBase
    {
        private IList<Guid> _categoryFilter;
        public IList<Guid> CategoryFilter
        {
            get => _categoryFilter;
            set => Set(ref _categoryFilter, value);
        }

        public List<MenuFolderTreeModel> CategoryTree { get; set; }

        private bool _isFilterPaneOpen;
        public bool IsFilterPanelOpen
        {
            get => _isFilterPaneOpen;
            set => Set(ref _isFilterPaneOpen, value);
        }
        public RelayCommand ToogleFilterBarCommand { get; }

        public DishesViewModel(IDishService dishService, IFilePickerService filePickerService, ICommonServices commonServices) 
            : base(commonServices)
        {
            DishService = dishService;

            DishList = new DishListViewModel(DishService, commonServices);
            DishDetails = new DishDetailsViewModel(DishService, filePickerService, commonServices);
            ToogleFilterBarCommand = new RelayCommand(ToogleFilterBar);
        }

        public IDishService DishService { get; }

        public DishListViewModel DishList { get; set; }
        public DishDetailsViewModel DishDetails { get; set; }

        public async Task LoadAsync(DishListArgs args)
        {
            CategoryTree = MenuFolderTreeModel.MenuFolderToTree(DishList.LookupTables.MenuFolders);
            await DishList.LoadAsync(args);
        }
        public void Unload()
        {
            DishDetails.CancelEdit();
            DishList.Unload();
            CategoryTree.Clear();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<DishListViewModel>(this, OnMessage);
            DishList.Subscribe();
            DishDetails.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            DishList.Unsubscribe();
            DishDetails.Unsubscribe();
        }

        private async void OnMessage(DishListViewModel viewModel, string message, object args)
        {
            if (viewModel == DishList && message == Messages.ItemSelected)
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (DishDetails.IsEditMode)
            {
                StatusReady();
                DishDetails.CancelEdit();
            }
            var selected = DishList.SelectedItem;
            if (!DishList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            DishDetails.Item = selected;
        }

        private async Task PopulateDetails(DishModel selected)
        {
            try
            {
                var model = await DishService.GetDishAsync(selected.Id);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("Dishes", "Load Details", ex);
            }
        }

        private void ToogleFilterBar()
        {
            IsFilterPanelOpen = !IsFilterPanelOpen;
        }

        public async void CategoryFilterChanged(MenuFolderTreeModel category)
        {
            if (category?.Id > MenuFolderModel.CATEGORY_ALL)
            {
                CategoryFilter = category.GetMenuFolderKeys();
            }
            else
            {
                CategoryFilter = null;
            }
            await CategoryProductItems(CategoryFilter);
        }

        private async Task CategoryProductItems(IList<Guid> filter)
        {
            try
            {
                var productArgs = DishList.ViewModelArgs;
                if (filter == null)
                {
                    productArgs.MenuFolderFilterKeys = null;
                }
                else
                {
                    productArgs.MenuFolderFilterKeys = filter;
                }
                await DishList.LoadAsync(productArgs);
            }
            catch (Exception ex)
            {
                LogException("Меню", "Загрузка CategoryProductсItems", ex);
            }
        }
    }
}

