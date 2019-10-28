using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Integra.LoadingMonitor.DataProviders;
using Integra.LoadingMonitor.Models;
using Integra.LoadingMonitor.Services;

namespace Inventory.Services
{
    public class OrderLoadingMonitor : IOrderLoadingMonitor
    {
        private IDataProvider _dataProvider;

        private IDataProvider DataProvider
        {
            get
            {
                if (_dataProvider == null)
                    _dataProvider = AppSettings.Current.GetMonitorDataProvider();
                return _dataProvider;
            }
        }

        private ILoadMonitor LoadMonitor { get; }

        public OrderLoadingMonitor(ILoadMonitor monitor)
        {
            LoadMonitor = monitor;
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
        {
            await LoadMonitor.ApplyProvider(DataProvider);

            var models = await LoadMonitor.GetRestaurantsAsync();
            return models;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            OrderFilter orderFilter = new OrderFilter
            {
                DepartmentIds = (await LoadMonitor.GetDepartmentsAsync()).Select(x => x.Id).ToList().AsReadOnly(),
                OrderStateIds = (await LoadMonitor.GetActiveStatesAsync()).Select(x => x.Id).ToList().AsReadOnly(),
                Restaurants = (await LoadMonitor.GetRestaurantsAsync()).ToList().AsReadOnly()
            };
            var orders = await LoadMonitor.GetOrdersAsync(orderFilter);
            return orders;
        }
    }
}
