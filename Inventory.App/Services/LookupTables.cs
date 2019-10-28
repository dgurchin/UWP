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

using Inventory.Models;

namespace Inventory.Services
{
    public class LookupTables : ILookupTables
    {
        #region Properties
        public ILogService LogService { get; }
        public IDataServiceFactory DataServiceFactory { get; }
        public IList<MenuFolderModel> MenuFolders { get; private set; }
        public IList<OrderStatusModel> OrderStatuses { get; private set; }
        public IList<DeliveryTypeModel> DeliveryTypes { get; private set; }
        public IList<SourceModel> OrderSources { get; private set; }
        public IList<OrderTypeModel> OrderTypes { get; private set; }
        public IList<PaymentTypeModel> PaymentTypes { get; private set; }
        public IList<RestaurantModel> Restaurants { get; private set; }
        public IList<TaxTypeModel> TaxTypes { get; private set; }
        public IList<CityModel> Cities { get; private set; }
        public IList<StreetModel> Streets { get; private set; }
        public IList<OrderStatusSequenceModel> OrderStatusSequences { get; private set; }
        #endregion

        #region Ctor
        public LookupTables(ILogService logService, IDataServiceFactory dataServiceFactory)
        {
            LogService = logService;
            DataServiceFactory = dataServiceFactory;
        }
        #endregion

        #region GetNameById methods
        public string GetMenuFolder(int id)
        {
            return MenuFolders.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetCity(int id)
        {
            return Cities.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetStreet(int id)
        {
            return Streets.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetCityAsync(int id)
        {
            return Cities.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetOrderStatus(int id)
        {
            return OrderStatuses.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }
        public string GetOrderType(int id)
        {
            return OrderTypes.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
        }
        public string GetOrderSource(int id)
        {
            return OrderSources.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }
        public string GetDeliveryType(int id)
        {
            return DeliveryTypes.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetPaymentType(int? id)
        {
            return id == null ? "" : PaymentTypes.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetRestaurant(int? id)
        {
            return id == null ? "" : Restaurants.Where(r => r.Id == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetTaxDesc(int id)
        {
            return TaxTypes.Where(r => r.Id == id).Select(r => $"{r.Rate} %").FirstOrDefault();
        }
        public decimal GetTaxRate(int id)
        {
            return TaxTypes.Where(r => r.Id == id).Select(r => r.Rate).FirstOrDefault();
        }
        #endregion

        #region Public methods

        public async Task InitializeAsync()
        {
            MenuFolders = await GetMenuFoldersAsync();
            OrderStatuses = await GetOrderStatusesAsync();
            DeliveryTypes = await GetDeliveryTypesAsync();
            OrderSources = await GetOrderSourcesAsync();
            OrderTypes = await GetOrderTypesAsync();
            PaymentTypes = await GetPaymentTypesAsync();
            Restaurants = await GetRestaurantsAsync();
            TaxTypes = await GetTaxTypesAsync();
            Cities = await GetCitiesAsync();
            Streets = await GetStreetsAsync();
            OrderStatusSequences = await GetOrderStatusSequencesAsync();
        }

        public IList<OrderStatusModel> GetAllowedOrderStatuses(int? statusIdBeginning)
        {
            int? nullStatusId = statusIdBeginning == 0 ? null : statusIdBeginning;
            var sequenceList = OrderStatusSequences
                .Where(r => r.StatusIdBeginning == nullStatusId)
                .Select(s => s.StatusIdEnd)
                .ToList();

            //Добавляется к списку возможных переходов текущий статус
            sequenceList.Add(statusIdBeginning);
            return OrderStatuses.Where(x => sequenceList.Contains(x.Id)).ToList();
        }

        /// <summary>
        /// Список улиц населенного пункта
        /// </summary>
        /// <param name="deliveredCityId"></param>
        /// <returns></returns>
        public IList<StreetModel> GetStreets(int? deliveredCityId)
        {
            return Streets.Where(r => r.CityId == deliveredCityId).ToList();
        }

        #endregion

        #region Private methods
        private async Task<IList<CityModel>> GetCitiesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetCitiesAsync();
                    return items.Select(r => new CityModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Загрузка населенных пунктов", ex);
            }
            return new List<CityModel>();
        }

        private async Task<IList<StreetModel>> GetStreetsAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetStreetsAsync();
                    return items.Select(r => new StreetModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        StreetTypeId = r.StreetTypeId,
                        CityId = r.CityId
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Загрузка улиц пунктов", ex);
            }
            return new List<StreetModel>();
        }

        private async Task<IList<MenuFolderModel>> GetMenuFoldersAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetMenuFoldersAsync();
                    return items.Select(folder => new MenuFolderModel
                    {
                        Id = folder.Id,
                        RowGuid = folder.RowGuid,
                        ParentGuid = folder.ParentGuid,
                        SequenceNumber = folder.RowNumber,
                        Name = folder.Name,
                        Description = folder.Description,
                        Picture = folder.Picture,
                        Thumbnail = folder.Thumbnail
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load Categories", ex);
            }
            return new List<MenuFolderModel>();
        }

        private async Task<IList<SourceModel>> GetOrderSourcesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetOrderSourcesAsync();
                    return items.Select(r => new SourceModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Загрузка источников информации", ex);
            }
            return new List<SourceModel>();
        }

        private async Task<IList<OrderTypeModel>> GetOrderTypesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetOrderTypesAsync();
                    return items.Select(r => new OrderTypeModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Загрузка типов заказа", ex);
            }
            return new List<OrderTypeModel>();
        }

        private async Task<IList<DeliveryTypeModel>> GetDeliveryTypesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetDeliveryTypesAsync();
                    return items.Select(r => new DeliveryTypeModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Загрузка типов доставки", ex);
            }
            return new List<DeliveryTypeModel>();
        }

        private async Task<IList<OrderStatusModel>> GetOrderStatusesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetOrderStatusesAsync();
                    var result = items.Select(r => new OrderStatusModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ColorStatus = r.ColorStatus
                    })
                    .ToList();

                    var statAll = new OrderStatusModel
                    {
                        Id = 0,
                        Name = "Все",
                        ColorStatus = "White"
                    };
                    result.Insert(0, statAll);

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load OrderStatus", ex);
            }
            return new List<OrderStatusModel>();
        }  

