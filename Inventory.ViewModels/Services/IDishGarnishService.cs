using Inventory.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IDishGarnishService
    {
        Task<DishGarnishModel> GetDishGarnishAsync(int id);
        Task<DishGarnishModel> GetDishGarnishAsync(Guid dishGarnishGuid);
        Task<IList<DishGarnishModel>> GetDishGarnishesAsync(Guid dishGuid);
    }
}
