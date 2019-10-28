using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Services
{
    public interface IOrderDishIngredientService
    {
        Task<OrderDishIngredientModel> GetIngredientAsync(int id);
        Task<IList<OrderDishIngredientModel>> GetIngredientsAsync(int orderDishId);
        Task<int> UpdateIngredientAsync(OrderDishIngredientModel model);
        Task<int> DeleteIngredientAsync(OrderDishIngredientModel model);

        Task<IList<OrderIngredientModel>> GetOrderViewIngredientsAsync(IList<DishIngredientModel> dishIngredients, int orderDishId);
        Task CheckAndDeleteIngredientsAsync(IList<OrderIngredientModel> ingredients);
        Task CheckAndAppendIngredientsAsync(IList<OrderIngredientModel> ingredients, int orderDishId);
    }
}
