using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;

namespace Delivery.WebApi.Services
{
    public class DishMarkService
    {
        private readonly IDataService _dataService;
        public DishMarkService(IDataService dataService)
        {
            _dataService = dataService;
        }

        #region CRUD methods  DishMark

        /// <summary>
        /// GET(rowGuid) - получить запись по DishMark.RowGuid
        /// </summary>
        /// <param name="DishMarkGuid"></param>
        /// <returns></returns>
        public async Task<DishMark> GetDishMarkAsync(Guid DishMarkGuid)
        {
            DishMark rez = await _dataService.GetDishMarkAsync(DishMarkGuid);
            return rez;
        }

        /// <summary>
        /// GET(rowGuid) - получить все записи для блюда Dish.RowGuid
        /// </summary>
        /// <param name="dishGuid"></param>
        /// <returns></returns>
        public async Task<IList<DishMark>> GetDishMarksAsync(Guid dishGuid)
        {
            IList<DishMark> rez = await _dataService.GetDishMarksAsync(dishGuid);
            return rez;
        }

        /// <summary>
        ///  POST(DishMark) - добавить/обновить запись
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdateDishMarkAsync(DishMark model)
        {
            int i = 0;
            if (model == null)
            {
                return -1;
            }
            await _dataService.UpdateDishMarkAsync(model);
            i = i + 1;
            return i;
        }

        /// <summary>
        /// DELETE(rowGuid) - удалить запись по DishMark.RowGuid
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteDishMarksAsync(DishMark model)
        {
            int rez = await _dataService.DeleteDishMarkAsync(model);
            return rez;
        }

        #endregion
    }
}
