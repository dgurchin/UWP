using System.Collections.Generic;
using System.Threading.Tasks;

using Integra.LoadingMonitor.Models;

namespace Inventory.Services
{
    public interface IOrderLoadingMonitor
    {
        /// <summary>
        /// Получить список ресторанов
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
    }
}
