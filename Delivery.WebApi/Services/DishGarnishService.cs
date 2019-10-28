using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;

namespace Delivery.WebApi.Services
{
    public class DishGarnishService
    {
        private readonly IDataService _dataService;
        public DishGarnishService(IDataService dataService)
        {
            _dataService = dataService;
        }
        #region CRUD methods  Dish

        /// <summary>
        /// GET(rowGuid) - получить гарнир по DishGarnish.RowGuid
        /// </summary>
        /// <param name="dishGarnishGuid"></param>
        /// <returns></returns>
        public async Task<DishGarnish> GetDishGarnishAsync(Guid dishGarnishGuid)
        {
            DishGarnish rez = await _dataService.GetDishGarnishAsync(dishGarnishGuid);
            return rez;
        }

        /// <summary>
        /// GET(rowGuid) - получить список всех гарниров блюда Dish.RowGuid
        /// </summary>
        /// <param name="dishGuid"></param>
        /// <returns></returns>
        public async Task<IList<DishGarnish>> GetDishGarnishesAsync(Guid dishGuid)
        {
            IList<DishGarnish> rez = await _dataService.GetDishGarnishesAsync(dishGuid);
            return rez;
        }

        /// <summary>
        /// GET() - получить список всех гарниров (сортировка по названию)
        /// </summary>
        /// <returns></returns>
        public async Task<IList<DishGarnish>> GetDishGarnishesAsync()
        {
            return await _dataService.GetDishGarnishesAsync();
        }

        /// <summary>
        ///  POST(DishGarnish) - добавить гарнир
        /// </summary>
        /// <returns></returns>
        public async Task<int> PostDishGarnishAsync(IList<DishGarnish> models)
        {
            int i = 0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateDishGarnishAsync(model);
                i = i + 1;
            }
            return i;
        }

        /// <summary>
        /// PUT(DishGarnish) - обновить гарниры - проверка по DishGarnish.RowGuid
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task PutDishGarnishAsync(IList<DishGarnish> models)
        {
            if (models == null || models.Count == 0)
            {
                return;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateDishGarnishAsync(model);
            }
        }

        /// <summary>
        /// DELETE(rowGuid) - удалить по DishGarnish.RowGuid
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteDishGarnishesAsync(params DishGarnish[] models)
        {
            int rez = await _dataService.DeleteDishGarnishesAsync(models);
            return rez;
        }

        #endregion
    }
}
