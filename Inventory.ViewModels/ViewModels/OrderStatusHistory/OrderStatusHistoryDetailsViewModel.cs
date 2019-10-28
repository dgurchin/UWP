using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Inventory.Common;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region OrderStatusHistoryDetailsArgs
    public class OrderStatusHistoryDetailsArgs
    {
        static public OrderStatusHistoryDetailsArgs CreateDefault() => new OrderStatusHistoryDetailsArgs();

        public int OrderId { get; set; }
        public int OrderLine { get; set; }

        public bool IsNew => OrderLine <= 0;
    }
    #endregion

    public class OrderStatusHistoryDetailsViewModel : GenericDetailsViewModel<OrderStatusHistoryModel>
    {
        public OrderStatusHistoryDetailsViewModel(IOrderStatusHistoryService orderStatusHistoryService, ICommonServices commonServices) : base(commonServices)
        {
            OrderStatusHistoryService = orderStatusHistoryService;
        }
        public IOrderStatusHistoryService OrderStatusHistoryService { get; }



        public override bool ItemIsNew => Item?.IsNew ?? true;
        public OrderStatusHistoryDetailsArgs ViewModelArgs { get; private set; }

        public int OrderId { get; set; }

        public void  LoadAsync(OrderStatusHistoryDetailsArgs args)
        {
            ViewModelArgs = args ?? OrderStatusHistoryDetailsArgs.CreateDefault();
            OrderId = ViewModelArgs.OrderId;

            if (ViewModelArgs.IsNew)
            {
                IsEditMode = true;
            }
        }
        public void Unload()
        {
            ViewModelArgs.OrderId = Item?.OrderId ?? 0;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderStatusHistoryDetailsViewModel, OrderStatusHistoryModel>(this, OnDetailsMessage);
            MessageService.Subscribe<OrderStatusHistoryListViewModel>(this, OnListMessage);
            //MessageService.Subscribe<OrderDetailsViewModel>(this, OnListMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderStatusHistoryDetailsArgs CreateArgs()
        {
            return new OrderStatusHistoryDetailsArgs
            {
                OrderId = Item?.OrderId ?? 0,
                OrderLine = Item?.OrderLine ?? 0
            };
        }

        protected async override Task<bool> SaveItemAsync(OrderStatusHistoryModel model)
        {
            try
            {
                StartStatusMessage("Сохранение заказа...");
                await Task.Delay(100);
                await OrderStatusHistoryService.UpdateOrderStatusHistoryAsync(model);
                EndStatusMessage("Заказ сохранен");
                LogInformation("OrderItem", "Save", "Заказ сохранен успешно", $"Заказ №{model.OrderId}, {model.OrderLine} был сохранен успешно.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Ошибка сохранения заказа: {ex.Message}");
                LogException("OrderItem", "Save", ex);
                return false;
            }
        }

        protected async override Task<bool> DeleteItemAsync(OrderStatusHistoryModel model)
        {
            try
            {
                StartStatusMessage("Заказ удаляется...");
                await Task.Delay(100);
                await OrderStatusHistoryService.DeleteOrderStatusHistoryAsync(model);
                EndStatusMessage("Заказ удален");
                LogWarning("OrderItem", "Delete", "Заказ удален", $"История по заказу №{model.OrderId}, {model.OrderLine} удалена.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Ошибка удаления заказа: {ex.Message}");
                LogException("OrderItem", "Delete", ex);
                return false;
            }
        }

        protected async override Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить текущий заказ?", "Ok", "Отмена");
        }

        override protected IEnumerable<IValidationConstraint<OrderStatusHistoryModel>> GetValidationConstraints(OrderStatusHistoryModel model)
        {
            yield return new RequiredConstraint<OrderStatusHistoryModel>("StatusIdBeginning", m => m.StatusIdBeginning);
            yield return new NonZeroConstraint<OrderStatusHistoryModel>("StatusIdEnd", m => m.StatusIdEnd);
            yield return new PositiveConstraint<OrderStatusHistoryModel>("StatusUser", m => m.StatusUser);
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(OrderStatusHistoryDetailsViewModel sender, string message, OrderStatusHistoryModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.OrderId == current?.OrderId && changed.OrderLine == current?.OrderLine)
                {
                    switch (message)
                    {
                        case Messages.ItemChanged:
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await OrderStatusHistoryService.GetOrderStatusHistoryAsync(current.OrderId, current.OrderLine);
                                    item = item ?? new OrderStatusHistoryModel { OrderId = OrderId, OrderLine = ViewModelArgs.OrderLine, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("Предупреждение: Соодержимое заказа изменено извне");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("OrderStatusHistory", "Handle Changes", ex);
                                }
                            });
                            break;
                        case Messages.ItemDeleted:
                            await OnItemDeletedExternally();
                            break;
                    }
                }
            }
        }

        private async void OnListMessage(OrderStatusHistoryListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case Messages.ItemsDeleted:
                        if (args is IList<OrderStatusHistoryModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.OrderId == current.OrderId && r.OrderLine == current.OrderLine))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case Messages.ItemRangesDeleted:
                        try
                        {
                            var model = await OrderStatusHistoryService.GetOrderStatusHistoryAsync(current.OrderId, current.OrderLine);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("OrderStatusHistory", "Handle Ranges Deleted", ex);
                        }
                        break;
                }
            }
        }
        private async Task OnItemDeletedExternally()
        {
            await ContextService.RunAsync(() =>
            {
                CancelEdit();
                IsEnabled = false;
                StatusMessage("Предупреждение: Этот заказ удален извне");
            });
        }
    }
}
