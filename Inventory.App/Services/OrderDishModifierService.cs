using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class OrderDishModifierService : IOrderDishModifierService
    {
        private IDataServiceFactory DataServiceFactory { get; }

        public OrderDishModifierService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public async Task<IList<ModifierModel>> GetRelatedDishModifiersAsync(Guid dishGuid)
        {
            IList<ModifierModel> models = new List<ModifierModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entities = await dataService.GetRelatedDishModifiersAsync(dishGuid);
                foreach (var entity in entities)
                {
                    var model = CreateModifierModelFromEntity(entity);
                    models.Add(model);
                }
            }
            return models;
        }

        private ModifierModel CreateModifierModelFromEntity(Modifier entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ModifierModel
            {
                Id = entity.Id,
                RowGuid = entity.RowGuid,
                Name = entity.Name,
                IsRequired = entity.IsRequired
            };
        }
    }
}
