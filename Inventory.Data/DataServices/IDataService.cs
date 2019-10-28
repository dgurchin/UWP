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
using System.Threading.Tasks;

namespace Inventory.Data.Services
{
    public interface IDataService : IDisposable
    {
        #region Customers
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> GetCustomerAsync(Guid customerGuid);
        Task<IList<Customer>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request);
        Task<IList<Customer>> GetCustomerKeysAsync(int skip, int take, DataRequest<Customer> request);
        Task<int> GetCustomersCountAsync(DataRequest<Customer> request);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomersAsync(params Customer[] customers);
        #endregion

        #region Communications
        Task<Communication> GetCommunicationAsync(Guid communicationGuid);
        Task<Communication> GetCommunicationAsync(int id);
        Task<IList<Communication>> GetCommunicationsAsync(int customerId);
        Task<IList<Communication>> GetCommunicationsAsync(Guid customerGuid);
        Task<int> UpdateCommunicationAsync(Communication communication);
        Task<int> DeleteCommunicationsAsync(params Communication[] communications);

        Task<Communication> GetPrimaryCommunicationAsync(int customerId);
        Task<Communication> FindCommuncationAsync(int customerId, string inputPhone);
        Task<IList<Customer>> FindCustomersByPhoneNumberAsync(string phoneNumber);
        #endregion

        #region Address
        Task<Address> GetAddressAsync(int id);
        Task<Address> GetAddressAsync(Guid addressGuid);
        Task<IList<Address>> GetAddressesAsync(int customerId);
        Task<IList<Address>> GetAddressesAsync(Guid customerGuid);
        Task<int> GetAddressesCountAsync(int customerId);
        Task<int> UpdateAddressAsync(Address address);
        Task<int> DeleteAddressAsync(Address address);
        Task<Address> FindExistAddressAsync(Address address);
        Task<Address> GetPrimaryOrFirstAddressAsync(int customerId);
        #endregion

        #region Dishes
        Task<Dish> GetDishAsync(int id);
        Task<Dish> GetDishAsync(Guid dishGuid);
        Task<IList<Dish>> GetDishesAsync(int skip, int take, DataRequest<Dish> request);
        Task<IList<Dish>> GetDishKeysAsync(int skip, int take, DataRequest<Dish> request);
        Task<int> GetDishesCountAsync(DataRequest<Dish> request);
        Task<int> UpdateDishAsync(Dish dish);
        Task<int> DeleteDishesAsync(params Dish[] dishes);
        #endregion

        #region DishGarnish
        Task<DishGarnish> GetDishGarnishAsync(int id);
        Task<DishGarnish> GetDishGarnishAsync(Guid dishGarnishGuid);
        Task<IList<DishGarnish>> GetDishGarnishesAsync(Guid dishGuid);
        Task<IList<DishGarnish>> GetDishGarnishesAsync();
        Task<int> UpdateDishGarnishAsync(DishGarnish dishGarnish);
        Task<int> DeleteDishGarnishesAsync(params DishGarnish[] dishGarnishes);
        #endregion

        #region Marks
        Task<Mark> GetMarkAsync(Guid markGuid);
        Task<IList<Mark>> GetMarksAsync();
        Task<int> UpdateMarkAsync(Mark mark);
        Task<int> DeleteMarkAsync(Mark mark);
        #endregion

        #region Modifier
        Task<Modifier> GetModifierAsync(Guid modifierGuid);
        Task<IList<Modifier>> GetModifiersAsync();
        Task<int> UpdateModifierAsync(Modifier modifier);
        Task<int> DeleteModifiersAsync(params Modifier[] modifiers);

        Task<IList<Modifier>> GetRelatedDishModifiersAsync(Guid dishGuid);
        #endregion

        #region DishMarks
        Task<DishMark> GetDishMarkAsync(Guid dishMarkGuid);
        Task<IList<DishMark>> GetDishMarksAsync(Guid dishGuid);
        Task<int> UpdateDishMarkAsync(DishMark dishMark);
        Task<int> DeleteDishMarkAsync(DishMark dishMark);
        #endregion

        #region DishIngredient
        Task<DishIngredient> GetDishIngredientAsync(int id);
        Task<DishIngredient> GetDishIngredientAsync(Guid dishIngredientGuid);
        Task<IList<DishIngredient>> GetDishIngredientsAsync(Guid dishGuid);
        Task<IList<DishIngredient>> GetDishIngredientsAsync();
        Task<int> UpdateDishIngredientAsync(DishIngredient dishIngredient);
        Task<int> DeleteDishIngredientsAsync(params DishIngredient[] dishIngredients);
        #endregion

        #region DishRecommend

        Task<IList<DishRecommend>> GetDishRecommendsAsync(Guid dishGuid);

        #endregion

