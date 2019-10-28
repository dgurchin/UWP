using Inventory.Data;
using Inventory.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class OrderDishGarnishService : IOrderDishGarnishService
    {
        private IDataServiceFactory DataServiceFactory { get; }

        public OrderDishGarnishService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public async Task<int> DeleteGarnishAsync(OrderDishGarnishModel garnishModel)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteOrderDishGarnishesAsync(CreateEntity(garnishModel));
            }
        }

        public async Task<OrderDishGarnishModel> GetGarnishAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return CreateModel(await dataService.GetOrderDishGarnishAsync(id));
            }
        }

        public async Task<IList<OrderDishGarnishModel>> GetOrderDishGarnishesAsync(int orderDishId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = new List<OrderDishGarnishModel>();
                var entities = await dataService.GetOrderDishGarnishesAsync(orderDishId);
                foreach (var entity in entities)
                {
                    var model = CreateModel(entity);
                    models.Add(model);
                }
                return models;
            }
        }

        public async Task<int> UpdateGarnishAsync(OrderDishGarnishModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = CreateEntity(model);
                int affectedRows = await dataService.UpdateOrderDishGarnishAsync(entity);
                model.Merge(await GetGarnishAsync(entity.Id));
                return affectedRows;
            }
        }

        public async Task CheckAndDeleteGarnishesAsync(IList<OrderGarnishModel> garishes, int existOrderGarnishId)
        {
            var oldGarnishes = garishes.Where(x => x.OrderGarnish != null && x.OrderGarnishId != existOrderGarnishId && x.OrderGarnish.Id > 0).ToList();
            foreach (var oldGarnish in oldGarnishes)
            {
                await DeleteGarnishAsync(oldGarnish.OrderGarnish);
                oldGarnish.OrderGarnish = null;
            }
        }

        public async Task CheckAndAppendGarnishAsync(int orderDishId, Guid dishGuid, OrderGarnishModel selectedGarnish)
        {
            if (selectedGarnish.OrderGarnish == null)
            {
                selectedGarnish.OrderGarnish = new OrderDishGarnishModel
                {
                    OrderDishId = orderDishId,
                    DishGuid = dishGuid,
                    GarnishGuid = selectedGarnish.Garnish.RowGuid,
                    Price = selectedGarnish.Garnish.Price,
                    Quantity = selectedGarnish.Garnish.Price
                };
            }
            await UpdateGarnishAsync(selectedGarnish.OrderGarnish);
        }

        public async Task<IList<OrderGarnishModel>> GetOrderViewGarnishesAsync(IList<DishGarnishModel> dishGarnishes, int orderDishId)
        {
            var viewGarnishes = new List<OrderGarnishModel>();

            var orderGarnishes = await GetOrderDishGarnishesAsync(orderDishId);
            foreach (var dishGarnish in dishGarnishes)
            {
                var orderGarnish = orderGarnishes.FirstOrDefault(orderDishGarnish => orderDishGarnish.GarnishGuid == dishGarnish.RowGuid);
                var orderIngredientModel = new OrderGarnishModel(dishGarnish, orderGarnish);
                viewGarnishes.Add(orderIngredientModel);
            }

            return viewGarnishes;
        }

        private OrderDishGarnishModel CreateModel(OrderDishGarnish entity)
        {
            var model = new OrderDishGarnishModel
            {
                Id = entity.Id,
                DishGuid = entity.DishGuid,
                GarnishGuid = entity.GarnishGuid,
                OrderDishId = entity.OrderDishId,
                Price = entity.Price,
                Quantity = entity.Quantity
            };
            return model;
        }

        private OrderDishGarnish CreateEntity(OrderDishGarnishModel model)
        {
            var entity = new OrderDishGarnish
            {
                Id = model.Id,
                DishGuid = model.DishGuid,
                GarnishGuid = model.GarnishGuid,
                OrderDishId = model.OrderDishId,
                Price = model.Price,
                Quantity = model.Quantity
            };
            return entity;
        }
    }
}
