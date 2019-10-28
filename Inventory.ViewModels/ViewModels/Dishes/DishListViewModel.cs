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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Common;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region DishListArgs
    public class DishListArgs
    {
        static public DishListArgs CreateEmpty() => new DishListArgs { IsEmpty = true };

        public DishListArgs()
        {
            OrderBy = r => r.Name;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public IList<Guid> MenuFolderFilterKeys { get; set; }

        public Expression<Func<Dish, bool>> Where { get; set; }
        public Expression<Func<Dish, object>> OrderBy { get; set; }
        public Expression<Func<Dish, object>> OrderByDesc { get; set; }
    }
    #endregion

    /// <summary>
    /// Список всех блюд
    /// </summary>
    public class DishListViewModel : GenericListViewModel<DishModel>
    {
        public DishListViewModel(IDishService dishService, ICommonServices commonServices)
            : base(commonServices)
        {
            DishService = dishService;
        }

        public IDishService DishService { get; }

        public DishListArgs ViewModelArgs { get; private set; }

        public ICommand ItemInvokedCommand => new RelayCommand<DishModel>(ItemInvoked);

        public IOrderDishService OrderDishService { get; private set; }

        private async void ItemInvoked(DishModel model)
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<DishDetailsViewModel>(new DishDetailsArgs { DishId = model.Id });
            }
            else
            {
                NavigationService.Navigate<DishDetailsViewModel>(new DishDetailsArgs { DishId = model.Id });
            }
        }

        public async Task LoadAsync(DishListArgs args)
        {
            ViewModelArgs = args ?? DishListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;
            StartStatusMessage("Загрузка меню...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Меню загружено");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<DishListViewModel>(this, OnMessage);
            MessageService.Subscribe<DishDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public DishListArgs CreateArgs()
        {
            return new DishListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<DishModel>();
                StatusError($"Ошибка загрузки блюд: {ex.Message}");
                LogException("Dishes", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<DishModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Dish> request = BuildDataRequest();
                return await DishService.GetDishesAsync(request);
            }
            return new List<DishModel>();
        }

        protected override async void OnNew()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<DishDetailsViewModel>(new DishDetailsArgs());
            }
            else
            {
                NavigationService.Navigate<DishDetailsViewModel>(new DishDetailsArgs());
            }

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Загрузка меню...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Меню загружено");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить выбранные блюда?", "Ok", "Отмена"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Удаляется {count} блюд...");
                        await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Удаляется {count} блюд...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Ошибка удаления {count} блюд: {ex.Message}");
                    LogException("Dishes", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} блюд удалено");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<DishModel> models)
        {
            foreach (var model in models)
            {
                await DishService.DeleteDishAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<Dish> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await DishService.DeleteDishRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<Dish> BuildDataRequest()
        {
            var request = new DataRequest<Dish>()
            {
                Query = Query,
                Where = BuildRequestWhere(),
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
            return request;
        }

        private Expression<Func<Dish, bool>> BuildRequestWhere()
        {
            Expression<Func<Dish, bool>> whereResult;
            Expression<Func<Dish, bool>> where = ViewModelArgs.Where;
            Expression<Func<Dish, bool>> menuFolderWhere = BuildMenuFolderWhere();

            if (where == null)
            {
                whereResult = menuFolderWhere;
            }
            else if (menuFolderWhere == null)
            {
                whereResult = where;
            }
            else
            {
                whereResult = Expression.Lambda<Func<Dish, bool>>(Expression.AndAlso(where, menuFolderWhere));
            }
            return whereResult;
        }

        private Expression<Func<Dish, bool>> BuildMenuFolderWhere()
        {
            if (ViewModelArgs.MenuFolderFilterKeys == null || ViewModelArgs.MenuFolderFilterKeys.Count == 0)
            {
                return null;
            }

            Expression<Func<Dish, bool>> filter = dish => ViewModelArgs.MenuFolderFilterKeys.Contains(dish.MenuFolder.RowGuid);
            return filter;
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case Messages.NewItemSaved:
                case Messages.ItemDeleted:
                case Messages.ItemsDeleted:
                case Messages.ItemRangesDeleted:
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
