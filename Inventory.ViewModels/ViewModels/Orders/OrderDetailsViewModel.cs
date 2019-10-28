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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Common;
using Inventory.Models;
using Inventory.Models.Enums;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region OrderDetailsArgs
    public class OrderDetailsArgs
    {
        static public OrderDetailsArgs CreateDefault() => new OrderDetailsArgs { CustomerId = 0 };

        public int CustomerId { get; set; }
        public Guid OrderGuid { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsNew => OrderGuid == Guid.Empty;
    }
    #endregion

    public class OrderDetailsViewModel : GenericDetailsViewModel<OrderModel>
    {
        #region Private fields
        private IList<OrderStatusModel> _orderStatuses;
        private IList<StreetModel> _streetModels;

        private const int MIN_PHONE_NUMBER_LENGHT = 10;
        #endregion

        #region Services
        private IOrderService OrderService { get; }
        private IOrderDishService OrderDishService { get; }
        private IOrderStatusHistoryService OrderStatusHistoryService { get; }
        private IVariableService VariableService { get; }
        
        private IOrderLoadingMonitor OrderLoadingMonitor { get; }
        private ICustomerService CustomerService { get; }
        #endregion

        #region Public properties
        public OrderDetailsArgs ViewModelArgs { get; private set; }
        public bool CopiedItem { get => !(Item.IsNew || Item.IsNewCustomer); }
        public override bool ItemIsNew => Item?.IsNew ?? true;
        public override string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => Item?.Customer == null ? "Новый заказ" : $"Новый заказ, {Item?.Customer?.FullName}";
        public string TitleEdit => Item == null ? "Заказ" : $"Заказ #{Item?.Id}";

        public bool CanEditPhone
        {
            get
            {
                if (EditableItem == null)
                {
                    return false;
                }

                // Если заказ новый и без начального телефона - то можем редактировать
                if (ItemIsNew)
                {
                    if (string.IsNullOrWhiteSpace(ViewModelArgs.PhoneNumber))
                    {
                        return true;
                    }
                    return false;
                }

                return string.IsNullOrEmpty(EditableItem.PhoneNumber);
            }
        }

        public bool IsValidPhone
        {
            get
            {
                if (EditableItem == null)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(EditableItem.PhoneNumber))
                {
                    return false;
                }

                return EditableItem.PhoneNumber.Length >= MIN_PHONE_NUMBER_LENGHT;
            }
        }

        public bool CanEditCustomer
        {
            get
            {
                return CanEditPhone && IsValidPhone && EditableItem.Customer == null;
            }
        }

        public IList<OrderStatusModel> OrderStatuses
        {
            get => _orderStatuses;
            private set => Set(ref _orderStatuses, value);
        }

        public IList<StreetModel> StreetModels
        {
            get => _streetModels;
            private set => Set(ref _streetModels, value);
        }

        public OrderDishListViewModel OrderDishList { get; set; }
        public OrderDishDetailsViewModel OrderDishDetails { get; set; }

        #endregion

        #region Commands
        public ICommand CustomerSelectedCommand => new RelayCommand<CustomerModel>(CustomerSelected);
        public ICommand StreetSelectedCommand => new RelayCommand<StreetModel>(StreetSelected);
        public ICommand OrderCopyCommand => new RelayCommand(OrderCopy);
        #endregion

        #region Ctor
        public OrderDetailsViewModel(IOrderService orderService, IOrderDishService orderDishService, IOrderStatusHistoryService orderStatusHistoryService,
            IVariableService variableService, ICommonServices commonServices,
            IDishService dishService, IDishGarnishService dishGarnishService, IOrderDishGarnishService orderDishGarnishService,
            IDishIngredientService dishIngredientService,
            IOrderDishIngredientService orderDishIngredientService,
            IOrderDishModifierService orderDishModifierService,
            IDishRecommendService dishRecommendService,
            IOrderLoadingMonitor orderLoadingMonitor, ICustomerService customerService)
            : base(commonServices)
        {
            OrderService = orderService;
            OrderDishService = orderDishService;
            OrderStatusHistoryService = orderStatusHistoryService;
            VariableService = variableService;
            OrderLoadingMonitor = orderLoadingMonitor;
            CustomerService = customerService;

            OrderDishList = new OrderDishListViewModel(orderDishService, commonServices);
            OrderDishDetails = new OrderDishDetailsViewModel(dishService, orderDishService, dishGarnishService, orderDishGarnishService,
                dishIngredientService, orderDishIngredientService, orderDishModifierService, dishRecommendService, commonServices);
        }
        #endregion

        #region Public methods

        public async Task LoadAsync(OrderDetailsArgs args)
        {
            ViewModelArgs = args ?? OrderDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                var item = await OrderService.CreateNewOrderAsync(ViewModelArgs.CustomerId, ViewModelArgs.PhoneNumber);
                await LoadCustomerAddressAsync(item);
                
                Item = item;
                ViewModelArgs.OrderGuid = Item.RowGuid;
                IsEditMode = true;
            }
            else
            {
                try
                {
                    var item = await OrderService.GetOrderAsync(ViewModelArgs.OrderGuid);
                    Item = item ?? new OrderModel { RowGuid = ViewModelArgs.OrderGuid, IsEmpty = true };
                }
                catch (Exception ex)
                {
                    LogException("Order", "Load", ex);
                }
            }

            NotifyPropertyChanged(nameof(ItemIsNew));
            NotifyCustomerFields();
        }

        private async Task LoadCustomerAddressAsync(OrderModel item)
        {
            if (item == null || !item.IsAddressRequired)
            {
                return;
            }

            AddressModel address = await CustomerService.GetPrimaryOrFirstAddressAsync(item.CustomerId);
            if (address == null)
            {
                return;
            }

            item.CityId = address.CityId;
            item.City = address.City;
            item.StreetId = address.StreetId;
            item.Street = address.Street;
            item.House = address.House;
            item.Housing = address.Housing;
            item.Apartment = address.Apartment;
            item.Entrance = address.Entrance;
            item.Intercom = address.Intercom;
            item.Floor = address.Floor;
        }

        public void Unload()
        {
            ViewModelArgs.CustomerId = Item?.CustomerId ?? 0;
            ViewModelArgs.OrderGuid = Item?.RowGuid ?? Guid.Empty;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDetailsViewModel, OrderModel>(this, OnDetailsMessage);
            MessageService.Subscribe<OrderListViewModel>(this, OnListMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public OrderDetailsArgs CreateArgs()
        {
            return new OrderDetailsArgs
            {
                CustomerId = Item?.CustomerId ?? 0,
                OrderGuid = Item?.RowGuid ?? Guid.Empty
            };
        }

        #endregion

        #region Protected methods

        protected override async Task<bool> SaveItemAsync(OrderModel editableModel)
        {
            try
            {
                StartStatusMessage("Заказ сохраняется...");
                await Task.Delay(100);
                await OrderService.UpdateOrderAsync(editableModel);
                EndStatusMessage("Order saved");
                LogInformation("Order", "Save", "Заказ сохранен успешно", $"Заказ #{editableModel.Id} был сохранен успешно.");
                NotifyCustomerFields();

                try
                {
                    //Статус заказа изменился (сохраняем запись в истории)
                    if (editableModel.StatusId != Item.StatusId)
                    {
                        var statusHistory = new OrderStatusHistoryModel
                        {
                            OrderId = editableModel.Id,
                            StatusIdBeginning = Item.StatusId,
                            StatusIdEnd = editableModel.StatusId,
                            StatusDate = DateTimeOffset.Now
                        };

                        await Task.Delay(100);
                        await OrderStatusHistoryService.UpdateOrderStatusHistoryAsync(statusHistory);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Ошибка сохранения статуса заказа: {ex.Message}");
                    LogException("OrderStatusHistory", "Save", ex);
                }

                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Ошибка сохранения заказа: {ex.Message}");
                LogException("Order", "Save", ex);
                return false;
            }
        }

        protected override async Task<bool> OnBeforeSaveAsync(OrderModel editableModel)
        {
            if (editableModel.IsMinOrderSumVisible)
            {
                decimal dishesSum = await OrderDishService.GetOrderDishesSumAsync(editableModel.RowGuid);
                bool isOrderSumOk = dishesSum >= editableModel.MinOrderSum;
                if (!isOrderSumOk)
                {
                    string orderMinSumMessage = $"Заказ меньше минимальной сумы: {editableModel.MinOrderSum.ToString("#.##")}, дозаполните заказ!"; 
                    await DialogService.ShowAsync("Минимальная сума заказа", orderMinSumMessage, "Вернутся к заказу");
                }
                return isOrderSumOk;
            }
            return true;
        }

        protected override void OnAfterSave()
        {
            // Сохранить текущий статус, сбрасывается на 0 после записи новых статусов
            var itemStatus = Item.StatusId;
            OrderStatuses = LookupTablesProxy.Instance.GetAllowedOrderStatuses(itemStatus);
            // Обновление текущего и старого статуса
            Item.StatusId = itemStatus;
        }

        protected override async Task<bool> DeleteItemAsync(OrderModel model)
        {
            try
            {
                StartStatusMessage("Deleting order...");
                await Task.Delay(100);
                await OrderService.DeleteOrderAsync(model);
                EndStatusMessage("Order deleted");
                LogWarning("Order", "Delete", "Order deleted", $"Order #{model.Id} was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Order: {ex.Message}");
                LogException("Order", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить текущий заказ?", "Ok", "Отмена");
        }

        protected override IEnumerable<IValidationConstraint<OrderModel>> GetValidationConstraints(OrderModel editableModel)
        {
            //yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Клиент", m => m.CustomerId);

            yield return new RequiredConstraint<OrderModel>("Номер телефона", m => m.PhoneNumber);
            yield return new RequiredConstraint<OrderModel>("Имя клиента", m => m.EditableCustomerName);

            var statuses = LookupTables.GetAllowedOrderStatuses(Item.StatusId).Select(order => order.Id).ToList();
            // Для проверки допустимости изменения статуса > 0
            yield return new InListGreaterThanZeroConstraint<OrderModel>("Статус", m => m.StatusId, statuses);

            yield return new RequiredConstraint<OrderModel>("Тип заказа", m => m.OrderTypeId);
            yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Тип заказа", m => m.OrderTypeId);

            yield return new RequiredConstraint<OrderModel>("Тип оплаты", m => m.PaymentTypeId);
            yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Тип оплаты", m => m.PaymentTypeId);

            yield return new RequiredConstraint<OrderModel>("Тип доставки", m => m.DeliveryTypeId);
            yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Тип доставки", m => m.DeliveryTypeId);

            yield return new RequiredConstraint<OrderModel>("Дата доставки", m => m.DeliveryDate);

            yield return new RequiredConstraint<OrderModel>("Ресторан приготовления", m => m.RestaurantId);
            yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Ресторан приготовления", m => m.RestaurantId);

            yield return new RequiredConstraint<OrderModel>("Количество персон", m => m.NumOfPeople);
            yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Количество персон", m => m.NumOfPeople);

            if (editableModel.IsAddressRequired)
            {
                yield return new RequiredConstraint<OrderModel>("Город", m => m.CityId);
                yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Город", m => m.CityId);

                yield return new RequiredConstraint<OrderModel>("Улица", m => m.StreetId);
                yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Улица", m => m.StreetId);

                yield return new RequiredConstraint<OrderModel>("Номер дома", m => m.House);
                yield return new RequiredGreaterThanZeroConstraint<OrderModel>("Номер дома", m => m.House);
            }

            if (editableModel.IsReasonVisible)
            {
                yield return new RequiredConstraint<OrderModel>("Причина уточнения", m => m.Reason);
            }
        }

        protected override void OnItemChanged(OrderModel item)
        {
            if (item == null)
            {
                OrderStatuses = new List<OrderStatusModel>();
                StreetModels = new List<StreetModel>();
                return;
            }

            OrderStatuses = LookupTablesProxy.Instance.GetAllowedOrderStatuses(item.StatusId);
            StreetModels = LookupTablesProxy.Instance.GetStreets(item.CityId);
            item.IsDeliveryDateReadOnly = item.DeliveryTypeId == (int)DeliveryTypeEnum.Soon;

            UpdateOrderMinSum(item);
            NotifyCustomerFields();
        }

        protected override void OnEditableItemChanged(OrderModel editableItem)
        {
            UpdateOrderMinSum(editableItem);
            NotifyCustomerFields();
        }

        protected override void OnEditableItemPropertyChanged(OrderModel editableItem, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(editableItem.CityId):
                    OnCityChanged(editableItem.CityId);
                    break;
                case nameof(editableItem.DeliveryTypeId):
                    OnDeliveryTypeChanged(editableItem.DeliveryTypeId);
                    break;
                case nameof(editableItem.PhoneNumber):
                    OnPhoneNumberChanged(editableItem.PhoneNumber);
                    break;
            }
        }

        #endregion

        #region Private methods

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(OrderDetailsViewModel sender, string message, OrderModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.Id == current?.Id)
                {
                    switch (message)
                    {
                        case Messages.ItemChanged:
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await OrderService.GetOrderAsync(current.RowGuid);
                                    item = item ?? new OrderModel { Id = current.Id, RowGuid = current.RowGuid, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("ВНИМАНИЕ: этот заказ был изменен извне");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Order", "Обработка изменений", ex);
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

        private async void OnListMessage(OrderListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case Messages.ItemsDeleted:
                        if (args is IList<OrderModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.Id == current.Id))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case Messages.ItemRangesDeleted:
                        try
                        {
                            var model = await OrderService.GetOrderAsync(current.RowGuid);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Order", "Выбранный диапазон удален", ex);
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
                StatusMessage("ВНИМАНИЕ: этот заказ был удален извне");
            });
        }

        private void CustomerSelected(CustomerModel customer)
        {
            if (customer == null)
            {
                ClearCustomer();
            }
            else
            {
                SelectCustomer(customer);
            }
        }

        private async void OnDeliveryTypeChanged(int deliveryTypeId)
        {
            // TODO: Дублирующийся как как при создании новой карточки заказа OrderService.CreateNewOrderAsync
            if (deliveryTypeId == (int)DeliveryTypeEnum.Soon)
            {
                int intervalFrom = await VariableService.GetVariableValueAsync<int>(VariableStrings.DeliverySoonIntervalFrom);
                DateTime deliveryDate = DateTime.Now.AddMinutes(intervalFrom);
                EditableItem.DeliveryDate = deliveryDate;
                EditableItem.DeliveryTime = deliveryDate.TimeOfDay;
                EditableItem.IsDeliveryDateReadOnly = true;
            }
            else
            {
                EditableItem.IsDeliveryDateReadOnly = false;
            }
        }

        private void OnCityChanged(int? cityId)
        {
            EditableItem.StreetId = 0;
            EditableItem.Street = null;
            StreetModels = LookupTablesProxy.Instance.GetStreets(cityId);
        }

        private void StreetSelected(StreetModel street)
        {
            if (street == null)
            {
                EditableItem.StreetId = 0;
                EditableItem.Street = null;
            }
            else
            {
                EditableItem.StreetId = street.Id;
                EditableItem.Street = street;
            }
        }

        private async void OnPhoneNumberChanged(string phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                if (phoneNumber.Length < MIN_PHONE_NUMBER_LENGHT)
                {
                    ClearCustomer();
                }
                else
                {
                    var customers = await CustomerService.FindCustomersByPhoneNumberAsync(phoneNumber);
                    var customer = customers.FirstOrDefault();
                    if (customer != null)
                    {
                        SelectCustomer(customer);
                    }
                    else
                    {
                        ClearCustomer();
                    }
                }
            }
            else
            {
                ClearCustomer();
            }

            NotifyCustomerFields();
        }

        private void ClearCustomer()
        {
            EditableItem.CustomerId = 0;
            EditableItem.Customer = null;
            EditableItem.EditableCustomerName = null;
        }

        private async void SelectCustomer(CustomerModel customer)
        {
            EditableItem.CustomerId = customer.Id;
            EditableItem.Customer = customer;
            EditableItem.EditableCustomerName = customer.FullName;
            await LoadCustomerAddressAsync(EditableItem);
        }

        private void NotifyCustomerFields()
        {
            NotifyPropertyChanged(nameof(CanEditCustomer));
            NotifyPropertyChanged(nameof(CanEditPhone));
            NotifyPropertyChanged(nameof(IsValidPhone));
        }

        private async void UpdateOrderMinSum(OrderModel model)
        {
            if (model == null)
            {
                return;
            }

            model.MinOrderSum = await VariableService.GetVariableValueAsync(VariableStrings.MinimumOrderSum, OrderService.DefaultMinimumOrderSum);
        }

        /// <summary>
        /// Копирование заказа
        /// </summary>
        private async void OrderCopy()
        {
            if (!CopiedItem)
            {
                return;
            }

            bool ret = await DialogService.ShowAsync("Подтверждение копирования", $"Вы уверены, что хотите копировать заказ №{Item?.Id}?", "Ok", "Отмена");
            if (ret)
            {
                await OrderDishList.LoadAsync(new OrderDishListArgs { OrderGuid = Item.RowGuid }, silent: true);

                OrderModel ItemNew = await OrderService.CreateNewOrderAsync(Item.CustomerId);
                ItemNew.Merge(Item);
                ItemNew.CreatedOn = DateTimeOffset.Now;
                ItemNew.RowGuid = Guid.NewGuid();
                ItemNew.Id = 0;
                ItemNew.StatusId = (int)OrderService.DefaultOrderStatus;
                bool retSaveOrder = await SaveItemAsync(ItemNew);

                foreach (var itemDish in OrderDishList.Items)
                {
                    OrderDishModel itemDishNew = new OrderDishModel();
                    itemDishNew.Merge(itemDish);
                    itemDishNew.OrderGuid = ItemNew.RowGuid;
                    itemDishNew.OrderId = ItemNew.Id;
                    itemDishNew.Id = 0;
                    itemDishNew.Garnishes = itemDish.Garnishes;
                    itemDishNew.Ingredients = itemDish.Ingredients;
                    OrderDishDetails.EditableItem = itemDishNew;
                    OrderDishDetails.Item = itemDishNew;
                    await OrderDishDetails.SaveAsync();
                }

                MessageService.Send(this, Messages.ItemCopied, Item);
            }
        }

        #endregion
    }
}
