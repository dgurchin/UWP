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
using Inventory.Extensions;
using Inventory.Models;
using Inventory.Models.Enums;

namespace Inventory.Services
{
    public class OrderService : IOrderService
    {
        #region Services
        private IDataServiceFactory DataServiceFactory { get; }

        private ILogService LogService { get; }

        private IVariableService VariableService { get; }

        private ICustomerService CustomerService { get; }
        #endregion

        #region Ctor
        public OrderService(IDataServiceFactory dataServiceFactory, ICustomerService customerService, 
            IVariableService variableService, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            CustomerService = customerService;
            VariableService = variableService;
            LogService = logService;
        }
        #endregion

        #region IOrderService
        public OrderStatusEnum DefaultOrderStatus => OrderStatusEnum.Registration;

        public SourceEnum DefaultSource => SourceEnum.Phone;

        public decimal DefaultMinimumOrderSum => 100m;

        public async Task<OrderModel> GetOrderAsync(Guid orderGuid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetOrderAsync(dataService, orderGuid);
            }
        }

        public async Task<IList<OrderModel>> GetOrdersAsync(DataRequest<Order> request)
        {
            var collection = new OrderCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<OrderModel>> GetOrdersAsync(int skip, int take, DataRequest<Order> request)
        {
            var models = new List<OrderModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetOrdersAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateOrderModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetOrdersCountAsync(DataRequest<Order> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetOrdersCountAsync(request);
            }
        }

        public async Task<OrderModel> CreateNewOrderAsync(int customerId = 0, string phoneNumber = null)
        {
            var now = DateTime.Now;
            int deliveryIntervalFrom = await VariableService.GetVariableValueAsync<int>(VariableStrings.DeliverySoonIntervalFrom);
            var deliveryDate = now.AddMinutes(deliveryIntervalFrom);

            var model = new OrderModel
            {
                RowGuid = Guid.NewGuid(),
                CustomerId = customerId,
                CreatedOn = now,
                CreatedOnTime = now.DateTimeToTimeSpan().Value,
                DeliveryDate = deliveryDate,
                DeliveryTime = deliveryDate.DateTimeToTimeSpan().Value,
                DeliveryTypeId = (int)DeliveryTypeEnum.Soon,
                IsDeliveryDateReadOnly = true,
                StatusId = (int)DefaultOrderStatus,
                OrderTypeId = (int)OrderTypeEnum.Delivery,
                PaymentTypeId = (int)PaymentTypeEnum.Cash,
                CityId = (int)CityEnum.Kyiv,
                PhoneNumber = phoneNumber,
                SourceId = string.IsNullOrWhiteSpace(phoneNumber) ? 0 : (int)SourceEnum.Phone
            };

            if (customerId > 0)
            {
                model.Customer = await CustomerService.GetCustomerAsync(customerId);
            }

            return model;
        }

        public async Task<int> UpdateOrderAsync(OrderModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                if (model.IsNewCustomer)
                {
                    await AddNewCustomerAsync(model);
                }
                else
                {
                    int customerSourceId = ExtractCustomerSourceId(model);
                    if (model.SourceId > 0 && customerSourceId <= 0)
                    {
                        await UpdateCustomerSource(model.CustomerId, model.SourceId.Value);
                    }
                }
                await UpdateCustomerPhoneAsync(model);

                if (model.IsAddressRequired)
                {
                    await UpdateCustomerAddressAsync(model);
                }

                if (model.SourceId == null || model.SourceId <= 0)
                {
                    model.SourceId = (int)DefaultSource;
                }

                var order = model.IsNew == false ? await dataService.GetOrderAsync(model.RowGuid) : new Order();
                UpdateOrderFromModel(order, model);

                int affectedRows = await dataService.UpdateOrderAsync(order);
                model.Merge(await GetOrderAsync(dataService, order.RowGuid));

                await BindDishesToOrderAsync(model);

                return affectedRows;
            }
        }

