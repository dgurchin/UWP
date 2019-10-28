using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class DishRecommendService : IDishRecommendService
    {
        private IDataServiceFactory DataServiceFactory { get; }

        public DishRecommendService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public async Task<IList<DishRecommendModel>> GetDishRecommendsAsync(Guid dishGuid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = new List<DishRecommendModel>();
                var entities = await dataService.GetDishRecommendsAsync(dishGuid);
                foreach (var entity in entities)
                {
                    models.Add(await CreateModel(entity));
                }
                return models;
            }
        }

        public async Task<IList<OrderRecommendModel>> GetOrderRecommendsAsync(Guid dishGuid)
        {
            var models = new List<OrderRecommendModel>();
            var recommends = await GetDishRecommendsAsync(dishGuid);
            foreach (var recommend in recommends)
            {
                models.Add(new OrderRecommendModel(recommend));
            }
            return models;
        }
        
        private async Task<DishRecommendModel> CreateModel(DishRecommend entity)
        {
            if (entity == null)
                return null;

            return new DishRecommendModel
            {
                Id = entity.Id,
                RecommendGuid = entity.RecommendGuid,
                Recommend = await DishService.CreateDishModelAsync(entity.RecommendDish, false),
                DishGuid = entity.DishGuid,
                RowPosition = entity.RowPosition
            };
        }
    }
}
