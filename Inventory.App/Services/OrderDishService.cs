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
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;
using Inventory.Models;

namespace Inventory.Services
{
    public class OrderDishService : IOrderDishService
    {
        private IDishGarnishService DishGarnishService { get; }
        private IOrderDishGarnishService OrderDishGarnishService { get; }
        private IDishIngredientService DishIngredientService { get; }
        private IOrderDishIngredientService OrderDishIngredientService { get; }

        public OrderDishService(IDataServiceFactory dataServiceFactory,
            IDishGarnishService dishGarnishService,
            IDishIngredientService dishIngredientService,
            IOrderDishGarnishService orderDishGarnishService,
            IOrderDishIngredientService orderDishIngredientService)
        {
            DataServiceFactory = dataServiceFactory;
            DishGarnishService = dishGarnishService;
            OrderDishGarnishService = orderDishGarnishService;
            DishIngredientService = dishIngredientService;
            OrderDishIngredientService = orderDishIngredientService;
        }

        public IDataServiceFactory DataServiceFactory { get; }

        public async Task<OrderDishModel> GetOrderDishAsync(int orderDishId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetOrderDishAsync(dataService, orderDishId);
            }
        }

        public Task<IList<OrderDishModel>> GetOrderDishesAsync(DataRequest<OrderDish> request)
        {
            // OrderItems are not virtualized
            return GetOrderDishesAsync(0, 100, request);
        }

        public async Task<IList<OrderDishModel>> GetOrderDishesAsync(int skip, int take, DataRequest<OrderDish> request)
        {
            var models = new List<OrderDishModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetOrderDishesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateOrderDishModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetOrderDishesCountAsync(DataRequest<OrderDish> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetOrderDishesCountAsync(request);
            }
        }

        public async Task<int> UpdateOrderDishAsync(OrderDishModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var orderDish = model.Id > 0 ? await dataService.GetOrderDishAsync(model.Id) : new OrderDish();
                if (orderDish != null)
                {
                    UpdateOrderDishFromModel(orderDish, model);
                    await dataService.UpdateOrderDishAsync(orderDish);
                    model.Merge(await GetOrderDishAsync(dataService, orderDish.Id));

                    var order = await dataService.GetOrderAsync(model.OrderGuid);
                    if (order != null)
                    {
                        await dataService.BindDishesToOrderAsync(model.OrderGuid, order.Id);
                    }
                }
                return 0;
            }
        }

        public async Task<int> DeleteOrderDishAsync(OrderDishModel model)
        {
            var orderItem = new OrderDish { Id = model.Id, OrderGuid = model.OrderGuid, DishGuid = model.DishGuid };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteOrderDishesAsync(orderItem);
            }
        }

        public async Task<int> DeleteOrderDishRangeAsync(int index, int length, DataRequest<OrderDish> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetOrderDishKeysAsync(index, length, request);
                return await dataService.DeleteOrderDishesAsync(items.ToArray());
            }
        }

        public async Task<decimal> GetOrderDishesSumAsync(Guid orderGuid)
        {
            var dishes = await GetOrderDishesAsync(new DataRequest<OrderDish>
            {
                Where = x => x.OrderGuid == orderGuid
            });

            decimal sum = dishes.Sum(dish => dish.Total);
            return sum;
        }

        private async Task<OrderDishModel> CreateOrderDishModelAsync(OrderDish source, bool includeAllFields)
        {
            var model = new OrderDishModel()
            {
                Id = source.Id,
                OrderGuid = source.OrderGuid,
                DishGuid = source.DishGuid,
                Quantity = source.Quantity,
                Price = source.Price,
                TaxTypeId = source.TaxTypeId,
                Dish = await DishService.CreateDishModelAsync(source.Dish, includeAllFields)
            };

            if (!includeAllFields)
            {
                // Include garnishes and ingredients for list view
                model.Garnishes = await CreateGarnishModels(source.Id);
                model.Ingredients = await CreateIngredientModels(source.Id);
            }
            return model;
        }

        private async Task<OrderDishModel> GetOrderDishAsync(IDataService dataService, int orderDishId)
        {
            var dish = await dataService.GetOrderDishAsync(orderDishId);
            if (dish != null)
            {
                return await CreateOrderDishModelAsync(dish, includeAllFields: true);
            }
            return null;
        }

        private void UpdateOrderDishFromModel(OrderDish target, OrderDishModel source)
        {
            target.OrderGuid = source.OrderGuid;
            target.DishGuid = source.DishGuid;
            target.Quantity = source.Quantity;
            target.Price = source.Price;
            target.TaxTypeId = source.TaxTypeId;
        }

        private async Task<IList<OrderGarnishModel>> CreateGarnishModels(int orderDishId)
        {
            var viewGarnishes = new List<OrderGarnishModel>();
            var orderGarnishes = await OrderDishGarnishService.GetOrderDishGarnishesAsync(orderDishId);
            foreach (var orderGarnish in orderGarnishes)
            {
                var viewGarnish = await CreateOrderGarnishModel(orderGarnish);
                viewGarnishes.Add(viewGarnish);
            }
            return viewGarnishes;
        }

        private async Task<OrderGarnishModel> CreateOrderGarnishModel(OrderDishGarnishModel orderGarnish)
        {
            var dishGarnish = await DishGarnishService.GetDishGarnishAsync(orderGarnish.GarnishGuid);
            var model = new OrderGarnishModel(dishGarnish, orderGarnish);
            return model;
        }

        private async Task<IList<OrderIngredientModel>> CreateIngredientModels(int orderDishId)
        {
            var viewIngredients = new List<OrderIngredientModel>();
            var orderIngredients = await OrderDishIngredientService.GetIngredientsAsync(orderDishId);
            foreach (var orderIngredient in orderIngredients)
            {
                var viewIngredient = await CreateOrderIngredientModel(orderIngredient);
                viewIngredients.Add(viewIngredient);
            }
            return viewIngredients;
        }

        private async Task<OrderIngredientModel> CreateOrderIngredientModel(OrderDishIngredientModel orderIngredient)
        {
            var dishIngredient = await DishIngredientService.GetDishIngredientAsync(orderIngredient.IngredientGuid);
            var model = new OrderIngredientModel(dishIngredient, orderIngredient)
            {
                IsSelected = true
            };
            return model;
        }
    }
}
