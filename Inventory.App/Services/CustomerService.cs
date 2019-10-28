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

using Inventory.Data;
using Inventory.Data.Services;
using Inventory.Models;
using Inventory.Models.Enums;

namespace Inventory.Services
{
    public class CustomerService : ICustomerService
    {
        #region Services
        private IDataServiceFactory DataServiceFactory { get; }
        private ILogService LogService { get; }
        #endregion

        #region Ctor
        public CustomerService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }
        #endregion

        #region ICustomerService
        public async Task<CustomerModel> GetCustomerAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetCustomerModelAsync(dataService, id);
            }
        }

        public async Task<IList<CustomerModel>> GetCustomersAsync(DataRequest<Customer> request)
        {
            var collection = new CustomerCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<CustomerModel>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request)
        {
            var models = new List<CustomerModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCustomersAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateCustomerModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetCustomersCountAsync(DataRequest<Customer> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetCustomersCountAsync(request);
            }
        }

        public async Task<int> UpdateCustomerAsync(CustomerModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var customer = model.Id > 0 ? await dataService.GetCustomerAsync(model.Id) : new Customer();
                if (customer != null)
                {
                    UpdateCustomerFromModel(customer, model);
                    await dataService.UpdateCustomerAsync(customer);
                    model.Merge(await GetCustomerModelAsync(dataService, customer.Id));
                }
                return 0;
            }
        }

        public async Task<int> DeleteCustomerAsync(CustomerModel model)
        {
            var customer = new Customer { Id = model.Id };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteCustomersAsync(customer);
            }
        }

        public async Task<int> DeleteCustomerRangeAsync(int index, int length, DataRequest<Customer> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCustomerKeysAsync(index, length, request);
                return await dataService.DeleteCustomersAsync(items.ToArray());
            }
        }

        public async Task<CustomerModel> AddNewCustomerAsync(CustomerModel customer)
        {
            customer.Id = 0;
            await UpdateCustomerAsync(customer);
            return customer;
        }

        public async Task<CustomerModel> CreateCustomerModelAsync(Customer source, bool includeAllFields)
        {
            var model = new CustomerModel()
            {
                RowGuid = source.RowGuid,
                Id = source.Id,
                CreatedOn = source.CreatedOn,
                LastModifiedOn = source.LastModifiedOn,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                BirthDate = source.BirthDate,
                SignOfConsent = source.SignOfConsent,
                SourceId = source.SourceId,
                IsBlockList = source.IsBlockList,
                Thumbnail = source.Thumbnail,
                ThumbnailSource = await BitmapTools.LoadBitmapAsync(source.Thumbnail)
            };
            if (includeAllFields)
            {
                model.Picture = source.Picture;
                model.PictureSource = await BitmapTools.LoadBitmapAsync(source.Picture);
            }
            if (source.SourceId > 0 && source.Source != null)
                model.Source = new SourceModel { Id = source.Source.Id, Name = source.Source.Name };

            return model;
        }
        #endregion

        #region Communication

        public async Task<CommunicationModel> GetCommunicationAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = await dataService.GetCommunicationAsync(id);
                var model = CreateCommunicationModel(entity);
                return model;
            }
        }

        public async Task<IList<CommunicationModel>> GetCommunicationsAsync(int customerId)
        {
            var models = new List<CommunicationModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCommunicationsAsync(customerId);
                foreach (var item in items)
                {
                    models.Add(CreateCommunicationModel(item));
                }
                return models;
            }
        }

        public async Task<int> UpdateCommunicationAsync(CommunicationModel communicationModel)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = CreateCommunicationEntity(communicationModel);
                int affectedRows = await dataService.UpdateCommunicationAsync(entity);
                communicationModel.Id = entity.Id;
                communicationModel.RowGuid = entity.RowGuid;
                communicationModel.CustomerGuid = entity.CustomerGuid;
                return affectedRows;
            }
        }

        public async Task<int> DeleteCommunicationAsync(CommunicationModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var communication = new Communication { Id = model.Id };
                return await dataService.DeleteCommunicationsAsync(communication);
            }
        }

        public async Task AddPhoneIfNotExistAsync(int customerId, string phoneNumber)
        {
            var communication = await FindCustomerCommuncationAsync(customerId, phoneNumber);
            if (communication == null)
            {
                var communications = await GetCommunicationsAsync(customerId);
                bool isPrimary = communications.Count == 0;
                var customermodel_ = await GetCustomerAsync(customerId);

                communication = new CommunicationModel
                {
                    RowGuid = Guid.NewGuid(),
                    CustomerGuid = customermodel_.RowGuid,
                    CustomerId = customerId,
                    Name = phoneNumber,
                    TypeId = (int)CommunicationTypeEnum.Mobile,
                    IsPrimary = isPrimary
                };
                await UpdateCommunicationAsync(communication);
            }
        }

        public async Task<CommunicationModel> GetPrimaryCommunicationAsync(int customerId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var item = await dataService.GetPrimaryCommunicationAsync(customerId);
                var model = CreateCommunicationModel(item);
                return model;
            }
        }

        public async Task<CommunicationModel> FindCustomerCommuncationAsync(int customerId, string inputPhone)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var item = await dataService.FindCommuncationAsync(customerId, inputPhone);
                return CreateCommunicationModel(item);
            }
        }

        public async Task<IList<CustomerModel>> FindCustomersByPhoneNumberAsync(string phoneNumber)
        {
            var models = new List<CustomerModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.FindCustomersByPhoneNumberAsync(phoneNumber);
                foreach (var item in items)
                {
                    models.Add(await CreateCustomerModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        #endregion

        #region Address
        public async Task<IList<AddressModel>> GetAddressesAsync(int customerId)
        {
            var models = new List<AddressModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetAddressesAsync(customerId);
                foreach (var item in items)
                {
                    models.Add(CreateAddressModel(item));
                }
                return models;
            }
        }

        public async Task<int> GetAddressesCountAsync(int customerId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetAddressesCountAsync(customerId);
            }
        }

        public async Task<int> UpdateAddressAsync(AddressModel addressModel)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var address = addressModel.Id > 0 ? await dataService.GetAddressAsync(addressModel.Id) : new Address();
                if (address != null)
                {
                    UpdateAddressFromModel(address, addressModel);
                    await dataService.UpdateAddressAsync(address);
                    // TODO: Взять еще раз с базы?
                    addressModel.Merge(CreateAddressModel(address));
                }
                return 0;
            }
        }

        public async Task<int> DeleteAddressAsync(AddressModel addressModel)
        {
            var address = new Address { Id = addressModel.Id };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteAddressAsync(address);
            }
        }

        public async Task<AddressModel> FindExistAddressAsync(AddressModel addressModel)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var item = await dataService.FindExistAddressAsync(CreateAddressEntity(addressModel));
                return CreateAddressModel(item);
            }
        }

        public async Task AddAddressIfNotExistAsync(int customerId, AddressModel addressModel)
        {
            if (addressModel.CustomerId != customerId)
            {
                addressModel.CustomerId = customerId;
            }

            var existAddress = await FindExistAddressAsync(addressModel);
            if (existAddress == null)
            {
                if (await GetAddressesCountAsync(customerId) == 0)
                {
                    addressModel.IsPrimary = true;
                }
                addressModel.RowGuid = Guid.NewGuid();
                var customermodel_ = await GetCustomerAsync(customerId);

                addressModel.CustomerGuid = customermodel_.RowGuid;
                await UpdateAddressAsync(addressModel);
            }
        }

        public async Task<AddressModel> GetPrimaryOrFirstAddressAsync(int customerId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = await dataService.GetPrimaryOrFirstAddressAsync(customerId);
                var model = CreateAddressModel(entity);
                return model;
            }
        }
        #endregion

        #region Private methods
        private async Task<CustomerModel> GetCustomerModelAsync(IDataService dataService, int id)
        {
            var item = await dataService.GetCustomerAsync(id);
            if (item != null)
            {
                return await CreateCustomerModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        private void UpdateCustomerFromModel(Customer target, CustomerModel source)
        {
            target.CreatedOn = source.CreatedOn;
            target.LastModifiedOn = source.LastModifiedOn;
            target.RowGuid = source.RowGuid;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.BirthDate = source.BirthDate;
            target.SignOfConsent = source.SignOfConsent;
            target.SourceId = source.SourceId;
            target.IsBlockList = source.IsBlockList;
            target.Picture = source.Picture;
            target.Thumbnail = source.Thumbnail;
        }

        private void UpdateAddressFromModel(Address entity, AddressModel model)
        {
            entity.Apartment = model.Apartment;
            entity.CityId = model.CityId;
            entity.RowGuid = model.RowGuid;
            entity.CustomerGuid = model.CustomerGuid;
            entity.CustomerId = model.CustomerId;
            entity.Entrance = model.Entrance;
            entity.Floor = model.Floor;
            entity.House = model.House;
            entity.Housing = model.Housing;
            entity.Intercom = model.Intercom;
            entity.IsPrimary = model.IsPrimary;
            entity.Name = model.Name;
            entity.StreetId = model.StreetId;
        }

        private CommunicationModel CreateCommunicationModel(Communication item)
        {
            if (item == null)
            {
                return null;
            }

            var model = new CommunicationModel
            {
                Id = item.Id,
                Name = item.Name,
                RowGuid = item.RowGuid,
                CustomerId = item.CustomerId,
                CustomerGuid = item.CustomerGuid,
                TypeId = item.TypeId,
                IsPrimary = item.IsPrimary
            };
            return model;
        }

        private Communication CreateCommunicationEntity(CommunicationModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new Communication
            {
                Id = model.Id,
                Name = model.Name,
                RowGuid = model.RowGuid,
                CustomerId = model.CustomerId,
                CustomerGuid = model.CustomerGuid,
                TypeId = model.TypeId,
                IsPrimary = model.IsPrimary
            };
        }

        private AddressModel CreateAddressModel(Address entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new AddressModel
            {
                Id = entity.Id,
                RowGuid = entity.RowGuid,
                CustomerId = entity.CustomerId,
                CustomerGuid = entity.CustomerGuid,

                Name = entity.Name,
                CityId = entity.CityId,
                StreetId = entity.StreetId,

                House = entity.House,
                Apartment = entity.Apartment,
                Floor = entity.Floor,
                Entrance = entity.Entrance,
                Housing = entity.Housing,
                Intercom = entity.Intercom,
                IsPrimary = entity.IsPrimary
            };

            if (entity.City != null)
            {
                model.City = new CityModel
                {
                    Id = entity.City.Id,
                    Name = entity.City.Name
                };
            }

            if (entity.Street != null)
            {
                model.Street = new StreetModel
                {
                    Id = entity.Street.Id,
                    Name = entity.Street.Name,
                    CityId = entity.Street.CityId,
                    StreetTypeId = entity.Street.StreetTypeId
                };

                if (entity.Street.StreetType != null)
                {
                    model.Street.StreetTypeModel = new StreetTypeModel
                    {
                        Id = entity.Street.StreetType.Id,
                        Name = entity.Street.StreetType.Name,
                        NameShort = entity.Street.StreetType.NameShort
                    };
                }
            }

            return model;
        }

        private Address CreateAddressEntity(AddressModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new Address
            {
                RowGuid = model.RowGuid,
                CustomerGuid = model.CustomerGuid,
                Id = model.Id,
                Apartment = model.Apartment,
                Floor = model.Floor,
                Entrance = model.Entrance,
                CityId = model.CityId,
                CustomerId = model.CustomerId,
                House = model.House,
                Housing = model.Housing,
                Intercom = model.Intercom,
                IsPrimary = model.IsPrimary,
                Name = model.Name,
                StreetId = model.StreetId
            };
        }
        #endregion
    }
}
