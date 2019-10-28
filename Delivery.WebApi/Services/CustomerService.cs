using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.Data;
using Inventory.Data.Services;
using Inventory.BLL.DTO;

namespace Delivery.WebApi.Services
{
    public class CustomerService
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public CustomerService(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>());
            //_mapper = config.CreateMapper();
        }

            //#region CRUD methods
            #region Customers
            /// <summary>
            /// Получить клиента
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _dataService.GetCustomerAsync(id);
        }
        /// <summary>
        /// Получить клиента
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerAsync(Guid customerGuid)
        {
            return await _dataService.GetCustomerAsync(customerGuid);
        }
        public async Task<CustomerDTO> GetCustomerDTOAsync(Guid customerGuid)
        {
            Customer _customer = await _dataService.GetCustomerAsync(customerGuid);
            CustomerDTO _customerDTO = _mapper.Map<Customer, CustomerDTO>(_customer);
            return _customerDTO;
        }

        /// <summary>
        /// Получить n клиентов
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Customer>> GetCustomersAsync(int skip,int take)
        {
            return await _dataService.GetCustomersAsync(skip, take, new DataRequest<Customer>());
        }
        public async Task<List<CustomerDTO>> GetCustomersDTOAsync()
        {
            int _take = await _dataService.GetCustomersCountAsync(new DataRequest<Customer>());
            IList<Customer> _customers= await GetCustomersAsync(0, _take);
            List<CustomerDTO> _customersDTO;
            _customersDTO = new List<CustomerDTO>();
            foreach (var model in _customers)
            {
                _customersDTO.Add(_mapper.Map<Customer, CustomerDTO>(model));
            }
            return _customersDTO;
        }



        /// <summary>
        /// Записать клиентов
        /// </summary>
        /// <returns></returns>
        public async Task<int> PostCustomerAsync(IList<Customer> models)
        {
            int i = 0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateCustomerAsync(model);
                i = i + 1;
            }
            return i;
        }
        public async Task<int> PostCustomerDTOAsync(IList<CustomerDTO> models)
        {
            int i = 0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                Customer _customer=_mapper.Map<CustomerDTO,Customer>(model);
                Customer _customerOld=await _dataService.GetCustomerAsync(_customer.RowGuid);
                if ((_customerOld?.Id > 0) & (_customer.Id == 0)) _customer.Id = _customerOld.Id;
                await _dataService.UpdateCustomerAsync(_customer);
                i = i + 1;
            }
            return i;
        }



        /// <summary>
        /// Удалить клиентов
        /// </summary>
        /// <returns></returns>
        public async Task DeleteCustomersAsync(params Customer[] customers)
        {
            await _dataService.DeleteCustomersAsync(customers);
        }
        #endregion
        #region Communication
        /// <summary>
        /// Получить Communication
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public async Task<Communication> GetCommunicationAsync(Guid communicationGuid)
        {
            return await _dataService.GetCommunicationAsync(communicationGuid);
        }
        /// <summary>
        /// Получить связи с клиентом
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Communication>> GetCustomerCommunicationsAsync(Guid customerGuid)
        {
            return await _dataService.GetCommunicationsAsync(customerGuid);
        }
        /// <summary>
        /// Записать контактов
        /// </summary>
        /// <returns></returns>
        public async Task<int> PostCommunicationAsync(IList<Communication> models)
        {
            int i = 0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateCommunicationAsync(model);
                i = i + 1;
            }
            return i;
        }
        /// <summary>
        /// Удалить контакты
        /// </summary>
        /// <returns></returns>
        public async Task DeleteCommunicationsAsync(params Communication[] communications)
        {
            await _dataService.DeleteCommunicationsAsync(communications);
        }
        #endregion Communication
        #region Address
        /// <summary>
        /// Получить Address
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public async Task<Address> GetAddressAsync(Guid addressGuid)
        {
            return await _dataService.GetAddressAsync(addressGuid);
        }
        /// <summary>
        /// Получить адреса клиента
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Address>> GetCustomerAddressesAsync(Guid customerGuid)
        {
            return await _dataService.GetAddressesAsync(customerGuid);
        }
        /// <summary>
        /// Записать адрес
        /// </summary>
        /// <returns></returns>
        public async Task<int> PostAddressAsync(IList<Address> models)
        {
            int i = 0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateAddressAsync(model);
                i = i + 1;
            }
            return i;
        }
        /// <summary>
        /// Удалить контакты
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAddressAsync(Address address)
        {
            await _dataService.DeleteAddressAsync(address);
        }

        #endregion Address
    }
}
