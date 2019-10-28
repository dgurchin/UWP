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

using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Services
{
    public interface ILookupTables
    {
        Task InitializeAsync();

        IList<MenuFolderModel> MenuFolders { get; }
        IList<OrderStatusModel> OrderStatuses { get; }
        IList<OrderTypeModel> OrderTypes { get; }
        IList<PaymentTypeModel> PaymentTypes { get; }
        IList<RestaurantModel> Restaurants { get; }
        IList<TaxTypeModel> TaxTypes { get; }
        IList<DeliveryTypeModel> DeliveryTypes { get; }
        IList<SourceModel> OrderSources { get; }
        IList<CityModel> Cities { get; }
        IList<StreetModel> Streets { get; }
        IList<OrderStatusSequenceModel> OrderStatusSequences { get; }
        IList<OrderStatusModel> GetAllowedOrderStatuses(int? statusIdBeginning);
        IList<StreetModel> GetStreets(int? cityId);

        string GetMenuFolder(int id);
        string GetOrderStatus(int id);
        string GetOrderType(int id);
        string GetPaymentType(int? id);
        string GetRestaurant(int? id);
        string GetTaxDesc(int id);
        decimal GetTaxRate(int id);
        string GetDeliveryType(int id);
        string GetOrderSource(int id);
        string GetCity(int id);
        string GetStreet(int id);
    }

    public class LookupTablesProxy
    {
        static public ILookupTables Instance { get; set; }
    }
}
