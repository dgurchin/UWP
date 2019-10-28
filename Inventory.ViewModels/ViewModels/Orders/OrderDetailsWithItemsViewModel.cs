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

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrderDetailsWithItemsViewModel : ViewModelBase
    {
        private OrderModel _editableItem;

        public OrderDetailsViewModel OrderDetails { get; set; }
        public OrderDishListViewModel OrderItemList { get; set; }

        public OrderListViewModel CustomerOrders { get; }

        private int OrderDetailCustomerId
        {
            get
            {
                int customerId = 0;
                if (OrderDetails.EditableItem != null)
                {
                    customerId = OrderDetails.EditableItem.CustomerId;
                }
                else if (OrderDetails.Item != null)
                {
                    customerId = OrderDetails.Item.CustomerId;
                }
                return customerId;
            }
        }

        public bool IsCustomerOrdersVisible => OrderDetailCustomerId > 0;

        // TODO: Слишком много сервисов. Оптимизировать.
        public OrderDetailsWithItemsViewModel(
            IOrderService orderService, 
            IOrderStatusHistoryService orderStatusHistoryService,
            ICustomerService customerService,
            IDishService dishService, IOrderDishService orderDishService,
            IDishGarnishService dishGarnishService, IOrderDishGarnishService orderDishGarnishService,
            IDishIngredientService dishIngredientService, IOrderDishIngredientService orderDishIngredientService,
            IOrderDishModifierService orderDishModifierService, IDishRecommendService dishRecommendService,
            IOrderLoadingMonitor orderLoadingMonitor, 
            IVariableService variableService,
            ICommonServices commonServices)
            : base(commonServices)
        {
            OrderDetails = new OrderDetailsViewModel(orderService, orderDishService, orderStatusHistoryService, variableService, commonServices,
                dishService, dishGarnishService, orderDishGarnishService, dishIngredientService, orderDishIngredientService, 
                orderDishModifierService, dishRecommendService, orderLoadingMonitor, customerService);

            OrderItemList = new OrderDishListViewModel(orderDishService, commonServices);
            CustomerOrders = new OrderListViewModel(orderService, commonServices);
        }

        public async Task LoadAsync(OrderDetailsArgs args)
        {
            await OrderDetails.LoadAsync(args);

            Guid orderGuid = args?.OrderGuid ?? Guid.Empty;
            if (orderGuid == Guid.Empty)
            {
                await OrderItemList.LoadAsync(new OrderDishListArgs { IsEmpty = true }, silent: true);
            }
            else
            {
                await OrderItemList.LoadAsync(new OrderDishListArgs { OrderGuid = args.OrderGuid });
            }

            LoadCustomerOrders();
        }

        public void Unload()
        {
            OrderDetails.CancelEdit();
            OrderDetails.Unload();
            OrderItemList.Unload();
            CustomerOrders.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDetailsViewModel, OrderModel>(this, OnMessage);
            OrderDetails.Subscribe();
            OrderItemList.Subscribe();
            CustomerOrders.Subscribe();
            OrderDetails.PropertyChanged += OrderDetails_PropertyChanged;
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderDetails.Unsubscribe();
            OrderItemList.Unsubscribe();
            CustomerOrders.Unsubscribe();
            OrderDetails.PropertyChanged -= OrderDetails_PropertyChanged;
        }

        private void OrderDetails_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderDetails.EditableItem))
            {
                if (_editableItem != null)
                {
                    _editableItem.PropertyChanged -= EditableItem_PropertyChanged;
                }
                if (OrderDetails.EditableItem != null)
                {
                    OrderDetails.EditableItem.PropertyChanged += EditableItem_PropertyChanged;
                }
                _editableItem = OrderDetails.EditableItem;
            }
        }

        private void EditableItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderDetails.EditableItem.CustomerId))
            {
                LoadCustomerOrders();
            }
        }

        private async void OnMessage(OrderDetailsViewModel viewModel, string message, OrderModel order)
        {
            if (viewModel == OrderDetails && (message == Messages.ItemChanged || message == Messages.NewItemSaved))
            {
                await OrderItemList.LoadAsync(new OrderDishListArgs { OrderGuid = order.RowGuid });
                LoadCustomerOrders();
            }
        }

        private async void LoadCustomerOrders()
        {
            if (OrderDetailCustomerId > 0)
            {
                var listArgs = new OrderListArgs { CustomerId = OrderDetailCustomerId };
                await CustomerOrders.LoadAsync(listArgs, true);
            }
            NotifyPropertyChanged(nameof(IsCustomerOrdersVisible));
        }
    }
}
