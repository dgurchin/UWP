using Inventory.Data;
using Inventory.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class DishIngredientService : IDishIngredientService
    {
        private IDataServiceFactory DataServiceFactory { get; }

        public DishIngredientService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public async Task<DishIngredientModel> GetDishIngredientAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = await dataService.GetDishIngredientAsync(id);
                return await CreateModelAsync(entity, true);
            }
        }

        public async Task<DishIngredientModel> GetDishIngredientAsync(Guid dishIngredientGuid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = await dataService.GetDishIngredientAsync(dishIngredientGuid);
                return await CreateModelAsync(entity, true);
            }
        }

        public async Task<IList<DishIngredientModel>> GetDishIngredientsAsync(Guid dishGuid)
        {
            var models = new List<DishIngredientModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entities = await dataService.GetDishIngredientsAsync(dishGuid);
                foreach (var entity in entities)
                {
                    var model = await CreateModelAsync(entity, false);
                    models.Add(model);
                }
            }
            return models;
        }

        private async Task<DishIngredientModel> CreateModelAsync(DishIngredient source, bool includeAllFields)
        {
            var model = new DishIngredientModel()
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
