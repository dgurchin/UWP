using Inventory.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IDishIngredientService
    {
        Task<DishIngredientModel> GetDishIngredientAsync(int id);
        Task<DishIngredientModel> GetDishIngredientAsync(Guid dishIngredientGuid);
        Task<IList<DishIngredientModel>> GetDishIngredientsAsync(Guid dishGuid);
    }
}
