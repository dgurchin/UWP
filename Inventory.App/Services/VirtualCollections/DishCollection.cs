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
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class DishCollection : VirtualCollection<DishModel>
    {
        private DataRequest<Dish> _dataRequest = null;

        public DishCollection(IDishService dishService, ILogService logService) : base(logService)
        {
            DishService = dishService;
        }

        public IDishService DishService { get; }

        private readonly DishModel _defaultItem = DishModel.CreateEmpty();
        protected override DishModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Dish> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await DishService.GetDishesCountAsync(_dataRequest);
                Ranges[0] = await DishService.GetDishesAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<DishModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await DishService.GetDishesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("ProductCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
