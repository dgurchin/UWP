using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public interface IOrderStatusHistoryService
    {
        Task<OrderStatusHistoryModel> GetOrderStatusHistoryAsync(int orderId, int lineId);
        Task<IList<OrderStatusHistoryModel>> GetOrderStatusHistorysAsync(DataRequest<OrderStatusHistory> request);
        Task<IList<OrderStatusHistoryModel>> GetOrderStatusHistorysAsync(int skip, int take, DataRequest<OrderStatusHistory> request);
        Task<int> GetOrderStatusHistoryCountAsync(DataRequest<OrderStatusHistory> request);

        Task<int> UpdateOrderStatusHistoryAsync(OrderStatusHistoryModel model);

        Task<int> DeleteOrderStatusHistoryAsync(OrderStatusHistoryModel model);
        Task<int> DeleteOrderStatusHistoryRangeAsync(int index, int length, DataRequest<OrderStatusHistory> request);
    }
}
