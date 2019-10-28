using System;
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrderStatusHistoryViewModel : ViewModelBase
    {
        public OrderStatusHistoryViewModel(IOrderStatusHistoryService orderStatusHistoryService, IOrderService orderService, ICommonServices commonServices) : base(commonServices)
        {
            OrderStatusHistoryService = orderStatusHistoryService;
            OrderStatusHistoryList = new OrderStatusHistoryListViewModel(OrderStatusHistoryService, commonServices);
            OrderStatusHistoryDetails = new OrderStatusHistoryDetailsViewModel(OrderStatusHistoryService, commonServices);
        }
        public IOrderStatusHistoryService OrderStatusHistoryService { get; }

        public OrderStatusHistoryListViewModel OrderStatusHistoryList { get; set; }
        public OrderStatusHistoryDetailsViewModel OrderStatusHistoryDetails { get; set; }

        public async Task LoadAsync(OrderStatusHistoryListArgs args)
        {
            await OrderStatusHistoryList.LoadAsync(args);
        }
        public void Unload()
        {
            OrderStatusHistoryDetails.CancelEdit();
            OrderStatusHistoryList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderStatusHistoryListViewModel>(this, OnMessage);
            OrderStatusHistoryList.Subscribe();
            OrderStatusHistoryDetails.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderStatusHistoryList.Unsubscribe();
            OrderStatusHistoryDetails.Unsubscribe();
        }

        private async void OnMessage(OrderStatusHistoryListViewModel viewModel, string message, object args)
        {
            if (viewModel == OrderStatusHistoryList && message == Messages.ItemSelected)
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (OrderStatusHistoryDetails.IsEditMode)
            {
                StatusReady();
                OrderStatusHistoryDetails.CancelEdit();
            }
            var selected = OrderStatusHistoryList.SelectedItem;
            if (!OrderStatusHistoryList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            OrderStatusHistoryDetails.Item = selected;
        }

        private async Task PopulateDetails(OrderStatusHistoryModel selected)
        {
            try
            {
                var model = await OrderStatusHistoryService.GetOrderStatusHistoryAsync(selected.OrderId, selected.OrderLine);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("OrderStatusHistory", "Загрузка истории выполнения заказа.", ex);
            }
        }
    }
}
