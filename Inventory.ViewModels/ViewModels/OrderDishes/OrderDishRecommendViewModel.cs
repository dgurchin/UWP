using System.Windows.Input;

using Inventory.Common;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrderDishRecommendViewModel : GenericListViewModel<OrderRecommendModel>
    {
        public OrderDishDetailsArgs ViewModelArgs { get; set; }

        public ICommand RecommendInvokedCommand => new RelayCommand<OrderRecommendModel>(RecommendInvoked);

        public OrderDishRecommendViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        protected override void OnDeleteSelection()
        {
        }

        protected override void OnNew()
        {
        }

        protected override void OnRefresh()
        {
        }

        private async void RecommendInvoked(OrderRecommendModel orderRecommend)
        {
            await NavigationService.CreateNewViewAsync<OrderDishDetailsViewModel>(new OrderDishDetailsArgs
            {
                OrderGuid = ViewModelArgs.OrderGuid,
                OrderDish = new OrderDishModel
                {
                    OrderGuid = ViewModelArgs.OrderGuid,
                    DishGuid = orderRecommend.RecommendGuid,
                    Price = orderRecommend.Price,
                    Quantity = 1m,
                    TaxTypeId = orderRecommend.TaxTypeId
                }
            });
        }
    }
}
