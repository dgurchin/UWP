using Inventory.Data;
using Inventory.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class DishGarnishService : IDishGarnishService
    {
        private IDataServiceFactory DataServiceFactory { get; }

        public DishGarnishService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public async Task<DishGarnishModel> GetDishGarnishAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = await dataService.GetDishGarnishAsync(id);
                return await CreateModelAsync(entity, true);
            }
        }

        public async Task<DishGarnishModel> GetDishGarnishAsync(Guid dishGarnishGuid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = await dataService.GetDishGarnishAsync(dishGarnishGuid);
                return await CreateModelAsync(entity, true);
            }
        }

        public async Task<IList<DishGarnishModel>> GetDishGarnishesAsync(Guid dishGuid)
        {
            var models = new List<DishGarnishModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entities = await dataService.GetDishGarnishesAsync(dishGuid);
                foreach (var entity in entities)
                {
                    var model = await CreateModelAsync(entity, false);
                    models.Add(model);
                }
            }
            return models;
        }

        private async Task<DishGarnishModel> CreateModelAsync(DishGarnish source, bool includeAllFields)
        {
            var model = new DishGarnishModel()
            {
                Id = source.Id,
                RowGuid = source.RowGuid,
                DishGuid = source.DishGuid,
                Name = source.Name,
                RowPosition = source.RowPosition,
                Price = source.Price,
                Thumbnail = source.Thumbnail,
                ThumbnailSource = await BitmapTools.LoadBitmapAsync(source.Thumbnail)
            };

            if (includeAllFields)
            {
                model.Picture = source.Picture;
                model.PictureSource = await BitmapTools.LoadBitmapAsync(source.Picture);
            }
            return model;
        }
    }
}
