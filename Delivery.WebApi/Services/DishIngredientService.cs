using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;

namespace Delivery.WebApi.Services
{
    public class DishIngredientService
    {
        private readonly IDataService _dataService;
        public DishIngredientService(IDataService dataService)
        {
            _dataService = dataService;
        }
        #region CRUD methods  DishIngredien

        /// <summary>
        /// GET(rowGuid) - получить инградиент по DishIngredient.RowGuid
        /// </summary>
        /// <param name="DishIngredientGuid"></param>
        /// <returns></returns>
        public async Task<DishIngredient> GetDishIngredientAsync(Guid DishIngredientGuid)
        {
            DishIngredient rez = await _dataService.GetDishIngredientAsync(DishIngredientGuid);
            return rez;
        }

        /// <summary>
        /// GET(rowGuid) - получить список всех инградиентов блюда Dish.RowGuid
        /// </summary>
        /// <param name="dishGuid"></param>
        /// <returns></returns>
        public async Task<IList<DishIngredient>> GetDishIngredientesAsync(Guid dishGuid)
        {
            IList<DishIngredient> rez = await _dataService.GetDishIngredientsAsync(dishGuid);
            return rez;
        }

        /// <summary>
        /// GET() - получить список всех инградиентов (сортировка по названию)
        /// </summary>
        /// <returns></returns>
        public async Task<IList<DishIngredient>> GetDishIngredientesAsync()
        {
            return await _dataService.GetDishIngredientsAsync();
        }

        /// <summary>
        ///  POST(DishIngredient) - добавить инградиент
        /// </summary>
        /// <returns></returns>
        public async Task<int> PostDishIngredientAsync(IList<DishIngredient> models)
        {
            int i = 0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateDishIngredientAsync(model);
                i = i + 1;
            }
            return i;
        }

        /// <summary>
        /// PUT(DishIngredient) - обновить инградиенты - проверка по DishIngredient.RowGuid
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task PutDishIngredientAsync(IList<DishIngredient> models)
        {
            if (models == null || models.Count == 0)
            {
                return;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateDishIngredientAsync(model);
            }
        }

        /// <summary>
        /// DELETE(rowGuid) - удалить по DishIngredient.RowGuid
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteDishIngredientsAsync(params DishIngredient[] models)
        {
            int rez = await _dataService.DeleteDishIngredientsAsync(models);
            return rez;
        }

        #endregion
    }
}