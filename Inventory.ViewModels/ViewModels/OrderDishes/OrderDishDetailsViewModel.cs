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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Common;
using Inventory.Models;
using Inventory.Models.Enums;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region OrderItemDetailsArgs
    public class OrderDishDetailsArgs
    {
        static public OrderDishDetailsArgs CreateDefault() => new OrderDishDetailsArgs();

        public Guid OrderGuid { get; set; }

        public OrderDishModel OrderDish { get; set; }

        public bool IsNew => OrderDish == null || OrderDish.Id <= 0;
    }
    #endregion

    /// <summary>
    /// Блюдо в заказе
    /// </summary>
    public class OrderDishDetailsViewModel : GenericDetailsViewModel<OrderDishModel>
    {
        #region Properties
        public OrderDishDetailsArgs ViewModelArgs { get; private set; }
        public Guid OrderGuid { get; private set; }
        public Guid DishGuid { get; private set; }

        public override string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => $"Добавление блюда";
        public string TitleEdit => $"Блюдо {Item?.Dish?.Name}, №{Item?.Dish?.Id}" ?? String.Empty;
        public override bool ItemIsNew => Item?.IsNew ?? true;

        public OrderDishRecommendViewModel RecommendViewModel { get; }
        #endregion

        #region Commands
        public ICommand DishSelectedCommand => new RelayCommand<DishModel>(DishSelected);
        public ICommand IngredientInvokedCommand => new RelayCommand(IngredientInvoked);
        #endregion

        #region Services
        private IDishService DishService { get; }
        private IOrderDishService OrderDishService { get; }

        private IDishGarnishService DishGarnishService { get; }
        private IOrderDishGarnishService OrderDishGarnishService { get; }

        private IDishIngredientService DishIngredientService { get; }
        private IOrderDishIngredientService OrderDishIngredientService { get; }

        private IDishRecommendService DishRecommendService { get; }

        private IOrderDishModifierService OrderDishModifierService { get; }
        #endregion

        #region Ctor
        public OrderDishDetailsViewModel(IDishService dishService, IOrderDishService orderDishService,
            IDishGarnishService dishGarnishService, IOrderDishGarnishService orderDishGarnishService,
            IDishIngredientService dishIngredientService, IOrderDishIngredientService orderDishIngredientService,
            IOrderDishModifierService orderDishModifierService, IDishRecommendService dishRecommendService, 
            ICommonServices commonServices)
            : base(commonServices)
        {
            DishService = dishService;
            OrderDishService = orderDishService;

            DishGarnishService = dishGarnishService;
            OrderDishGarnishService = orderDishGarnishService;

            DishIngredientService = dishIngredientService;
            OrderDishIngredientService = orderDishIngredientService;
            OrderDishModifierService = orderDishModifierService;

            DishRecommendService = dishRecommendService;

            RecommendViewModel = new OrderDishRecommendViewModel(commonServices);

            SaveBehaviour = DetailSaveBehaviour.SaveAndClose;
        }
        #endregion

        #region Public methods

        public OrderDishDetailsArgs CreateArgs()
        {
            return new OrderDishDetailsArgs
            {
                OrderGuid = Item?.OrderGuid ?? Guid.Empty
            };
        }

        public async Task LoadAsync(OrderDishDetailsArgs args)
        {
            StatusReady();
            DisableThisView("Открытие продукта...");
            try
            {
                ViewModelArgs = args ?? OrderDishDetailsArgs.CreateDefault();
                OrderGuid = ViewModelArgs.OrderGuid;
                DishGuid = ViewModelArgs.OrderDish?.DishGuid ?? Guid.Empty;

                if (ViewModelArgs.IsNew)
                {
                    OrderDishModel item;
                    if (args.OrderDish == null)
                    {
                        item = new OrderDishModel { OrderGuid = OrderGuid, Quantity = 1, TaxTypeId = (int)TaxTypeEnum.Nds0 };
                    }
                    else
                    {
                        if (args.OrderDish.Dish == null)
                        {
                            args.OrderDish.Dish = await DishService.GetDishAsync(DishGuid);
                        }
                        item = args.OrderDish;
                    }
                    await LoadDictinariesAsync(item);
                    Item = item;
                    IsEditMode = true;
                }
                else
                {
                    try
                    {
                        var item = await OrderDishService.GetOrderDishAsync(ViewModelArgs.OrderDish.Id);
                        if (item.Dish == null)
                        {
                            item.Dish = await DishService.GetDishAsync(DishGuid);
                        }
                        await LoadDictinariesAsync(item);
                        Item = item ?? new OrderDishModel { OrderGuid = OrderGuid, IsEmpty = true };
                    }
                    catch (Exception ex)
                    {
                        LogException("OrderDish", "Load", ex);
                    }
                }
            }
            finally
            {
                EnableThisView("");
            }
        }

        public void Unload()
        {
            ViewModelArgs.OrderGuid = Item?.OrderGuid ?? Guid.Empty;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDishDetailsViewModel, OrderDishModel>(this, OnDetailsMessage);
            MessageService.Subscribe<OrderDishListViewModel>(this, OnListMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        #endregion

        #region Protected methods

        protected override async Task<bool> SaveItemAsync(OrderDishModel editableItem)
        {
            try
            {
                StartStatusMessage("Сохранение заказа...");
                await Task.Delay(100);
                await OrderDishService.UpdateOrderDishAsync(editableItem);

                if (editableItem.Garnishes.Count > 0 && editableItem.SelectedGarnish != null)
                {
                    await OrderDishGarnishService.CheckAndDeleteGarnishesAsync(editableItem.Garnishes, editableItem.SelectedGarnish.OrderGarnishId);
                    await OrderDishGarnishService.CheckAndAppendGarnishAsync(editableItem.Id, DishGuid, editableItem.SelectedGarnish);
                }

                await OrderDishIngredientService.CheckAndDeleteIngredientsAsync(editableItem.Ingredients);
                await OrderDishIngredientService.CheckAndAppendIngredientsAsync(editableItem.Ingredients, editableItem.Id);

                EndStatusMessage("Заказ сохранен");
                LogInformation("OrderDish", "Save", "Заказ сохранен успешно", $"Заказ № {editableItem.Id} был сохранен успешно.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Ошибка сохранения заказа: {ex.Message}");
                LogException("OrderDish", "Save", ex);
                return false;
            }
        }

        protected override void OnBeforeMerge(OrderDishModel item, OrderDishModel editableItem)
        {
            if (item == null || editableItem == null)
                return;

            item.Garnishes = editableItem.Garnishes;
            item.SelectedGarnish = editableItem.SelectedGarnish;
            item.Ingredients = editableItem.Ingredients;
        }

        protected override void OnBeforeMergeEditable(OrderDishModel item, OrderDishModel editableItem)
        {
            if (editableItem == null || item == null)
                return;

            editableItem.Garnishes = item.Garnishes;
            editableItem.SelectedGarnish = item.SelectedGarnish;
            editableItem.Ingredients = item.Ingredients;
        }

        protected override async Task<bool> DeleteItemAsync(OrderDishModel model)
        {
            try
            {
                StartStatusMessage("Заказ удаляется...");
                await Task.Delay(100);
                await OrderDishService.DeleteOrderDishAsync(model);
                EndStatusMessage("Заказ удален");
                LogWarning("OrderDish", "Delete", "Заказ удален", $"Данные по заказу №{model.Id} удалены.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Ошибка удаления заказа: {ex.Message}");
                LogException("OrderDish", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить текущий заказ?", "Ok", "Отмена");
        }

        protected override IEnumerable<IValidationConstraint<OrderDishModel>> GetValidationConstraints(OrderDishModel model)
        {
            yield return new RequiredConstraint<OrderDishModel>("Блюдо", m => m.DishGuid);
            if (model.Garnishes.Count > 0)
            {
                yield return new RequiredConstraint<OrderDishModel>("Основа", m => m.SelectedGarnish);
            }
            yield return new NonZeroConstraint<OrderDishModel>("Количество", m => m.Quantity);
            yield return new PositiveConstraint<OrderDishModel>("Количество", m => m.Quantity);
            yield return new LessThanConstraint<OrderDishModel>("Количество", m => m.Quantity, 100);
            yield return new PositiveConstraint<OrderDishModel>("Скидка", m => m.Discount);
            yield return new NonGreaterThanConstraint<OrderDishModel>("Скидка", m => m.Discount, (double)model.Subtotal, "'Итого'");
        }
        
        #endregion

        #region Private methods

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(OrderDishDetailsViewModel sender, string message, OrderDishModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.Id == current?.Id && changed.OrderGuid == current?.OrderGuid)
                {
                    switch (message)
                    {
                        case Messages.ItemChanged:
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await OrderDishService.GetOrderDishAsync(current.Id);
                                    item = item ?? new OrderDishModel
                                    {
                                        OrderGuid = OrderGuid,
                                        IsEmpty = true
                                    };
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
                                    LogException("OrderItem", "Handle Changes", ex);
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

        private async void OnListMessage(OrderDishListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case Messages.ItemsDeleted:
                        if (args is IList<OrderDishModel> deletedModels)
                        {
                            if (deletedModels.Any(x => x.Id == current.Id))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case Messages.ItemRangesDeleted:
                        try
                        {
                            var model = await OrderDishService.GetOrderDishAsync(current.Id);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("OrderItem", "Handle Ranges Deleted", ex);
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

        private void DishSelected(DishModel dish)
        {
            EditableItem.DishGuid = dish.RowGuid;
            EditableItem.Dish = dish;
            EditableItem.Price = dish.Price;

            EditableItem.NotifyChanges();
        }

        private async Task LoadDictinariesAsync(OrderDishModel item)
        {
            await LoadGarnishesAsync(item);
            await LoadIngredients(item);
            await LoadRecommends(item);
            await LoadModifiers(item);
        }

        private async Task LoadGarnishesAsync(OrderDishModel item)
        {
            var dishGarnishes = await DishGarnishService.GetDishGarnishesAsync(DishGuid);
            item.Garnishes = await OrderDishGarnishService.GetOrderViewGarnishesAsync(dishGarnishes, item.Id);
            item.SelectedGarnish = item.Garnishes.FirstOrDefault(x => x.OrderGarnish != null);
        }

        private async Task LoadIngredients(OrderDishModel item)
        {
            var dishIngredients = await DishIngredientService.GetDishIngredientsAsync(DishGuid);
            item.Ingredients = await OrderDishIngredientService.GetOrderViewIngredientsAsync(dishIngredients, item.Id);
        }

        private async Task LoadRecommends(OrderDishModel item)
        {
            var dishRecommends = await DishRecommendService.GetOrderRecommendsAsync(item.DishGuid);

            RecommendViewModel.ViewModelArgs = ViewModelArgs;
            RecommendViewModel.Items = dishRecommends;
            RecommendViewModel.ItemsCount = dishRecommends.Count;
        }

        private async Task LoadModifiers(OrderDishModel item)
        {
            var modifiers = await OrderDishModifierService.GetRelatedDishModifiersAsync(item.DishGuid);
            System.Diagnostics.Trace.WriteLine("Loaded modifiers count: " + modifiers.Count);
        }

        private void IngredientInvoked()
        {
            EditableItem.UpdateTotals();
        }

        #endregion
    }
}
