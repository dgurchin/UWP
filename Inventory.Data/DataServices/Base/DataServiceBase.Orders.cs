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

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    partial class DataServiceBase
    {
        #region Order
        public async Task<Order> GetOrderAsync(Guid orderGuid)
        {
            return await _dataSource.Orders.Where(order => order.RowGuid == orderGuid)
                .Include(order => order.Customer)
                .Include(order => order.DeliveryType)
                .Include(order => order.Status)
                .Include(order => order.OrderType)
                .Include(order => order.Source)
                .Include(order => order.PaymentType)
                .Include(order => order.Restaurant)
                .Include(order => order.City)
                .Include(order => order.Street)
                    .ThenInclude(street => street.StreetType)
                .FirstOrDefaultAsync();
        }
        public async Task<Order> GetOrderAsync(int orderId)
        {
            return await _dataSource.Orders.Where(order => order.Id == orderId)
                .Include(order => order.Customer)
                .Include(order => order.DeliveryType)
                .Include(order => order.Status)
                .Include(order => order.OrderType)
                .Include(order => order.Source)
                .Include(order => order.PaymentType)
                .Include(order => order.Restaurant)
                .Include(order => order.City)
                .Include(order => order.Street)
                    .ThenInclude(street => street.StreetType)
                .FirstOrDefaultAsync();
        }
        public async Task<IList<Order>> GetOrdersAsync(DataRequest<Order> request)
        {
            IQueryable<Order> items = GetOrders(request);

            // Execute
            var records = await items
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<IList<Order>> GetOrdersAsync(int skip, int take, DataRequest<Order> request)
        {
            IQueryable<Order> items = GetOrders(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .AsNoTracking()
                .ToListAsync();

            return records;
        }


        public async Task<IList<Order>> GetOrderKeysAsync(int skip, int take, DataRequest<Order> request)
        {
            IQueryable<Order> items = GetOrders(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Select(r => new Order
                {
                    Id = r.Id,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Order> GetOrders(DataRequest<Order> request)
        {
            IQueryable<Order> items = _dataSource.Orders
                .Include(order => order.Restaurant)
                .Include(order => order.Status)
                .Include(order => order.OrderType)
                .Include(order => order.City)
                .Include(order => order.Street)
                    .ThenInclude(street => street.StreetType);

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }

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

        public async Task<int> GetOrdersCountAsync(DataRequest<Order> request)
        {
            IQueryable<Order> items = _dataSource.Orders
                .Include(order => order.Restaurant)
                .Include(order => order.Status)
                .Include(order => order.OrderType)
                .Include(order => order.City)
                .Include(order => order.Street)
                    .ThenInclude(street => street.StreetType);


            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            return await items.CountAsync();
        }

        public async Task<int> UpdateOrderAsync(Order order)
        {
            if (order.Id > 0)
            {
                _dataSource.Entry(order).State = EntityState.Modified;
            }
            else
            {
                // TODO: UIDGenerator
                //order.Id = UIDGenerator.Next(4);
                order.CreatedOn = DateTime.UtcNow;
                _dataSource.Entry(order).State = EntityState.Added;
            }
            if (order.RowGuid == Guid.Empty)
            {
                order.RowGuid = Guid.NewGuid();
            }
            order.LastModifiedOn = DateTime.UtcNow;

            if (order.SourceId > 0 && order.Source == null)
                order.Source = await GetOrderSourceAsync(order.SourceId);

            if (order.OrderTypeId > 0 && order.OrderType == null)
                order.OrderType = await GetOrderTypeAsync(order.OrderTypeId);

            if (order.StatusId > 0 && order.Status == null)
                order.Status = await GetOrderStatusAsync(order.StatusId);

            if (order.PaymentTypeId > 0 && order.PaymentType == null)
                order.PaymentType = await GetPaymentTypeAsync(order.PaymentTypeId);

            if (order.DeliveryTypeId > 0 && order.DeliveryType == null)
                order.DeliveryType = await GetDeliveryTypeAsync(order.DeliveryTypeId);

            if (order.RestaurantId > 0 && order.Restaurant == null)
                order.Restaurant = await GetRestaurantAsync(order.RestaurantId);

            if (order.CityId > 0 && order.City == null)
                order.City = await GetCityAsync(order.CityId);

            if (order.StreetId > 0 && order.Street == null)
                order.Street = await GetStreetAsync(order.StreetId);

            if (order.SourceId <= 0)
                order.SourceId = null;

            order.SearchTerms = order.BuildSearchTerms();
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteOrdersAsync(params Order[] orders)
        {
            _dataSource.Orders.RemoveRange(orders);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> BindDishesToOrderAsync(Guid orderGuid, int orderId)
        {
            var entities = await _dataSource.OrderDishes
                .Where(orderDish => orderDish.OrderGuid == orderGuid && (orderDish.OrderId == null || orderDish.OrderId <= 0))
                .ToListAsync();

            entities.ForEach(entity => entity.OrderId = orderId);
            return await _dataSource.SaveChangesAsync();
        }
        #endregion

        #region OrderDishGarnish
        public async Task<OrderDishGarnish> GetOrderDishGarnishAsync(int id)
        {
            return await _dataSource.OrderDishGarnishes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<OrderDishGarnish>> GetOrderDishGarnishesAsync(int orderDishId)
        {
            return await _dataSource.OrderDishGarnishes
                .Where(x => x.OrderDishId == orderDishId)
                .ToListAsync();
        }

        public async Task<int> UpdateOrderDishGarnishAsync(OrderDishGarnish garnish)
        {
            if (garnish.Id <= 0)
            {
                _dataSource.Entry(garnish).State = EntityState.Added;
            }
            else
            {
                _dataSource.Entry(garnish).State = EntityState.Modified;
            }
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderDishGarnishesAsync(params OrderDishGarnish[] garnishes)
        {
            _dataSource.OrderDishGarnishes.RemoveRange(garnishes);
            return await _dataSource.SaveChangesAsync();
        }
        #endregion

        #region OrderDishIngredient
        public async Task<OrderDishIngredient> GetOrderDishIngredientAsync(int id)
        {
            return await _dataSource.OrderDishIngredients.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<OrderDishIngredient>> GetOrderDishIngredientsAsync(int orderDishId)
        {
            return await _dataSource.OrderDishIngredients
                .Where(ingredient => ingredient.OrderDishId == orderDishId)
                .ToListAsync();
        }

        public async Task<int> UpdateOrderDishIngredientAsync(OrderDishIngredient ingredient)
        {
            if (ingredient.Id <= 0)
            {
                _dataSource.Entry(ingredient).State = EntityState.Added;
            }
            else
            {
                _dataSource.Entry(ingredient).State = EntityState.Modified;
            }
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderDishIngredientsAsync(params OrderDishIngredient[] ingredients)
        {
            _dataSource.OrderDishIngredients.RemoveRange(ingredients);
            return await _dataSource.SaveChangesAsync();
        }
        #endregion

        #region OrderStatusSequnce
        public async Task<IList<OrderStatusSequence>> GetOrderStatusSequenceAsync()
        {
            return await _dataSource.OrderStatusSequences.ToListAsync();
        }
        #endregion
    }
}
