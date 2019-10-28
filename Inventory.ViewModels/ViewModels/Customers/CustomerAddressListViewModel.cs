using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class CustomerAdddressListArgs
    {
        public static CustomerAdddressListArgs CreateEmpty() => new CustomerAdddressListArgs { IsEmpty = true };

        public bool IsEmpty { get; set; }

        public int CustomerId { get; set; }

        public Guid CustomerGuid { get; set; }

        public string Query { get; set; }
    }

    public class CustomerAddressListViewModel : GenericListViewModel<AddressModel>
    {
        public CustomerAdddressListArgs ViewModelArgs { get; private set; }

        private ICustomerService CustomerService { get; }

        public CustomerAddressListViewModel(ICustomerService customerService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;
        }

        public async Task LoadAsync(CustomerAdddressListArgs args, bool silent = false)
        {
            ViewModelArgs = args ?? CustomerAdddressListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            if (silent)
            {
                await RefreshAsync();
            }
            else
            {
                StartStatusMessage("Загружаются адреса клиента...");
                if (await RefreshAsync())
                {
                    EndStatusMessage("Адреса клиента загружены");
                }
            }
        }

        public void Unload()
        {
            if (ViewModelArgs == null)
            {
                return;
            }

            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {

        }

        public void Unsubscribe()
        {

        }

        public CustomerAdddressListArgs CreateArgs()
        {
            return new CustomerAdddressListArgs
            {
                Query = Query,
                CustomerId = ViewModelArgs.CustomerId,
                CustomerGuid = ViewModelArgs.CustomerGuid
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<AddressModel>();
                StatusError($"Ошибка загрузки адресов клиента: {ex.Message}");
                LogException("Customer Address", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        protected override void OnNew()
        {
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Адреса клиента загружаются...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Адреса клиентов загружены");
            }
        }

        protected override void OnDeleteSelection()
        {
        }

        private async Task<IList<AddressModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                return await CustomerService.GetAddressesAsync(ViewModelArgs.CustomerId);
            }
            return new List<AddressModel>();
        }
    }
}
