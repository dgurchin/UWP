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

using Inventory.Data;
using Inventory.Models;
using Inventory.Models.Enums;

namespace Inventory.Services
{
    public interface ICustomerService
    {
        #region Customer
        Task<CustomerModel> GetCustomerAsync(int id);
        Task<IList<CustomerModel>> GetCustomersAsync(DataRequest<Customer> request);
        Task<IList<CustomerModel>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request);
        Task<int> GetCustomersCountAsync(DataRequest<Customer> request);

        Task<int> UpdateCustomerAsync(CustomerModel model);

        Task<int> DeleteCustomerAsync(CustomerModel model);
        Task<int> DeleteCustomerRangeAsync(int index, int length, DataRequest<Customer> request);

        /// <summary>
        /// Создает нового клиента
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<CustomerModel> AddNewCustomerAsync(CustomerModel customer);

        Task<CustomerModel> CreateCustomerModelAsync(Customer source, bool includeAllFields);
        #endregion

        #region Communication

        /// <summary>
        /// Добавляет новый телефон с типом <see cref="CommunicationTypeEnum.Mobile"/> если не нашло существующий <br></br>
        /// и помечает его основным <see cref="Communication.IsPrimary"/>
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task AddPhoneIfNotExistAsync(int customerId, string phoneNumber);

        Task<CommunicationModel> GetCommunicationAsync(int id);

        Task<IList<CommunicationModel>> GetCommunicationsAsync(int customerId);

        Task<int> UpdateCommunicationAsync(CommunicationModel communicationModel);

        Task<int> DeleteCommunicationAsync(CommunicationModel communication);

        Task<CommunicationModel> GetPrimaryCommunicationAsync(int customerId);

        /// <summary>
        /// Выполняет поиск первого телефона клиента с сортировкой по <see cref="Communication.IsPrimary"/> полю
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="inputPhone"></param>
        /// <returns></returns>
        Task<CommunicationModel> FindCustomerCommuncationAsync(int customerId, string inputPhone);

        /// <summary>
        /// Поиск контактов по любой максе телефона<br/>
        /// +3ХХХХХХХХХХ    +38 000-000-00-00 [mobile]<br/>
        /// ХХХХХХХХХХХ      38 000-000-00-00 [mobile]<br/>
        /// ХХХХХХХХХХ          000-000-00-00 [mobile]<br/>
        /// ХХХХХХХ                 000-00-00 [home]
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<IList<CustomerModel>> FindCustomersByPhoneNumberAsync(string phoneNumber);

        #endregion

        #region Address
        Task<IList<AddressModel>> GetAddressesAsync(int customerId);
        Task<int> GetAddressesCountAsync(int customerId);
        Task<int> UpdateAddressAsync(AddressModel addressModel);
        Task<int> DeleteAddressAsync(AddressModel addressModel);

        /// <summary>
        /// Проверяет существует ли адрес с такими же параметрами как <paramref name="addressModel"/> в <see cref="Customer"/> <br/>
        /// Фильтрация выполняется по полю <see cref="AddressModel.CustomerId"/>
        /// </summary>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        Task<AddressModel> FindExistAddressAsync(AddressModel addressModel);

        /// <summary>
        /// Выполняет проверку существующего адреса <see cref="FindExistAddressAsync(AddressModel)"/>. <br/>
        /// Если не существует - то тогда ставит флаг <see cref="AddressModel.IsPrimary"/> в true и добавляет к <see cref="Customer"/> по <paramref name="customerId"/>.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        Task AddAddressIfNotExistAsync(int customerId, AddressModel addressModel);

        /// <summary>
        /// Получить основной или первый адрес клиента
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<AddressModel> GetPrimaryOrFirstAddressAsync(int customerId);
        #endregion
    }
}
