using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.Data;
using Inventory.Data.Services;
using Inventory.BLL.DTO;

namespace Delivery.WebApi.Services
{
    public class OrderService
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public OrderService(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>());
            //_mapper = config.CreateMapper();
       }
        #region Order CRUD methods
        /// <summary>
        /// Получить заказ
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderAsync(Guid orderGuid)
        {
            Order ord= await _dataService.GetOrderAsync(orderGuid);
            var model = _mapper.Map<Order, OrderDTO>(ord);
            return ord;
            //return await _dataService.GetOrderAsync(orderGuid);
        }

        /// <summary>
        /// Получить заказ по номеру
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderAsync(int orderId)
        {
            return await _dataService.GetOrderAsync(orderId);
        }

        /// <summary>
        /// Получить заказы за сегодня
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Order>> GetOrdersTodayAsync()
        {
            DataRequest<Order> dataRequest = new DataRequest<Order>();
            dataRequest.Where = (r) => r.CreatedOn.Day==DateTime.Now.Day;
            //dataRequest.Where = (r) => r.RowGuid == customerGuid;
            //int take = await _dataService.h();
            return await _dataService.GetOrdersAsync(dataRequest);
        }

        /// <summary>
        /// Получить заказы за сегодня по подразделению
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Order>> GetRestauantOrdersTodayAsync(int restaurantID)
        {
            DataRequest<Order> dataRequest = new DataRequest<Order>();
            dataRequest.Where = (r) => (r.CreatedOn.Day == DateTime.Now.Day)& (r.RestaurantId == restaurantID);
            //dataRequest.Where = (r) => r.RowGuid == customerGuid;
            //int take = await _dataService.h();
            return await _dataService.GetOrdersAsync(dataRequest);
        }

        /// <summary>
        /// Получить заказы за сегодня по подразделению и статусу
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Order>> GetRestauantOrdersTodayAsync(int restaurantID, int statusID)
        {
            DataRequest<Order> dataRequest = new DataRequest<Order>();
            dataRequest.Where = (r) => (r.CreatedOn.Day == DateTime.Now.Day) & (r.RestaurantId == restaurantID)& (r.StatusId == statusID);
            //dataRequest.Where = (r) => r.RowGuid == customerGuid;
            //int take = await _dataService.h();
            return await _dataService.GetOrdersAsync(dataRequest);
        }

        /// <summary>
        /// Получить {take} заказов
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Order>> GetOrdersAsync(int skip = 0, int take = 50)
        {
            DataRequest<Order> dataRequest = new DataRequest<Order>();
            dataRequest.OrderByDesc = (r) => r.CreatedOn;
            //dataRequest.Where = (r) => r.RowGuid == customerGuid;

            return await _dataService.GetOrdersAsync(skip, take, dataRequest);
        }

        /// <summary>
        /// Получить {take} заказов клиента
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Order>> GetCustomerOrdersAsync(Guid customerGuid,int skip = 0, int take = 50)
        {
            DataRequest<Order> dataRequest = new DataRequest<Order>();
            dataRequest.OrderByDesc = (r) => r.CreatedOn;
            if (!((customerGuid == null)||(customerGuid==Guid.Empty)))
            {
                dataRequest.Where = (r) => r.Customer.RowGuid == customerGuid;
            }
            return await _dataService.GetOrdersAsync(skip, take, dataRequest);
        }
        /// <summary>
        /// Получить количество заказов
        /// </summary>
        /// <returns></returns>
        //public async Task<IList<Order>> GetOrdersAsync(int skip, int take, Guid customerGuid)
        public async Task<int> GetOrdersCountAsync()
        {
            DataRequest<Order> dataRequest = new DataRequest<Order>();
            dataRequest.OrderByDesc = (r) => r.CreatedOn;
            return await _dataService.GetOrdersCountAsync( dataRequest);
        }

        /// <summary>
        ///  POST(Order) - добавить заказы
        /// </summary>
        /// <returns></returns>
        public async Task<int> PostOrderAsync(IList<Order> models)
        {
            int i = 0;
            if (models == null || models.Count == 0)
            {
                return -1;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateOrderAsync(model);
                i = i + 1;
            }
            return i;
        }

        /// <summary>
        /// PUT(Order) - обновить заказы - проверка по Order.RowGuid
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task PutOrderAsync(IList<Order> models)
        {
            if (models == null || models.Count == 0)
            {
                return;
            }

            foreach (var model in models)
            {
                await _dataService.UpdateOrderAsync(model);
            }
        }

        /// <summary>
        /// DELETE(rowGuid) - удалить по Order.RowGuid
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteOrdersAsync(params Order[] models)
        {
            int rez = await _dataService.DeleteOrdersAsync(models);
            return rez;
        }

        #endregion Order
        #region OrderDish
        ///// <summary>
        ///// Получить блюда заказа
        ///// </summary>
 
        public async Task<IList<OrderDish>> GetOrderDishesAsync(Guid orderGuid)
        {
          return  await _dataService.GetOrderDishesAsync(orderGuid);
        }
        #endregion OrderDish
        #region OrderDishGarnish
        ///// <summary>
        ///// Получить список гарнир блюда заказа
        ///// </summary>

        public async Task<IList<OrderDishGarnish>> GetOrderDishGarnishesAsync(int orderDishID)
        {
            return await _dataService.GetOrderDishGarnishesAsync(orderDishID);
        }
        #endregion OrderDishGarnish
        #region OrderDishIngredient
        ///// <summary>
        ///// Получить список гарнир блюда заказа
        ///// </summary>

        public async Task<IList<OrderDishIngredient>> GetOrderDishIngredientsAsync(int orderDishID)
        {
            return await _dataService.GetOrderDishIngredientsAsync(orderDishID);
        }
        #endregion OrderDishIngredient
    }
}
