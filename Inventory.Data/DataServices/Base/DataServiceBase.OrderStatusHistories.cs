using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<OrderStatusHistory> GetOrderStatusHistoryAsync(int orderId, int orderLine)
        {
            return await _dataSource.OrderStatusHistories
                .Where(r => r.OrderId == orderId && r.OrderLine == orderLine)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<OrderStatusHistory>> GetOrderStatusHistorysAsync(int skip, int take, DataRequest<OrderStatusHistory> request)
        {
            IQueryable<OrderStatusHistory> items = GetOrderStatusHistorys(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .AsNoTracking().ToListAsync();

            return records;
        }

        public async Task<IList<OrderStatusHistory>> GetOrderStatusHistorysKeysAsync(int skip, int take, DataRequest<OrderStatusHistory> request)
        {
            IQueryable<OrderStatusHistory> items = GetOrderStatusHistorys(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Select(r => new OrderStatusHistory
                {
                    OrderId = r.OrderId,
                    OrderLine = r.OrderLine
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<OrderStatusHistory> GetOrderStatusHistorys(DataRequest<OrderStatusHistory> request)
        {
            IQueryable<OrderStatusHistory> items = _dataSource.OrderStatusHistories;

            // Query
            // TODO: Not supported
            //if (!String.IsNullOrEmpty(request.Query))
            //{
            //    items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            //}

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Order By
            if (request.OrderBy != null)
            {
                items = items.OrderBy(request.OrderBy);
            }
            if (request.OrderByDesc != null)
            {
                items = items.OrderByDescending(request.OrderByDesc);
            }

            return items;
        }

        public async Task<int> GetOrderStatusHistoryCountAsync(DataRequest<OrderStatusHistory> request)
        {
            IQueryable<OrderStatusHistory> items = _dataSource.OrderStatusHistories;

            // Query
            // TODO: Not supported
            //if (!String.IsNullOrEmpty(request.Query))
            //{
            //    items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            //}

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            return await items.CountAsync();
        }

        public async Task<int> UpdateOrderStatusHistoryAsync(OrderStatusHistory orderStatusHistory)
        {
            if (orderStatusHistory.OrderLine > 0)
            {
                _dataSource.Entry(orderStatusHistory).State = EntityState.Modified;
            }
            else
            {
                orderStatusHistory.OrderLine = _dataSource.OrderStatusHistories
                    .Where(r => r.OrderId == orderStatusHistory.OrderId)
                    .Select(r => r.OrderLine)
                    .DefaultIfEmpty(0)
                    .Max() + 1;
                // TODO: 
                //OrderStatusHistory.CreateOn = DateTime.UtcNow;
                _dataSource.Entry(orderStatusHistory).State = EntityState.Added;
            }
            // TODO: 
            //OrderStatusHistory.LastModifiedOn = DateTime.UtcNow;
            //OrderStatusHistory.SearchTerms = OrderStatusHistory.BuildSearchTerms();
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderStatusHistoryAsync(params OrderStatusHistory[] OrderStatusHistory)
        {
            _dataSource.OrderStatusHistories.RemoveRange(OrderStatusHistory);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
