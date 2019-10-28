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
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    partial class DataServiceBase
    {
        #region Customer

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _dataSource.Customers.Where(customer => customer.Id == id)
                .Include(customer => customer.Source)
                .FirstOrDefaultAsync();
        }
        public async Task<Customer> GetCustomerAsync(Guid rowGuid)
        {
            return await _dataSource.Customers.FirstOrDefaultAsync(x => x.RowGuid == rowGuid);
        }

        public async Task<IList<Customer>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request)
        {
            IQueryable<Customer> items = GetCustomers(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Include(customer => customer.Source)
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<IList<Customer>> GetCustomerKeysAsync(int skip, int take, DataRequest<Customer> request)
        {
            IQueryable<Customer> items = GetCustomers(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Select(r => new Customer
                {
                    Id = r.Id,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Customer> GetCustomers(DataRequest<Customer> request)
        {
            IQueryable<Customer> items = _dataSource.Customers
                .Include(customer => customer.Source);

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

        public async Task<int> GetCustomersCountAsync(DataRequest<Customer> request)
        {
            IQueryable<Customer> items = _dataSource.Customers
                .Include(customer => customer.Source);

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

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            if (customer.Id > 0)
            {
                _dataSource.Entry(customer).State = EntityState.Modified;
            }
            else
            {
                // TODO: UIDGenerator
                //customer.Id = UIDGenerator.Next();
                customer.CreatedOn = DateTime.UtcNow;
                _dataSource.Entry(customer).State = EntityState.Added;
            }
            customer.LastModifiedOn = DateTime.UtcNow;

            if (customer.SourceId <= 0)
                customer.SourceId = null;

            customer.SearchTerms = customer.BuildSearchTerms();
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteCustomersAsync(params Customer[] customers)
        {
            _dataSource.Customers.RemoveRange(customers);
            return await _dataSource.SaveChangesAsync();
        }

        #endregion

        #region Communication

        public async Task<Communication> GetCommunicationAsync(int id)
        {
            return await _dataSource.Communications
                .Include(communication => communication.Type)
                .FirstOrDefaultAsync(communication => communication.Id == id);
        }

        public async Task<IList<Communication>> GetCommunicationsAsync(int customerId)
        {
            return await _dataSource.Communications
                .Where(communication => communication.CustomerId == customerId)
                .Include(communication => communication.Type)
                .ToListAsync();
        }
        public async Task<Communication> GetCommunicationAsync(Guid communicationGuid)
        {
            return await _dataSource.Communications
                .Include(communication => communication.Type)
                .FirstOrDefaultAsync(communication => communication.RowGuid == communicationGuid);
        }

        public async Task<IList<Communication>> GetCommunicationsAsync(Guid customerGuid)
        {
            return await _dataSource.Communications
                .Where(communication => communication.CustomerGuid == customerGuid)
                .Include(communication => communication.Type)
                .ToListAsync();
        }

        public async Task<int> UpdateCommunicationAsync(Communication communication)
        {
            if (communication.Id > 0)
            {
                _dataSource.Entry(communication).State = EntityState.Modified;
            }
            else
            {
                _dataSource.Entry(communication).State = EntityState.Added;
            }
            int affectedRows = await _dataSource.SaveChangesAsync();
            return affectedRows;
        }

        public async Task<int> DeleteCommunicationsAsync(params Communication[] communications)
        {
            _dataSource.Communications.RemoveRange(communications);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<Communication> GetPrimaryCommunicationAsync(int customerId)
        {
            return await _dataSource.Communications
                .Where(communication => communication.CustomerId == customerId && communication.IsPrimary == true)
                .Include(communication => communication.Type)
                .FirstOrDefaultAsync();
        }

        public async Task<Communication> FindCommuncationAsync(int customerId, string inputPhone)
        {
            if (customerId <= 0 || string.IsNullOrWhiteSpace(inputPhone))
                return null;

            var phoneNumber = NormalizePhoneNumber(inputPhone);
            var communication = await GetCommunicationQuery(phoneNumber)
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.IsPrimary)
                .FirstOrDefaultAsync();
            return communication;
        }

        public async Task<IList<Customer>> FindCustomersByPhoneNumberAsync(string inputPhone)
        {
            if (string.IsNullOrWhiteSpace(inputPhone))
            {
                return new List<Customer>();
            }

            var phoneNumber = NormalizePhoneNumber(inputPhone);
            var contactPhones = await GetCommunicationQuery(phoneNumber).ToListAsync();

            var contactIds = contactPhones.Select(x => x.CustomerId).ToList();
            var customers = await _dataSource.Customers.Where(customer => contactIds.Contains(customer.Id)).ToListAsync();
            return customers;
        }

        #endregion

        #region Address

        public async Task<Address> GetAddressAsync(int id)
        {
            return await _dataSource.Addresses
                .Where(address => address.Id == id)
                .Include(address => address.City)
                .Include(address => address.Street)
                    .ThenInclude(street => street.StreetType)
                .FirstOrDefaultAsync();
        }
        public async Task<Address> GetAddressAsync(Guid addressGuid)
        {
            return await _dataSource.Addresses
                .Where(address => address.RowGuid == addressGuid)
                .Include(address => address.City)
                .Include(address => address.Street)
                    .ThenInclude(street => street.StreetType)
                .FirstOrDefaultAsync();
        }
        public async Task<IList<Address>> GetAddressesAsync(int customerId)
        {
            return await _dataSource.Addresses
                .Where(address => address.CustomerId == customerId)
                .Include(address => address.City)
                .Include(address => address.Street)
                    .ThenInclude(street => street.StreetType)
                .ToListAsync();
        }
        public async Task<IList<Address>> GetAddressesAsync(Guid customerGuid)
        {
            return await _dataSource.Addresses
                .Where(address => address.CustomerGuid == customerGuid)
                .Include(address => address.City)
                .Include(address => address.Street)
                    .ThenInclude(street => street.StreetType)
                .ToListAsync();
        }
        public async Task<int> GetAddressesCountAsync(int customerId)
        {
            return await _dataSource.Addresses
                .Where(address => address.CustomerId == customerId)
                .Include(address => address.City)
                .Include(address => address.Street)
                    .ThenInclude(street => street.StreetType)
                .CountAsync();
        }

        public async Task<Address> FindExistAddressAsync(Address address)
        {
            return await _dataSource.Addresses.FirstOrDefaultAsync(x =>
                x.CustomerId == address.CustomerId &&
                x.CityId == address.CityId &&
                x.Apartment == address.Apartment &&
                x.Entrance == address.Entrance &&
                x.Floor == address.Floor &&
                x.House == address.House &&
                x.Housing == address.Housing &&
                x.Intercom == address.Intercom &&
                x.StreetId == address.StreetId);
        }

        public async Task<int> UpdateAddressAsync(Address address)
        {
            if (address.Id > 0)
            {
                _dataSource.Entry(address).State = EntityState.Modified;
            }
            else
            {
                _dataSource.Entry(address).State = EntityState.Added;
            }

            if (address.StreetId > 0 && address.Street == null)
                address.Street = await _dataSource.Streets.Include(street => street.StreetType).FirstOrDefaultAsync(street => street.Id == address.StreetId);

            if (address.CityId > 0 && address.City == null)
                address.City = await _dataSource.Cities.FirstOrDefaultAsync(city => city.Id == address.CityId);

            address.BuildName();

            int affectedRows = await _dataSource.SaveChangesAsync();
            return affectedRows;
        }

        public async Task<int> DeleteAddressAsync(Address address)
        {
            if (address == null || address.Id <= 0)
            {
                return 0;
            }
            _dataSource.Addresses.Remove(address);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<Address> GetPrimaryOrFirstAddressAsync(int customerId)
        {
            return await _dataSource.Addresses
                .Where(address => address.CustomerId == customerId)
                .Include(address => address.City)
                .Include(address => address.Street)
                    .ThenInclude(street => street.StreetType)
                .OrderByDescending(address => address.IsPrimary)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Private methods
        private string NormalizePhoneNumber(string inputPhone)
        {
            var phoneNumber = inputPhone.Trim().Replace("+38", "").Replace("+", "").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
            return phoneNumber;
        }

        private IQueryable<Communication> GetCommunicationQuery(string phoneNumber)
        {
            return _dataSource.Communications
                .Include(x => x.Type)
                .Where(x => x.Name.Trim().Replace("+38", "").Replace("+", "").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "") == phoneNumber);
        }
        #endregion
    }
}
