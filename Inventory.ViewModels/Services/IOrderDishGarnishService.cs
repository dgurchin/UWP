using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IOrderDishGarnishService
    {
        Task<OrderDishGarnishModel> GetGarnishAsync(int id);
        Task<IList<OrderDishGarnishModel>> GetOrderDishGarnishesAsync(int orderDishId);
        Task<int> UpdateGarnishAsync(OrderDishGarnishModel model);
        Task<int> DeleteGarnishAsync(OrderDishGarnishModel garnishModel);

        Task CheckAndDeleteGarnishesAsync(IList<OrderGarnishModel> garnishes, int existOrderGarnishId);
        Task CheckAndAppendGarnishAsync(int orderDishId, Guid dishGuid, OrderGarnishModel selectedModel);
        Task<IList<OrderGarnishModel>> GetOrderViewGarnishesAsync(IList<DishGarnishModel> dishGarnishes, int orderDishId);
    }
}
