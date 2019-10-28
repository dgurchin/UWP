using Inventory.Data;
using Inventory.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class OrderDishIngredientService : IOrderDishIngredientService
    {
        private IDataServiceFactory DataServiceFactory { get; }

        public OrderDishIngredientService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public async Task<OrderDishIngredientModel> GetIngredientAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return CreateModel(await dataService.GetOrderDishIngredientAsync(id));
            }
        }

        public async Task<IList<OrderDishIngredientModel>> GetIngredientsAsync(int orderDishId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = new List<OrderDishIngredientModel>();
                var entities = await dataService.GetOrderDishIngredientsAsync(orderDishId);
                foreach (var entity in entities)
                {
                    var model = CreateModel(entity);
                    models.Add(model);
                }
                return models;
            }
        }

        public async Task<int> UpdateIngredientAsync(OrderDishIngredientModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = CreateEntity(model);
                int affectedRows = await dataService.UpdateOrderDishIngredientAsync(entity);
                model.Merge(await GetIngredientAsync(entity.Id));
                return affectedRows;
            }
        }

        public async Task<int> DeleteIngredientAsync(OrderDishIngredientModel ingredientModel)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteOrderDishIngredientsAsync(CreateEntity(ingredientModel));
            }
        }

        public async Task<IList<OrderIngredientModel>> GetOrderViewIngredientsAsync(IList<DishIngredientModel> dishIngredients, int orderDishId)
        {
            var viewIngredients = new List<OrderIngredientModel>();

            var orderIngredients = await GetIngredientsAsync(orderDishId);
            foreach (var dishIngredient in dishIngredients)
            {
                var orderIngredient = orderIngredients.FirstOrDefault(orderDishIngredient => orderDishIngredient.IngredientGuid == dishIngredient.RowGuid);
                var orderIngredientModel = new OrderIngredientModel(dishIngredient, orderIngredient);
                viewIngredients.Add(orderIngredientModel);
            }

            return viewIngredients;
        }

        public async Task CheckAndDeleteIngredientsAsync(IList<OrderIngredientModel> ingredients)
        {
            var deleteIngredients = ingredients.Where(ingredient => !ingredient.IsSelected && ingredient.OrderIngredient != null).ToList();
            foreach (var ingredientView in deleteIngredients)
            {
                await DeleteIngredientAsync(ingredientView.OrderIngredient);
                ingredientView.OrderIngredient = null;
            }
        }

        public async Task CheckAndAppendIngredientsAsync(IList<OrderIngredientModel> ingredients, int orderDishId)
        {
            var newIngredients = ingredients.Where(ingredient => ingredient.IsSelected && ingredient.OrderIngredient == null).ToList();
            foreach (var ingredientView in newIngredients)
            {
                var ingredient = new OrderDishIngredientModel
                {
                    OrderDishId = orderDishId,
                    DishGuid = ingredientView.Ingredient.DishGuid,
                    IngredientGuid = ingredientView.Ingredient.RowGuid,
                    Price = ingredientView.Ingredient.Price,
                    Quantity = 1
                };
                await UpdateIngredientAsync(ingredient);
                ingredientView.OrderIngredient = ingredient;
            }
        }

        private OrderDishIngredientModel CreateModel(OrderDishIngredient entity)
        {
            var model = new OrderDishIngredientModel
            {
                Id = entity.Id,
                DishGuid = entity.DishGuid,
                IngredientGuid = entity.IngredientGuid,
                OrderDishId = entity.OrderDishId,
                Price = entity.Price,
                Quantity = entity.Quantity
            };
            return model;
        }

        private OrderDishIngredient CreateEntity(OrderDishIngredientModel model)
        {
            var entity = new OrderDishIngredient
            {
                Id = model.Id,
                DishGuid = model.DishGuid,
                IngredientGuid = model.IngredientGuid,
                OrderDishId = model.OrderDishId,
                Price = model.Price,
                Quantity = model.Quantity
            };
            return entity;
        }
    }
}
