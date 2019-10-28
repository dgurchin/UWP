using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Services;
using Inventory.Models;

namespace Inventory.Services
{
    public class OrderStatusHistoryService : IOrderStatusHistoryService
    {
        public OrderStatusHistoryService(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public IDataServiceFactory DataServiceFactory { get; }

        public async Task<OrderStatusHistoryModel> GetOrderStatusHistoryAsync(int orderID, int lineID)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetOrderStatusHistoryAsync(orderID, lineID);
            }
        }

        static private async Task<OrderStatusHistoryModel> GetOrderStatusHistoryAsync(IDataService dataService, int orderId, int lineID)
        {
            var item = await dataService.GetOrderStatusHistoryAsync(orderId, lineID);
            if (item != null)
            {
                return CreateOrderStatusHistoryModel(item, includeAllFields: true);
            }
            return null;
        }

        public Task<IList<OrderStatusHistoryModel>> GetOrderStatusHistorysAsync(DataRequest<OrderStatusHistory> request)
        {
            // OrderItems are not virtualized
            return GetOrderStatusHistorysAsync(0, 100, request);
        }

        public async Task<IList<OrderStatusHistoryModel>> GetOrderStatusHistorysAsync(int skip, int take, DataRequest<OrderStatusHistory> request)
        {
            var models = new List<OrderStatusHistoryModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetOrderStatusHistorysAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateOrderStatusHistoryModel(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetOrderStatusHistoryCountAsync(DataRequest<OrderStatusHistory> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetOrderStatusHistoryCountAsync(request);
            }
        }

        public async Task<int> UpdateOrderStatusHistoryAsync(OrderStatusHistoryModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var orderStatusHistoryItem = model.OrderLine > 0 
                    ? await dataService.GetOrderStatusHistoryAsync(model.OrderId, model.OrderLine) 
                    : new OrderStatusHistory();
                if (orderStatusHistoryItem != null)
                {
                    UpdateOrderStatusHistoryFromModel(orderStatusHistoryItem, model);
                    await dataService.UpdateOrderStatusHistoryAsync(orderStatusHistoryItem);
                    model.Merge(await GetOrderStatusHistoryAsync(dataService, orderStatusHistoryItem.OrderId, orderStatusHistoryItem.OrderLine));
                }
                return 0;
            }
        }

        public async Task<int> DeleteOrderStatusHistoryAsync(OrderStatusHistoryModel model)
        {
            var orderStatusHistory = new OrderStatusHistory { OrderId = model.OrderId, OrderLine = model.OrderLine };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteOrderStatusHistoryAsync(orderStatusHistory);
            }
        }

        public async Task<int> DeleteOrderStatusHistoryRangeAsync(int index, int length, DataRequest<OrderStatusHistory> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetOrderStatusHistorysKeysAsync(index, length, request);
                return await dataService.DeleteOrderStatusHistoryAsync(items.ToArray());
            }
        }

        private static OrderStatusHistoryModel CreateOrderStatusHistoryModel(OrderStatusHistory source, bool includeAllFields)
        {
            var model = new OrderStatusHistoryModel()
            {
                Id = source.Id,
                OrderId = source.OrderId,
                OrderLine = source.OrderLine,
                StatusIdBeginning = source.StatusIdBeginning,
                StatusIdEnd = source.StatusIdEnd,
                StatusUser = source.StatusUser,
                StatusDate = source.StatusDate,
                Comment = source.Comment
            };
            return model;
        }

        private void UpdateOrderStatusHistoryFromModel(OrderStatusHistory target, OrderStatusHistoryModel source)
        {
            target.OrderId = source.OrderId;
            target.OrderLine = source.OrderLine;
            target.StatusIdBeginning = source.StatusIdBeginning;
            target.StatusIdEnd = source.StatusIdEnd;
            target.StatusUser = source.StatusUser;
            target.StatusDate = source.StatusDate;
            target.Comment = source.Comment;
        }
    }
}

