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

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(ICustomerService customerService, IOrderService orderService, IDishService productService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;
            OrderService = orderService;
            ProductService = productService;
        }

        public ICustomerService CustomerService { get; }
        public IOrderService OrderService { get; }
        public IDishService ProductService { get; }

        private IList<CustomerModel> _customers = null;
        public IList<CustomerModel> Customers
        {
            get => _customers;
            set => Set(ref _customers, value);
        }

        private IList<DishModel> _products = null;
        public IList<DishModel> Products
        {
            get => _products;
            set => Set(ref _products, value);
        }

        private IList<OrderModel> _orders = null;
        public IList<OrderModel> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }

        public async Task LoadAsync()
        {
            StartStatusMessage("Loading dashboard...");
            await LoadCustomersAsync();
            await LoadOrdersAsync();
            await LoadProductsAsync();
            EndStatusMessage("Dashboard loaded");
        }
        public void Unload()
        {
            Customers = null;
            Products = null;
            Orders = null;
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                var request = new DataRequest<Customer>
                {
                    OrderByDesc = r => r.CreatedOn
                };
                Customers = await CustomerService.GetCustomersAsync(0, 5, request);
            }
            catch (Exception ex)
            {
                LogException("Dashboard", "Load Customers", ex);
            }
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                var request = new DataRequest<Order>
                {
                    OrderByDesc = r => r.CreatedOn
                };
                Orders = await OrderService.GetOrdersAsync(0, 5, request);
            }
            catch (Exception ex)
            {
                LogException("Dashboard", "Load Orders", ex);
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var request = new DataRequest<Dish>
                {
                    OrderByDesc = r => r.CreatedOn
                };
                Products = await ProductService.GetDishesAsync(0, 5, request);
            }
            catch (Exception ex)
            {
                LogException("Dashboard", "Load Products", ex);
            }
        }

        public void ItemSelected(string item)
        {
            switch (item)
            {
                case "Customers":
                    NavigationService.Navigate<CustomersViewModel>(new CustomerListArgs { OrderByDesc = r => r.CreatedOn });
                    break;
                case "Orders":
                    NavigationService.Navigate<OrdersViewModel>(new OrderListArgs { OrderByDesc = r => r.CreatedOn });
                    break;
                case "Products":
                    NavigationService.Navigate<DishesViewModel>(new DishListArgs { OrderByDesc = r => r.Price });
                    break;
                default:
                    break;
            }
        }
    }
}
