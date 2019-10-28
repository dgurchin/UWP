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
    public class OrderDishesViewModel : ViewModelBase
    {
        public OrderDishesViewModel(IDishService dishService, IOrderDishService orderDishService,
            IDishGarnishService dishGarnishService, IOrderDishGarnishService orderDishGarnishService,
            IDishIngredientService dishIngredientService, IOrderDishIngredientService orderDishIngredientService,
            IOrderDishModifierService orderDishModifierService, IDishRecommendService dishRecommendService, 
            ICommonServices commonServices)
            : base(commonServices)
        {
            OrderDishService = orderDishService;
            OrderDishList = new OrderDishListViewModel(OrderDishService, commonServices);
            OrderDishDetails = new OrderDishDetailsViewModel(dishService, orderDishService,
                dishGarnishService, orderDishGarnishService,
                dishIngredientService, orderDishIngredientService,
                orderDishModifierService, dishRecommendService,
                commonServices);
        }

        public IOrderDishService OrderDishService { get; }

        public OrderDishListViewModel OrderDishList { get; set; }
        public OrderDishDetailsViewModel OrderDishDetails { get; set; }

        public async Task LoadAsync(OrderDishListArgs args)
        {
            await OrderDishList.LoadAsync(args);
        }
        public void Unload()
        {
            OrderDishDetails.CancelEdit();
            OrderDishList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDishListViewModel>(this, OnMessage);
            OrderDishList.Subscribe();
            OrderDishDetails.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderDishList.Unsubscribe();
            OrderDishDetails.Unsubscribe();
        }

        private async void OnMessage(OrderDishListViewModel viewModel, string message, object args)
        {
            if (viewModel == OrderDishList && message == Messages.ItemSelected)
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (OrderDishDetails.IsEditMode)
            {
                StatusReady();
                OrderDishDetails.CancelEdit();
            }
            var selected = OrderDishList.SelectedItem;
            if (!OrderDishList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            OrderDishDetails.Item = selected;
        }

        private async Task PopulateDetails(OrderDishModel selected)
        {
            try
            {
                var model = await OrderDishService.GetOrderDishAsync(selected.Id);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("OrderDishes", "Загрузка деталей", ex);
            }
        }
    }
}