        public async Task<int> DeleteOrderAsync(OrderModel model)
        {
            var order = new Order { Id = model.Id };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteOrdersAsync(order);
            }
        }

        public async Task<int> DeleteOrderRangeAsync(int index, int length, DataRequest<Order> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetOrderKeysAsync(index, length, request);
                return await dataService.DeleteOrdersAsync(items.ToArray());
            }
        }

        public async Task<int> BindDishesToOrderAsync(OrderModel model)
        {
            Guid orderGuid = model.RowGuid;
            int orderId = model.Id;

            if (orderId <= 0)
            {
                return 0;
            }

            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.BindDishesToOrderAsync(orderGuid, orderId);
            }
        }
        #endregion

        #region Private methods

        private async Task<OrderModel> GetOrderAsync(IDataService dataService, Guid orderGuid)
        {
            var item = await dataService.GetOrderAsync(orderGuid);
            if (item != null)
            {
                return await CreateOrderModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        private async Task<OrderModel> CreateOrderModelAsync(Order source, bool includeAllFields)
        {
            var model = new OrderModel()
            {
                Id = source.Id,
                RowGuid = source.RowGuid,
                CreatedOn = source.CreatedOn,
                CreatedOnTime = source.CreatedOn.GetTimeOfDayOrDefault(),
                CustomerId = source.CustomerId,
                SourceId = source.SourceId,
                OrderTypeId = source.OrderTypeId,
                DeliveryDate = source.DeliveryDate,
                DeliveryTime = source.DeliveryDate.GetTimeOfDayOrDefault(),
                DeliveryTypeId = source.DeliveryTypeId,
                StatusId = source.StatusId,
                RestaurantId = source.RestaurantId,
                PaymentTypeId = source.PaymentTypeId,
                Reason = source.Reason,
                IsConfirmation = source.IsConfirmation,
                Change = source.Change,
                CityId = source.CityId ?? 0,
                StreetId = source.StreetId ?? 0,
                Apartment = source.Apartment,
                Entrance = source.Entrance,
                Floor = source.Floor,
                House = source.House,
                Housing = source.Housing,
                Intercom = source.Intercom,
                PhoneNumber = source.PhoneNumber,
                NumOfPeople = source.NumOfPeople,
                CallOnArrival = source.CallOnArrival,
                Comment = source.Comment,
                CommentAddress = source.CommentAddress,
                CommentCustomer = source.CommentCustomer
            };

            if (source.Customer != null)
            {
                model.Customer = await CustomerService.CreateCustomerModelAsync(source.Customer, includeAllFields);
            }
            if (source.DeliveryType != null)
            {
                model.DeliveryType = new DeliveryTypeModel { Id = source.DeliveryType.Id, Name = source.DeliveryType.Name };
            }
            if (source.Source != null)
            {
                model.OrderSource = new SourceModel { Id = source.Source.Id, Name = source.Source.Name };
            }
            if (source.OrderType != null)
            {
                model.OrderType = new OrderTypeModel { Id = source.OrderType.Id, Name = source.OrderType.Name };
            }
            if (source.PaymentType != null)
            {
                model.PaymentType = new PaymentTypeModel { Id = source.PaymentType.Id, Name = source.PaymentType.Name };
            }
            if (source.Restaurant != null)
            {
                model.Restaurant = new RestaurantModel
                {
                    Id = source.Restaurant.Id,
                    Name = source.Restaurant.Name,
                    Manufacturer = source.Restaurant.Manufacturer,
                    Phone = source.Restaurant.Phone
                };
            }
            if (source.City != null)
            {
                model.City = new CityModel { Id = source.City.Id, Name = source.City.Name };
            }
            if (source.Street != null)
            {
                model.Street = new StreetModel
                {
                    Id = source.Street.Id,
                    Name = source.Street.Name,
                    CityId = source.Street.CityId,
                    StreetTypeId = source.Street.StreetTypeId,
                    StreetTypeModel = source.Street.StreetType != null
                        ? new StreetTypeModel
                        {
                            Id = source.Street.StreetType.Id,
                            Name = source.Street.StreetType.Name,
                            NameShort = source.Street.StreetType.NameShort
                        } : null
                };
            }
            if (source.Status != null)
            {
                model.Status = new OrderStatusModel
                {
                    Id = source.Status.Id,
                    Name = source.Status.Name,
                    ColorStatus = source.Status.ColorStatus
                };
            }
            return model;
        }

        private void UpdateOrderFromModel(Order target, OrderModel source)
        {
            target.RowGuid = source.RowGuid;
            target.CreatedOn = source.CreatedOn.CombineDateTimeOffsetWithTimeNotNull(source.CreatedOnTime);
            target.CustomerId = source.CustomerId;
            target.SourceId = source.SourceId;
            target.OrderTypeId = source.OrderTypeId;
            target.DeliveryDate = source.DeliveryDate.CombineDateTimeOffsetWithTime(source.DeliveryTime);
            target.DeliveryTypeId = source.DeliveryTypeId;
            target.StatusId = source.StatusId;
            target.RestaurantId = source.RestaurantId;
            target.PaymentTypeId = source.PaymentTypeId;
            target.Reason = source.Reason;
            target.IsConfirmation = source.IsConfirmation;
            target.Change = source.Change;
            target.CityId = source.CityId == 0 ? null : (int?)source.CityId;
            target.StreetId = source.StreetId == 0 ? null : (int?)source.StreetId;
            target.Apartment = source.Apartment;
            target.House = source.House;
            target.Housing = source.Housing;
            target.Intercom = source.Intercom;
            target.Floor = source.Floor;
            target.Entrance = source.Entrance;
            target.PhoneNumber = source.PhoneNumber;
            target.NumOfPeople = source.NumOfPeople;
            target.CallOnArrival = source.CallOnArrival;
            target.Comment = source.Comment;
            target.CommentAddress = source.CommentAddress;
            target.CommentCustomer = source.CommentCustomer;
        }

        private async Task AddNewCustomerAsync(OrderModel model)
        {
            CustomerModel customer = new CustomerModel
            {
                FirstName = model.EditableCustomerName,
                SourceId = model.SourceId
            };
            customer = await CustomerService.AddNewCustomerAsync(customer);
            model.CustomerId = customer.Id;
        }

        private int ExtractCustomerSourceId(OrderModel model)
        {
            return model.Customer?.SourceId ?? 0;
        }

        private async Task UpdateCustomerSource(int customerId, int sourceId)
        {
            var customer = await CustomerService.GetCustomerAsync(customerId);
            if (!customer.SourceId.HasValue || customer.SourceId <= 0)
            {
                customer.SourceId = sourceId;
            }
            await CustomerService.UpdateCustomerAsync(customer);
        }

        private async Task UpdateCustomerPhoneAsync(OrderModel model)
        {
            await CustomerService.AddPhoneIfNotExistAsync(model.CustomerId, model.PhoneNumber);
        }

        private async Task UpdateCustomerAddressAsync(OrderModel model)
        {
            var address = ExtractAddressModelFromOrder(model);
            await CustomerService.AddAddressIfNotExistAsync(model.CustomerId, address);
        }

        private AddressModel ExtractAddressModelFromOrder(OrderModel model)
        {
            var address = new AddressModel
            {
                Name = model.PhoneNumber,
                Apartment = model.Apartment,
                CityId = model.CityId,
                City = model.City,
                CustomerId = model.CustomerId,
                Entrance = model.Entrance,
                Floor = model.Floor,
                House = model.House,
                Housing = model.Housing,
                Intercom = model.Intercom,
                Street = model.Street,
                StreetId = model.StreetId
            };
            return address;
        }

        #endregion
    }
}
