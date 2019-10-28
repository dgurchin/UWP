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
    abstract public partial class DataServiceBase : IDataService, IDisposable
    {
        #region Private fields
        private readonly IDataSource _dataSource = null;
        #endregion

        #region Ctor
        public DataServiceBase(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        #endregion

        #region Dictionaries

        public async Task<MenuFolder> GetMenuFolderAsync(int? id)
        {
            return await _dataSource.MenuFolders.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<MenuFolder> GetMenuFolderAsync(Guid rowGuid)
        {
            return await _dataSource.MenuFolders.FirstOrDefaultAsync(x => x.RowGuid == rowGuid);
        }
        public async Task<IList<MenuFolder>> GetMenuFoldersAsync()
        {
            return await _dataSource.MenuFolders.ToListAsync();
        }

        public async Task<OrderStatus> GetOrderStatusAsync(int? id)
        {
            return await _dataSource.OrderStatuses.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<OrderStatus>> GetOrderStatusesAsync()
        {
            return await _dataSource.OrderStatuses.ToListAsync();
        }

        public async Task<DeliveryType> GetDeliveryTypeAsync(int? id)
        {
            return await _dataSource.DeliveryTypes.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<DeliveryType>> GetDeliveryTypesAsync()
        {
            return await _dataSource.DeliveryTypes.ToListAsync();
        }

        public async Task<IList<City>> GetCitiesAsync()
        {
            return await _dataSource.Cities.ToListAsync();
        }
        public async Task<City> GetCityAsync(int? id)
        {
            return await _dataSource.Cities.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<OrderType> GetOrderTypeAsync(int? id)
        {
            return await _dataSource.OrderTypes.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<OrderType>> GetOrderTypesAsync()
        {
            return await _dataSource.OrderTypes.ToListAsync();
        }

        public async Task<Source> GetOrderSourceAsync(int? id)
        {
            return await _dataSource.Sources.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<Source>> GetOrderSourcesAsync()
        {
            return await _dataSource.Sources.ToListAsync();
        }

        public async Task<PaymentType> GetPaymentTypeAsync(int? id)
        {
            return await _dataSource.PaymentTypes.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<PaymentType>> GetPaymentTypesAsync()
        {
            return await _dataSource.PaymentTypes.ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantAsync(int? id)
        {
            return await _dataSource.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<Restaurant>> GetRestaurantsAsync()
        {
            return await _dataSource.Restaurants.ToListAsync();
        }

        public async Task<TaxType> GetTaxTypeAsync(int? id)
        {
            return await _dataSource.TaxTypes.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<TaxType>> GetTaxTypesAsync()
        {
            return await _dataSource.TaxTypes.ToListAsync();
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dataSource != null)
                {
                    _dataSource.Dispose();
                }
            }
        }

        #endregion
    }
}
