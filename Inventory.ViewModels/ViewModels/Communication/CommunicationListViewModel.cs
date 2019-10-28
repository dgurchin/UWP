using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class CommunicationListArgs
    {
        public static CommunicationListArgs CreateEmpty() => new CommunicationListArgs { IsEmpty = true };

        public bool IsEmpty { get; set; }

        public int CustomerId { get; set; }

        public Guid CustomerGuid { get; set; }

        public string Query { get; set; }
    }

    public class CommunicationListViewModel : GenericListViewModel<CommunicationModel>
    {
        public CommunicationListArgs ViewModelArgs { get; private set; }

        private ICustomerService CustomerService { get; }

        public CommunicationListViewModel(ICustomerService customerService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;
        }

        public async Task LoadAsync(CommunicationListArgs args, bool silent = false)
        {
            ViewModelArgs = args ?? CommunicationListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            if (silent)
            {
                await RefreshAsync();
            }
            else
            {
                StartStatusMessage("Загружаются средства связи...");
                if (await RefreshAsync())
                {
                    EndStatusMessage("Средства связи загружены");
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

        public CommunicationListArgs CreateArgs()
        {
            return new CommunicationListArgs
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
                Items = new List<CommunicationModel>();
                StatusError($"Ошибка загрузки средств связи: {ex.Message}");
                LogException("Communication", "Refresh", ex);
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

        protected override async void OnRefresh()
        {
            StartStatusMessage("Средства связи загружаются...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Средства связи загружены");
            }
        }

        protected override void OnDeleteSelection()
        {
        }

        protected override void OnNew()
        {
        }

        private async Task<IList<CommunicationModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                return await CustomerService.GetCommunicationsAsync(ViewModelArgs.CustomerId);
            }
            return new List<CommunicationModel>();
        }
    }
}
