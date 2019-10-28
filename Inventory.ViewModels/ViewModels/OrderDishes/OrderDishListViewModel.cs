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
    #region OrderItemListArgs
    public class OrderDishListArgs
    {
        public static OrderDishListArgs CreateEmpty() => new OrderDishListArgs { IsEmpty = true };

        public OrderDishListArgs()
        {
            OrderBy = r => r.Id;
        }

        public Guid OrderGuid { get; set; }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<OrderDish, object>> OrderBy { get; set; }
        public Expression<Func<OrderDish, object>> OrderByDesc { get; set; }
    }
    #endregion

    /// <summary>
    /// Блюда в заказе
    /// </summary>
    public class OrderDishListViewModel : GenericListViewModel<OrderDishModel>
    {
        public OrderDishListViewModel(IOrderDishService orderDishService, ICommonServices commonServices) : base(commonServices)
        {
            OrderDishService = orderDishService;
        }

        public IOrderDishService OrderDishService { get; }

        public OrderDishListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(OrderDishListArgs args, bool silent = false)
        {
            ViewModelArgs = args ?? OrderDishListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            if (silent)
            {
                await RefreshAsync();
            }
            else
            {
                StartStatusMessage("Загружаются позиции заказа...");
                if (await RefreshAsync())
                {
                    EndStatusMessage("Позиции заказа загружены");
                }
            }
        }
        public void Unload()
        {
            if (ViewModelArgs == null)
            {
                return;
            }

            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDishListViewModel>(this, OnMessage);
            MessageService.Subscribe<OrderDishDetailsViewModel>(this, OnMessage);
            // Подписываем ProductListViewModel(доб. из списка) на обработку сообщ. в  OnMessage
            MessageService.Subscribe<DishListViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderDishListArgs CreateArgs()
        {
            return new OrderDishListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc,
                OrderGuid = ViewModelArgs.OrderGuid
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
                Items = new List<OrderDishModel>();
                StatusError($"Ошибка загрузки позиций заказа: {ex.Message}");
                LogException("OrderDishes", "Refresh", ex);
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

        private async Task<IList<OrderDishModel>> GetItemsAsync()
        {
            if (ViewModelArgs == null)
            {
                ViewModelArgs = OrderDishListArgs.CreateEmpty();
            }

            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<OrderDish> request = BuildDataRequest();
                return await OrderDishService.GetOrderDishesAsync(request);
            }
            return new List<OrderDishModel>();
        }

        public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        private async void OnOpenInNewView()
        {
            if (SelectedItem != null)
            {
                await NavigationService.CreateNewViewAsync<OrderDishDetailsViewModel>(new OrderDishDetailsArgs
                {
                    OrderGuid = SelectedItem.OrderGuid,
                    OrderDish = SelectedItem
                });
            }
        }

        protected override async void OnNew()
        {
            int Wid = ContextService.NewWindowID;
            var orderDishArgs = new OrderDetailsArgs
            {
                OrderGuid = ViewModelArgs.OrderGuid
            };

            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<OrderDishChoiceViewModel>(orderDishArgs);
            }
            // TODO: Magic number
            else if (Wid == 10)
            {
                await NavigationService.CreateNewViewAsync<OrderDishChoiceViewModel>(orderDishArgs);
            }
            else
            {
                if (!NavigationService.Navigate<OrderDishChoiceViewModel>(orderDishArgs))
                {
                    await NavigationService.CreateNewViewAsync<OrderDishChoiceViewModel>(orderDishArgs);
                }
            }

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Заказ загружается...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Блюда заказа загружены");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Подтвердить удаление", "Вы уверены, что хотите удалить выбранные позиции заказа ?", "ОК", "Отмена"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Удаляется {count} позиций заказа...");
                        await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, Messages.ItemRangesDeleted, SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Удалется {count} позиций заказа...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, Messages.ItemsDeleted, SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Ошибка удаления {count} позиций заказа: {ex.Message}");
                    LogException("OrderDishes", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} заказов удалено");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<OrderDishModel> models)
        {
            foreach (var model in models)
            {
                await OrderDishService.DeleteOrderDishAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<OrderDish> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await OrderDishService.DeleteOrderDishRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<OrderDish> BuildDataRequest()
        {
            var request = new DataRequest<OrderDish>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
            if (ViewModelArgs.OrderGuid != Guid.Empty)
            {
                request.Where = (orderDish) => orderDish.OrderGuid == ViewModelArgs.OrderGuid;
            }
            return request;
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
