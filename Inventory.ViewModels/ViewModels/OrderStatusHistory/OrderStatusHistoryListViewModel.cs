using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region OrderStatusHistoryListArgs
    public class OrderStatusHistoryListArgs
    {
        static public OrderStatusHistoryListArgs CreateEmpty() => new OrderStatusHistoryListArgs { IsEmpty = true };

        public OrderStatusHistoryListArgs()
        {
            OrderBy = r => r.OrderLine;
        }

        public long OrderId { get; set; }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<OrderStatusHistory, object>> OrderBy { get; set; }
        public Expression<Func<OrderStatusHistory, object>> OrderByDesc { get; set; }
    }
    #endregion
    public class OrderStatusHistoryListViewModel : GenericListViewModel<OrderStatusHistoryModel>
    {
        private OrderStatusHistoryListArgs _viewModelArgs;

        public OrderStatusHistoryListViewModel(IOrderStatusHistoryService orderStatusHistoryService, ICommonServices commonServices) : base(commonServices)
        {
            OrderStatusHistoryService = orderStatusHistoryService;
        }
        public IOrderStatusHistoryService OrderStatusHistoryService { get; }

        public OrderStatusHistoryListArgs ViewModelArgs
        {
            get
            {
                if (_viewModelArgs == null)
                    _viewModelArgs = OrderStatusHistoryListArgs.CreateEmpty();
                return _viewModelArgs;
            }

            private set => _viewModelArgs = value;
        }

        public async Task LoadAsync(OrderStatusHistoryListArgs args, bool silent = false)
        {
            ViewModelArgs = args ?? OrderStatusHistoryListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            if (silent)
            {
                await RefreshAsync();
            }
            else
            {
                StartStatusMessage("Загружается история выполнения заказа...");
                if (await RefreshAsync())
                {
                    EndStatusMessage("История выполнения заказа загружена");
                }
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }
        public void Subscribe()
        {
            MessageService.Subscribe<OrderStatusHistoryListViewModel>(this, OnMessage);
            MessageService.Subscribe<OrderStatusHistoryDetailsViewModel>(this, OnMessage);
            //Подписываем OrderDetailsViewModel(доб. в историю при изм.статуса) на обр.сообщ. в OnMessage
            MessageService.Subscribe<OrderDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderStatusHistoryListArgs CreateArgs()
        {
            return new OrderStatusHistoryListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc,
                OrderId = ViewModelArgs.OrderId
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
                Items = new List<OrderStatusHistoryModel>();
                StatusError($"Ошибка загрузки позиций заказа: {ex.Message}");
                LogException("OrderStatusHistory", "Refresh", ex);
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

        private async Task<IList<OrderStatusHistoryModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<OrderStatusHistory> request = BuildDataRequest();
                return await OrderStatusHistoryService.GetOrderStatusHistorysAsync(request);
            }
            return new List<OrderStatusHistoryModel>();
        }
        private DataRequest<OrderStatusHistory> BuildDataRequest()
        {
            var request = new DataRequest<OrderStatusHistory>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
            if (ViewModelArgs.OrderId > 0)
            {
                request.Where = (r) => r.OrderId == ViewModelArgs.OrderId;
            }
            return request;
        }

        protected override void OnDeleteSelection()
        {
        }

        protected override void OnNew()
        {
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading order status history...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Order status history loaded");
            }
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case Messages.NewItemSaved:
                case Messages.ItemDeleted:
                case Messages.ItemsDeleted:
                case Messages.ItemRangesDeleted:
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }

        private async void OnMessage(OrderDetailsViewModel sender, string message, object args)
        {
            switch (message)
            {
                case Messages.NewItemSaved:
                case Messages.ItemDeleted:
                case Messages.ItemChanged:
                case Messages.ItemsDeleted:
                case Messages.ItemRangesDeleted:
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
