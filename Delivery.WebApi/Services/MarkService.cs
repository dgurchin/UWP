using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;

namespace Delivery.WebApi.Services
{
    public class MarkService
    {
        private readonly IDataService _dataService;
        public MarkService(IDataService dataService)
        {
            _dataService = dataService;
        }

        #region CRUD methods  Mark

        /// <summary>
        /// GET(rowGuid) - получить особенность по Mark.RowGuid
        /// </summary>
        /// <param name="MarkGuid"></param>
        /// <returns></returns>
        public async Task<Mark> GetMarkAsync(Guid MarkGuid)
        {
            Mark rez = await _dataService.GetMarkAsync(MarkGuid);
            return rez;
        }

        /// <summary>
        /// GET(rowGuid) - получить список всех особенностей
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Mark>> GetMarksAsync()
        {
            IList<Mark> rez = await _dataService.GetMarksAsync();
            return rez;
        }

        /// <summary>
        ///  POST(Mark) - добавить/обновить особенность
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdateMarkAsync(Mark model)
        {
            int i = 0;
            if (model == null)
            {
                return -1;
            }
            await _dataService.UpdateMarkAsync(model);
            i = i + 1;
            return i;
        }

        /// <summary>
        /// DELETE(rowGuid) - удалить особенность по Mark.RowGuid
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteMarksAsync(Mark model)
        {
            int rez = await _dataService.DeleteMarkAsync(model);
            return rez;
        }

        #endregion
    }
}
