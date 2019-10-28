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
using System.Threading.Tasks;

using Inventory.Common;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public MapViewModel Map { get; }

        private bool _IsCtrlKeyDown;
        public bool IsCtrlKeyDown
        {
            get => _IsCtrlKeyDown;
            set => Set(ref _IsCtrlKeyDown, value);
        }

        public RelayCommand ToogleFilterBarCommand { get; }

        public OrdersViewModel(IOrderService orderService, IOrderDishService orderDishService, IOrderStatusHistoryService orderStatusHistoryService,
            ILocationService locationService, IVariableService variableService, ICommonServices commonServices, IDishService dishService,
            IDishGarnishService dishGarnishService, IOrderDishGarnishService orderDishGarnishService,
            IDishIngredientService dishIngredientService, IOrderDishIngredientService orderDishIngredientService,
            IOrderDishModifierService orderDishModifierService, IDishRecommendService dishRecommendService, 
            IOrderLoadingMonitor orderLoadingMonitor, ICustomerService customerService)
            : base(commonServices)
        {
            OrderService = orderService;

            OrderList = new OrderListViewModel(OrderService, commonServices);
            OrderDetails = new OrderDetailsViewModel(orderService, orderDishService, orderStatusHistoryService, variableService, commonServices,
                dishService, dishGarnishService, orderDishGarnishService, dishIngredientService,
                orderDishIngredientService, orderDishModifierService, dishRecommendService, orderLoadingMonitor, customerService);
            OrderDishList = new OrderDishListViewModel(orderDishService, commonServices);
            OrderStatusHistoryList = new OrderStatusHistoryListViewModel(orderStatusHistoryService, commonServices);
            CustomerOrders = new OrderListViewModel(orderService, commonServices);
            OrderFilter = new OrdersFilterViewModel(OrderList, commonServices);
            Map = new MapViewModel(locationService, commonServices);

            ToogleFilterBarCommand = new RelayCommand(ToogleFilterBar);
        }

        public IOrderService OrderService { get; }

        #region ViewModels
        public OrderListViewModel OrderList { get; set; }
        public OrderDetailsViewModel OrderDetails { get; set; }
        public OrderDishListViewModel OrderDishList { get; set; }
        public OrderStatusHistoryListViewModel OrderStatusHistoryList { get; set; }
        public OrdersFilterViewModel OrderFilter { get; set; }
        public OrderListViewModel CustomerOrders { get; }
        #endregion

        public async Task LoadAsync(OrderListArgs args)
        {
            await OrderList.LoadAsync(args);
        }
        public void Unload()
        {
            OrderDetails.CancelEdit();
            OrderList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderListViewModel>(this, OnMessage);
            OrderList.Subscribe();
            OrderDetails.Subscribe();
            OrderDishList.Subscribe();
            OrderStatusHistoryList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderList.Unsubscribe();
            OrderDetails.Unsubscribe();
            OrderDishList.Unsubscribe();
            OrderStatusHistoryList.Unsubscribe();
        }

        private async void OnMessage(OrderListViewModel viewModel, string message, object args)
        {
            if (viewModel == OrderList && message == Messages.ItemSelected)
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (OrderDetails.IsEditMode)
            {
                StatusReady();
                OrderDetails.CancelEdit();
            }
            OrderDishList.IsMultipleSelection = false;
            var selected = OrderList.SelectedItem;
            if (!OrderList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                    await PopulateOrderItems(selected);
                    await PopulateCustomerOrders(selected);
                }
            }
            OrderDetails.Item = await OrderService.GetOrderAsync(selected?.RowGuid ?? Guid.Empty);
        }

        private async Task PopulateDetails(OrderModel selected)
        {
            try
            {
                var model = await OrderService.GetOrderAsync(selected?.RowGuid ?? Guid.Empty);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("Orders", "Load Details", ex);
            }
        }

        private async Task PopulateOrderItems(OrderModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    await OrderDishList.LoadAsync(new OrderDishListArgs { OrderGuid = selectedItem.RowGuid }, silent: true);
                    await OrderStatusHistoryList.LoadAsync(new OrderStatusHistoryListArgs { OrderId = selectedItem.Id }, silent: true);
                }
            }
            catch (Exception ex)
            {
                LogException("Orders", "Load OrderItems", ex);
            }
        }

        private async Task PopulateCustomerOrders(OrderModel selected)
        {
            try
            {
                if (selected != null)
                {
                    var ordersArgs = new OrderListArgs { CustomerId = selected.CustomerId };
                    await CustomerOrders.LoadAsync(ordersArgs, true);
                }
            }
            catch (Exception ex)
            {
                LogException("Orders", "Customer orders", ex);
            }
        }

        private void ToogleFilterBar()
        {
            OrderList.IsFilterPanelOpen = !OrderList.IsFilterPanelOpen;
        }
    }
}
