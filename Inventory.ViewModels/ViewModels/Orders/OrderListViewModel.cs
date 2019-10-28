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
    #region OrderListArgs
    public class OrderListArgs
    {
        static public OrderListArgs CreateEmpty() => new OrderListArgs { IsEmpty = true };

        public OrderListArgs()
        {
            OrderByDesc = r => r.CreatedOn;
        }

        public bool IsEmpty { get; set; }

        public int CustomerId { get; set; }

        public string Query { get; set; }

        public Expression<Func<Order, bool>> Where { get; set; }
        public Expression<Func<Order, object>> OrderBy { get; set; }
        public Expression<Func<Order, object>> OrderByDesc { get; set; }
    }
    #endregion

    public class OrderListViewModel : GenericListViewModel<OrderModel>
    {
        public string PhoneNumber { get; set; }
        public OrderDetailsArgs OrderDetailsArgs_phone { get; set; }

        private bool _isFilterPaneOpen = true;

        public bool IsFilterPanelOpen
        {
            get => _isFilterPaneOpen;
            set => Set(ref _isFilterPaneOpen, value);
        }

        public OrderListViewModel(IOrderService orderService, ICommonServices commonServices) : base(commonServices)
        {
            OrderService = orderService;
        }
        public IOrderService OrderService { get; }

        public OrderListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(OrderListArgs args, bool silent = false)
        { 
            ViewModelArgs = args ?? OrderListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            if (silent)
            {
                await RefreshAsync();
            }
            else
            {
                StartStatusMessage("Заказы загружаются...");
                if (await RefreshAsync())
                {
                    EndStatusMessage("Заказы загружены");
                }
            }

            SelectedItem?.NotifyChanges();
        }

        public void Unload()
        {
            if (ViewModelArgs != null)
            {
                ViewModelArgs.Query = Query;
            }
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderListViewModel>(this, OnMessage);
            MessageService.Subscribe<OrderDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderListArgs CreateArgs()
        {
            return new OrderListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc,
                CustomerId = ViewModelArgs.CustomerId
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
                Items = new List<OrderModel>();
                StatusError($"Ошибка загрузки заказов: {ex.Message}");
                LogException("Orders", "Refresh", ex);
                isOk = false;
                throw;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<OrderModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Order> request = BuildDataRequest();
                return await OrderService.GetOrdersAsync(request);
            }
            return new List<OrderModel>();
        }

        public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        private async void OnOpenInNewView()
        {
            if (SelectedItem != null)
            {
                await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(new OrderDetailsArgs { OrderGuid = SelectedItem.RowGuid });
            }
        }

        protected override async void OnNew()
        {
            var orderArgs = new OrderDetailsArgs
            {
                CustomerId = ViewModelArgs?.CustomerId ?? 0,
                PhoneNumber = PhoneNumber
            };

            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(orderArgs);
            }
            else if (!string.IsNullOrEmpty(PhoneNumber))
            {
                await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(orderArgs);
            }
            else
            {
                    NavigationService.Navigate<OrderDetailsViewModel>(orderArgs);
            }

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Заказы загружаются...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Заказы загружены");
            }
        }

        protected override void OnFilter()
        {
            IsFilterPanelOpen = !IsFilterPanelOpen;
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить выбранные заказы?", "Ok", "Отмена"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Удаляется {count} заказов...");
                        await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, Messages.ItemRangesDeleted, SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Удалено {count} заказов...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, Messages.ItemsDeleted, SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Ошибка при удаленмм {count} заказов: {ex.Message}");
                    LogException("Orders", "Delete", ex);
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

        private async Task DeleteItemsAsync(IEnumerable<OrderModel> models)
        {
            foreach (var model in models)
            {
                await OrderService.DeleteOrderAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<Order> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await OrderService.DeleteOrderRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<Order> BuildDataRequest()
        {
            var request = new DataRequest<Order>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc,
                Where=ViewModelArgs.Where
            };
            if (ViewModelArgs.CustomerId > 0)
            {
                request.Where = (r) => r.CustomerId == ViewModelArgs.CustomerId;
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
                case Messages.ItemCopied:
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