        private async Task<IList<PaymentTypeModel>> GetPaymentTypesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetPaymentTypesAsync();
                    return items.Select(r => new PaymentTypeModel
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load PaymentTypes", ex);
            }
            return new List<PaymentTypeModel>();
        }

        private async Task<IList<RestaurantModel>> GetRestaurantsAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetRestaurantsAsync();
                    return items.Select(r => new RestaurantModel
                    {
                        Id = r.Id,
                        Manufacturer = r.Manufacturer,
                        Name = r.Name,
                        Phone = r.Phone
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load Restaurants", ex);
            }
            return new List<RestaurantModel>();
        }

        private async Task<IList<TaxTypeModel>> GetTaxTypesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetTaxTypesAsync();
                    return items.Select(r => new TaxTypeModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Rate = r.Rate
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load TaxTypes", ex);
            }
            return new List<TaxTypeModel>();
        }

        private async Task<IList<OrderStatusSequenceModel>> GetOrderStatusSequencesAsync()
        {
            try
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    var items = await dataService.GetOrderStatusSequenceAsync();
                    var result = items.Select(r => new OrderStatusSequenceModel
                    {
                        Id = r.Id,
                        StatusIdBeginning = r.StatusIdBeginning,
                        StatusIdEnd = r.StatusIdEnd
                    })
                    .ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogException("LookupTables", "Load OrderStatusSequence", ex);
            }
            return new List<OrderStatusSequenceModel>();
        }

        private async void LogException(string source, string action, Exception exception)
        {
            await LogService.WriteAsync(Data.LogType.Error, source, action, exception.Message, exception.ToString());
        }

        #endregion
    }
}