        #region Orders
        Task<Order> GetOrderAsync(Guid orderGuid);
        Task<Order> GetOrderAsync(int Id);
        Task<IList<Order>> GetOrdersAsync(DataRequest<Order> request);
        Task<IList<Order>> GetOrdersAsync(int skip, int take, DataRequest<Order> request);
        Task<IList<Order>> GetOrderKeysAsync(int skip, int take, DataRequest<Order> request);
        Task<int> GetOrdersCountAsync(DataRequest<Order> request);
        Task<int> UpdateOrderAsync(Order order);
        Task<int> DeleteOrdersAsync(params Order[] orders);

        Task<int> BindDishesToOrderAsync(Guid orderGuid, int orderId);

        Task<IList<OrderStatusSequence>> GetOrderStatusSequenceAsync();
        #endregion

        #region OrderDishes
        Task<IList<OrderDish>> GetOrderDishesAsync(Guid orderGuid);
        Task<OrderDish> GetOrderDishAsync(int id);
        Task<IList<OrderDish>> GetOrderDishesAsync(int skip, int take, DataRequest<OrderDish> request);
        Task<IList<OrderDish>> GetOrderDishKeysAsync(int skip, int take, DataRequest<OrderDish> request);
        Task<int> GetOrderDishesCountAsync(DataRequest<OrderDish> request);
        Task<int> UpdateOrderDishAsync(OrderDish orderDish);
        Task<int> DeleteOrderDishesAsync(params OrderDish[] orderDishes);
        #endregion

        #region OrderDishGarnish
        Task<OrderDishGarnish> GetOrderDishGarnishAsync(int id);
        Task<IList<OrderDishGarnish>> GetOrderDishGarnishesAsync(int orderDishId);
        Task<int> UpdateOrderDishGarnishAsync(OrderDishGarnish garnish);
        Task<int> DeleteOrderDishGarnishesAsync(params OrderDishGarnish[] garnishes);
        #endregion

        #region OrderDishIngredient
        Task<OrderDishIngredient> GetOrderDishIngredientAsync(int id);
        Task<IList<OrderDishIngredient>> GetOrderDishIngredientsAsync(int orderDishId);
        Task<int> UpdateOrderDishIngredientAsync(OrderDishIngredient ingredient);
        Task<int> DeleteOrderDishIngredientsAsync(params OrderDishIngredient[] ingredients);
        #endregion

        #region OrderStatusHistories
        Task<OrderStatusHistory> GetOrderStatusHistoryAsync(int orderID, int orderLine);
        Task<IList<OrderStatusHistory>> GetOrderStatusHistorysAsync(int skip, int take, DataRequest<OrderStatusHistory> request);
        Task<IList<OrderStatusHistory>> GetOrderStatusHistorysKeysAsync(int skip, int take, DataRequest<OrderStatusHistory> request);
        Task<int> GetOrderStatusHistoryCountAsync(DataRequest<OrderStatusHistory> request);
        Task<int> UpdateOrderStatusHistoryAsync(OrderStatusHistory OrderStatusHistory);
        Task<int> DeleteOrderStatusHistoryAsync(params OrderStatusHistory[] OrderStatusHistory);
        #endregion

        #region Variables
        Task<Variable> GetVariableAsync(int id);
        Task<Variable> GetVariableAsync(string name);
        Task<IList<Variable>> GetVariablesAsync();
        Task<int> UpdateVariableAsync(Variable variable);
        #endregion

        #region Streets
        Task<Street> GetStreetAsync(int? id);
        Task<IList<Street>> GetStreetsAsync();
        Task<IList<Street>> GetStreetsAsync(DataRequest<Street> request);
        #endregion

        #region Dictionaties
        Task<MenuFolder> GetMenuFolderAsync(int? id);
        Task<MenuFolder> GetMenuFolderAsync(Guid rowGuid);
        Task<IList<MenuFolder>> GetMenuFoldersAsync();

        Task<OrderStatus> GetOrderStatusAsync(int? id);
        Task<IList<OrderStatus>> GetOrderStatusesAsync();

        Task<Source> GetOrderSourceAsync(int? id);
        Task<IList<Source>> GetOrderSourcesAsync();

        Task<OrderType> GetOrderTypeAsync(int? id);
        Task<IList<OrderType>> GetOrderTypesAsync();

        Task<DeliveryType> GetDeliveryTypeAsync(int? id);
        Task<IList<DeliveryType>> GetDeliveryTypesAsync();

        Task<City> GetCityAsync(int? id);
        Task<IList<City>> GetCitiesAsync();

        Task<PaymentType> GetPaymentTypeAsync(int? id);
        Task<IList<PaymentType>> GetPaymentTypesAsync();

        Task<Restaurant> GetRestaurantAsync(int? id);
        Task<IList<Restaurant>> GetRestaurantsAsync();

        Task<TaxType> GetTaxTypeAsync(int? id);
        Task<IList<TaxType>> GetTaxTypesAsync();
        #endregion
    }
}
