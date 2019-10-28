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

using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System;

namespace Inventory.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<IList<OrderDish>> GetOrderDishesAsync(Guid orderGuid)
        {
            DataRequest<OrderDish> request = new DataRequest<OrderDish> { Where = (r) => r.OrderGuid == orderGuid };
            IQueryable<OrderDish> items = GetOrderDishes(request);

            // Execute
            var records = await items
                .Include(orderDish => orderDish.Dish)
                .AsNoTracking()
                .ToListAsync();
            return records;
        }

        public async Task<OrderDish> GetOrderDishAsync(int id)
        {
            return await _dataSource.OrderDishes
                .Include(orderDish => orderDish.Dish)
                .FirstOrDefaultAsync(orderDish => orderDish.Id == id);
        }

        public async Task<IList<OrderDish>> GetOrderDishesAsync(int skip, int take, DataRequest<OrderDish> request)
        {
            IQueryable<OrderDish> items = GetOrderDishes(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Include(orderDish => orderDish.Dish)
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<IList<OrderDish>> GetOrderDishKeysAsync(int skip, int take, DataRequest<OrderDish> request)
        {
            IQueryable<OrderDish> items = GetOrderDishes(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Select(x => new OrderDish
                {
                    Id = x.Id,
                    DishGuid = x.DishGuid,
                    OrderGuid = x.OrderGuid,
                    TaxTypeId = x.TaxTypeId
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<OrderDish> GetOrderDishes(DataRequest<OrderDish> request)
        {
            IQueryable<OrderDish> items = _dataSource.OrderDishes
                .Include(orderDish => orderDish.Dish)
                .Include(orderDish => orderDish.OrderDishGarnishes)
                .Include(orderDish => orderDish.OrderDishIngredients);

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

        public async Task<int> GetOrderDishesCountAsync(DataRequest<OrderDish> request)
        {
            IQueryable<OrderDish> items = _dataSource.OrderDishes
                .Include(orderDish => orderDish.Dish);

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

        public async Task<int> UpdateOrderDishAsync(OrderDish orderDish)
        {
            if (orderDish.Id > 0)
            {
                _dataSource.Entry(orderDish).State = EntityState.Modified;
            }
            else
            {
                // TODO: 
                //orderItem.CreateOn = DateTime.UtcNow;
                _dataSource.Entry(orderDish).State = EntityState.Added;
            }
            // TODO: 
            //orderItem.LastModifiedOn = DateTime.UtcNow;
            //orderItem.SearchTerms = orderItem.BuildSearchTerms();
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderDishesAsync(params OrderDish[] orderDishes)
        {
            _dataSource.OrderDishes.RemoveRange(orderDishes);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
