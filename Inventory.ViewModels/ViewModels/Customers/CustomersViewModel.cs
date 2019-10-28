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
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(ICustomerService customerService, IOrderService orderService, IVariableService variableService,
            IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;

            CustomerList = new CustomerListViewModel(CustomerService, commonServices);
            CustomerDetails = new CustomerDetailsViewModel(CustomerService, variableService, filePickerService, commonServices);
            CommunicationList = new CommunicationListViewModel(CustomerService, commonServices);
            AddressList = new CustomerAddressListViewModel(CustomerService, commonServices);
            CustomerOrders = new OrderListViewModel(orderService, commonServices);
        }

        public ICustomerService CustomerService { get; }

        public CustomerListViewModel CustomerList { get; }
        public CustomerDetailsViewModel CustomerDetails { get; }
        public CommunicationListViewModel CommunicationList { get; }
        public CustomerAddressListViewModel AddressList { get; }
        public OrderListViewModel CustomerOrders { get; set; }

        public async Task LoadAsync(CustomerListArgs args)
        {
            await CustomerList.LoadAsync(args);
        }
        public void Unload()
        {
            CustomerDetails.CancelEdit();
            CustomerList.Unload();
            CommunicationList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CustomerListViewModel>(this, OnMessage);
            CustomerList.Subscribe();
            CustomerDetails.Subscribe();
            CustomerOrders.Subscribe();
            CommunicationList.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            CustomerList.Unsubscribe();
            CustomerDetails.Unsubscribe();
            CustomerOrders.Unsubscribe();
            CommunicationList.Unsubscribe();
        }

        private async void OnMessage(CustomerListViewModel viewModel, string message, object args)
        {
            if (viewModel == CustomerList && message == Messages.ItemSelected)
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (CustomerDetails.IsEditMode)
            {
                StatusReady();
                CustomerDetails.CancelEdit();
            }
            CustomerOrders.IsMultipleSelection = false;
            var selected = CustomerList.SelectedItem;
            if (!CustomerList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                    await PopulateCommunications(selected);
                    await PopulateAddresses(selected);
                    await PopulateOrders(selected);
                }
            }
            CustomerDetails.Item = selected;
        }

        private async Task PopulateDetails(CustomerModel selected)
        {
            try
            {
                var model = await CustomerService.GetCustomerAsync(selected.Id);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("Customers", "Load Details", ex);
            }
        }

        private async Task PopulateOrders(CustomerModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    await CustomerOrders.LoadAsync(new OrderListArgs { CustomerId = selectedItem.Id }, silent: true);
                }
            }
            catch (Exception ex)
            {
                LogException("Customers", "Load Orders", ex);
            }
        }

        private async Task PopulateCommunications(CustomerModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    await CommunicationList.LoadAsync(new CommunicationListArgs { CustomerId = selectedItem.Id }, silent: true);
                }
            }
            catch (Exception ex)
            {
                LogException("Customers", "Load communications", ex);
            }
        }

        private async Task PopulateAddresses(CustomerModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    await AddressList.LoadAsync(new CustomerAdddressListArgs { CustomerId = selectedItem.Id }, silent: true);
                }
            }
            catch (Exception ex)
            {
                LogException("Customer", "Load addresses", ex);
            }
        }
    }
}
