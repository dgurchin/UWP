using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;

namespace Delivery.WebApi.Services
{
    public class DishService
    {
        private readonly IDataService _dataService;

        public DishService(IDataService dataService)
        {
            _dataService = dataService;
        }
        #region CRUD methods  Dish

        /// <summary>
        /// GET(rowGuid) - получить блюдо по Dish.RowGuid
        /// </summary>
        /// <param name="dishGuid"></param>
        /// <returns></returns>
        public async Task<Dish> GetDishAsync(Guid dishGuid)
        {
            Dish rez =await _dataService.GetDishAsync(dishGuid);
            return rez;
        }

        /// <summary>
        /// GET() - получить take-блюд из списка
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Dish>> GetDishesAsync(int skip=0,int take=200)
        {
            return await _dataService.GetDishesAsync(skip, take, new DataRequest<Dish>());
        }

        /// <summary>
        ///  POST(Dish) - добавить блюда
        /// </summary>
        /// <returns></returns>
        public async Task<int> PostDishAsync(IList<Dish> models)
        {
            int i=0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateDishAsync(model);
                i=i+1;
            }
            return i;
        }

        /// <summary>
        /// PUT(Dish) - обновить блюда - проверка по Dish.RowGuid
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task PutDishAsync(IList<Dish> models)
        {
            if (models == null || models.Count == 0)
            {
                return;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateDishAsync(model);
            }
        }

        /// <summary>
        /// DELETE(rowGuid) - удалить по Dish.RowGuid
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteDishesAsync(params Dish[] models)
        {
            int rez=await _dataService.DeleteDishesAsync(models);
            return rez;
        }

        #endregion
    }
}
