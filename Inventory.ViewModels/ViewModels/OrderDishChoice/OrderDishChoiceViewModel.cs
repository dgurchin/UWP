using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Common;
using Inventory.Models;
using Inventory.Models.Enums;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrderDishChoiceViewModel : ViewModelBase
    {
        public DishListViewModel DishesList { get; }
        public OrderDishChoiceDishesFilterViewModel Filter { get; }
        public OrderDishListViewModel OrderDishes { get; }

        public ICommand GoBackCommand { get; }

        public ICommand AppendDishCommand { get; }

        private OrderDetailsArgs OrderDetailArgs { get; set; }

        public OrderDishChoiceViewModel(IDishService dishService, IOrderDishService orderDishService, ICommonServices commonServices)
            : base(commonServices)
        {
            DishesList = new DishListViewModel(dishService, commonServices);
            Filter = new OrderDishChoiceDishesFilterViewModel(DishesList, commonServices);
            OrderDishes = new OrderDishListViewModel(orderDishService, commonServices);

            GoBackCommand = new RelayCommand(GoBack, CanGoBack);
            AppendDishCommand = new RelayCommand(AppendDish);
        }

        public async Task LoadAsync(OrderDetailsArgs args)
        {
            OrderDetailArgs = args;
            await DishesList.LoadAsync(new DishListArgs());
            await OrderDishes.LoadAsync(new OrderDishListArgs { OrderGuid = args.OrderGuid });
        }

        public void Unload()
        {
            DishesList.Unload();
            OrderDishes.Unload();
        }

        public void Subscribe()
        {
            DishesList.Subscribe();
            OrderDishes.Subscribe();
        }

        public void Unsubscribe()
        {
            DishesList.Unsubscribe();
            OrderDishes.Unsubscribe();
        }

        private bool CanGoBack()
        {
            try
            {
                return NavigationService.CanGoBack;
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
        }

        private void GoBack()
        {
            NavigationService.GoBack();
        }

        private async void AppendDish()
        {
            var selectedDish = DishesList.SelectedItem;
            if (selectedDish == null)
            {
                await DialogService.ShowAsync("Не выбрано блюдо", "Выберите блюдо для добавления");
                return;
            }

            await NavigationService.CreateNewViewAsync<OrderDishDetailsViewModel>(new OrderDishDetailsArgs
            {
                OrderGuid = OrderDetailArgs.OrderGuid,
                OrderDish = new OrderDishModel
                {
                    OrderGuid = OrderDetailArgs.OrderGuid,
                    DishGuid = selectedDish.RowGuid,
                    Price = selectedDish.Price,
                    Quantity = 1,
                    TaxTypeId = selectedDish.TaxType?.Id ?? (int)TaxTypeEnum.Nds0
                }
            });
        }
    }
}
