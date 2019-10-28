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
using System.Linq;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;
using Inventory.Models;

namespace Inventory.Services
{
    public class DishService : IDishService
    {
        private IDataServiceFactory DataServiceFactory { get; }
        private ILogService LogService { get; }

        public DishService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<DishModel> GetDishAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetDishDataAsync(dataService, id);
            }
        }

        public async Task<DishModel> GetDishAsync(Guid dishGuid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetDishDataAsync(dataService, dishGuid);
            }
        }

        public async Task<IList<DishModel>> GetDishesAsync(DataRequest<Dish> request)
        {
            var collection = new DishCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<DishModel>> GetDishesAsync(int skip, int take, DataRequest<Dish> request)
        {
            var models = new List<DishModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetDishesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateDishModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetDishesCountAsync(DataRequest<Dish> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetDishesCountAsync(request);
            }
        }

        public async Task<int> UpdateDishAsync(DishModel model)
        {
            int id = model.Id;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var dish = id > 0 ? await dataService.GetDishAsync(model.Id) : new Dish();
                if (dish != null)
                {
                    UpdateDishFromModel(dish, model);
                    await dataService.UpdateDishAsync(dish);
                    model.Merge(await GetDishDataAsync(dataService, dish.Id));
                }
                return 0;
            }
        }

        public async Task<int> DeleteDishAsync(DishModel model)
        {
            var product = new Dish { Id = model.Id };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteDishesAsync(product);
            }
        }

        public async Task<int> DeleteDishRangeAsync(int index, int length, DataRequest<Dish> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetDishKeysAsync(index, length, request);
                return await dataService.DeleteDishesAsync(items.ToArray());
            }
        }

        public static async Task<DishModel> CreateDishModelAsync(Dish source, bool includeAllFields)
        {
            var model = new DishModel()
            {
                Id = source.Id,
                CreatedOn = source.CreatedOn,
                LastModifiedOn = source.LastModifiedOn,
                Name = source.Name,
                Description = source.Description,
                MenuFolderGuid = source.MenuFolderGuid,

                Price = source.Price,
                TaxTypeGuid = source.TaxTypeGuid,

                Barcode = source.Barcode,
                EnergyValue = source.EnergyValue,
                UnitCount = source.UnitCount,
                RowGuid = source.RowGuid,

                Thumbnail = source.Thumbnail,
                ThumbnailSource = await BitmapTools.LoadBitmapAsync(source.Thumbnail)
            };

            if (source.TaxType != null)
            {
                model.TaxType = new TaxTypeModel
                {
                    Id = source.TaxType.Id,
                    RowGuid = source.TaxType.RowGuid,
                    Name = source.TaxType.Name,
                    Rate = source.TaxType.Rate
                };
            }

            if (includeAllFields)
            {
                model.Picture = source.Picture;
                model.PictureSource = await BitmapTools.LoadBitmapAsync(source.Picture);
            }
            return model;
        }

        private async Task<DishModel> GetDishDataAsync(IDataService dataService, int id)
        {
            var item = await dataService.GetDishAsync(id);
            if (item != null)
            {
                return await CreateDishModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        private async Task<DishModel> GetDishDataAsync(IDataService dataService, Guid dishGuid)
        {
            var item = await dataService.GetDishAsync(dishGuid);
            if (item != null)
            {
                return await CreateDishModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        private void UpdateDishFromModel(Dish target, DishModel source)
        {
            target.CreatedOn = source.CreatedOn;
            target.LastModifiedOn = source.LastModifiedOn;
            target.Name = source.Name;
            target.Description = source.Description;
            target.MenuFolderGuid = source.MenuFolderGuid;
            target.Price = source.Price;
            target.TaxTypeGuid = source.TaxTypeGuid;
            target.Barcode = source.Barcode;
            target.EnergyValue = source.EnergyValue;
            target.UnitCount = source.UnitCount;
            target.RowGuid = source.RowGuid;
            target.Picture = source.Picture;
            target.Thumbnail = source.Thumbnail;
        }
    }
}
