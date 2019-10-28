using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Models;
using Inventory.ViewModels;
using Windows.UI.Core;

namespace Inventory.Services
{
    public class PhoneCallReceiveViewModel : ViewModelBase
    {
        //public RelayCommand ToogleFilterBarCommand { get; }

        private CoreDispatcher _coreDispatcher;
        private ICommonServices _commonServices;
        private ISettingsService _settingsService;
        public CoreDispatcher CoreDispatcher
        {
            get { return _coreDispatcher; }
            set { _coreDispatcher = value; LoadPhoneCall(); }
        }
        public PhoneCallArgs ViewModelArgs { get; private set; }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { Set(ref _phoneNumber, value); PhoneCall.PhoneNumber = PhoneNumber; }
        }
        private CustomerModel _customerCurrent;
        public CustomerModel CustomerCurrent
        {
            get { return _customerCurrent; }
            set { Set(ref _customerCurrent, value); }
        }
        public MapViewModel Map { get; }
        private Thread _phoneThread;

        private bool _IsCtrlKeyDown;
        public bool IsCtrlKeyDown
        {
            get => _IsCtrlKeyDown;
            set => Set(ref _IsCtrlKeyDown, value);
        }

        // TODO: Вынести этот код в ViewModel без Thread.
        public PhoneCallReceiveViewModel(IFilePickerService filePickerService, ICustomerService customerService, IOrderService orderService,
            IOrderDishService orderDishService, IOrderStatusHistoryService orderStatusHistoryService,
            ILocationService locationService, IVariableService variableService, ICommonServices commonServices, ISettingsService settingsService,
            IDishService dishService, IDishGarnishService dishGarnishService, IOrderDishGarnishService orderDishGarnishService,
            IDishIngredientService dishIngredientService, IOrderDishIngredientService orderDishIngredientService,
            IOrderDishModifierService orderDishModifierService,
            IDishRecommendService dishRecommendService, IOrderLoadingMonitor orderLoadingMonitor)
            : base(commonServices)
        {
            _settingsService = settingsService;
            _commonServices = commonServices;
            CustomerDetails = new CustomerDetailsViewModel(customerService, variableService, filePickerService, commonServices);
            OrdersView = new OrdersViewModel(orderService, orderDishService, orderStatusHistoryService, locationService, variableService, commonServices,
                dishService, dishGarnishService, orderDishGarnishService, dishIngredientService,
                orderDishIngredientService, orderDishModifierService, dishRecommendService, orderLoadingMonitor, customerService);
            CustomerList = new CustomerListViewModel(customerService, commonServices);
            OrdersView.Subscribe();
            Subscribe();
            // TODO: не должно быть тут этого
            _phoneThread = Thread.CurrentThread;
            CustomerSelect();
        }


        public IOrderService OrderService { get; }

        #region ViewModels
        public CustomerDetailsViewModel CustomerDetails { get; set; }
        public CustomerListViewModel CustomerList { get; set; }
        public OrdersViewModel OrdersView { get; set; }
        public PhoneCallViewModel PhoneCall { get; set; }
        #endregion

        private void LoadPhoneCall()
        {
            PhoneCall = new PhoneCallViewModel(_commonServices, _settingsService);
            PhoneCall.CoreDispatcher = CoreDispatcher;
            PhoneCall.PhoneNumber = PhoneNumber;
            CustomerDetails.PhoneNumberIsRinging = PhoneNumber;
            PhoneCall.NotIncommingCall = false;
            PhoneCall.Load();
        }

        public async Task LoadAsync(PhoneCallArgs args)
        {
            ViewModelArgs = args ?? PhoneCallArgs.CreateEmpty();
            PhoneNumber = args.PhoneNumber;
            OrderListArgs orderListArgs = new OrderListArgs();
            try
            {
                CustomerList.Items = await CustomerList.CustomerService.FindCustomersByPhoneNumberAsync(PhoneNumber);
                if (CustomerList.Items.Count > 0)
                {
                    CustomerCurrent = CustomerList.Items[0];
                    CustomerSelect();
                }
            }
            catch (Exception ex)
            {
                CustomerList.Items = new List<CustomerModel>();
                StatusError($"Error loading Customers: {ex.Message}");
                LogException("Customers", "Refresh", ex);
            }
            LoadPhoneCall();
        }

        public void Unload()
        {
            OrdersView.Unload();
            PhoneCall.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CustomerDetailsViewModel, CustomerModel>(this, OnDetailsMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        private async void CustomerSelect()
        {
            // TODO:
            if (CustomerCurrent != null)
            {
                OrderListArgs orderListArgs = new OrderListArgs();
                orderListArgs.CustomerId = CustomerCurrent.Id;
                await ContextService.RunAsync(async () => { await OrdersView.LoadAsync(orderListArgs); });
                OrdersView.OrderList.PhoneNumber = PhoneNumber;
                OrdersView.OrderList.OrderDetailsArgs_phone = new OrderDetailsArgs { CustomerId = CustomerCurrent.Id, PhoneNumber = PhoneNumber };

                if (OrdersView.OrderList.Items != null) OrdersView.OrderList.ItemsCount = OrdersView.OrderList.Items.Count;
                if ((!OrdersView.OrderList.IsMultipleSelection) & (OrdersView.OrderList.Items != null))
                {
                    if (OrdersView.OrderList.Items.Count > 0) OrdersView.OrderList.SelectedItem = OrdersView.OrderList.Items?[0];
                }
                OrdersView.OrderList.NotifyPropertyChanged(nameof(Title));

                CustomerDetailsArgs customerDetailsArgs = new CustomerDetailsArgs();
                customerDetailsArgs.CustomerId = CustomerCurrent.Id;
                await CustomerDetails.LoadAsync(customerDetailsArgs);
            }
            else
            {
                CustomerDetailsArgs customerDetailsArgs = new CustomerDetailsArgs();
                customerDetailsArgs.CustomerId = -1;                                     //Признак новой записи
                await CustomerDetails.LoadAsync(customerDetailsArgs);
            }
        }

        //public async Task LLL(OrderListArgs args)
        //{
        //    await OrdersView.OrderList.LoadAsync(args);
        //}

        /// <summary>
        /// Обработка внешних сообщений
        /// </summary>
        private void OnDetailsMessage(CustomerDetailsViewModel sender, string message, CustomerModel changed)
        {
            Thread currentThread = Thread.CurrentThread;
            if (currentThread == _phoneThread)
            {
                switch (message)
                {
                    case Messages.NewItemSaved:
                        if (changed is CustomerModel)
                        {
                            CustomerCurrent = changed;
                            CustomerSelect();
                        }
                        break;
                }
            }
        }
    }
}
