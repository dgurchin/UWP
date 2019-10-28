using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class StreetService : IStreetService
    {
        public IDataServiceFactory DataServiceFactory { get; }

        public StreetService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public async Task<StreetModel> GetStreetAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return CreateStreetModel(await dataService.GetStreetAsync(id));
            }
        }

        public async Task<IList<StreetModel>> GetStreetsAsync()
        {
            var models = new List<StreetModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetStreetsAsync();
                foreach (var item in items)
                {
                    models.Add(CreateStreetModel(item));
                }
                return models;
            }
        }

        public async Task<IList<StreetModel>> GetStreetsAsync(DataRequest<Street> request)
        {
            var models = new List<StreetModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetStreetsAsync(request);
                foreach (var item in items)
                {
                    models.Add(CreateStreetModel(item));
                }
                return models;
            }
        }

        public StreetModel CreateStreetModel(Street entity)
        {
            var model = new StreetModel
            {
                Id = entity.Id,
                Name = entity.Name,
                StreetTypeId = entity.StreetTypeId,
                CityId = entity.CityId
            };

            if (entity.StreetType != null)
            {
                model.StreetTypeModel = new StreetTypeModel
                {
                    Id = entity.StreetType.Id,
                    Name = entity.StreetType.Name,
                    NameShort = entity.StreetType.NameShort
                };
            }

            return model;
        }
    }
}
