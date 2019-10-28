namespace Inventory.Models
{
    public class OrderGarnishModel : ObservableObject
    {
        private OrderDishGarnishModel _orderGarnish;

        public int OrderGarnishId
        {
            get => OrderGarnish?.Id ?? 0;
        }

        public string Name
        {
            get => Garnish.Name;
        }

        public decimal Price
        {
            get => Garnish.Price;
        }

        public DishGarnishModel Garnish { get; }

        public OrderDishGarnishModel OrderGarnish
        {
            get => _orderGarnish;
            set
            {
                if (Set(ref _orderGarnish, value))
                {
                    NotifyPropertyChanged(nameof(OrderGarnishId));
                }
            }
        }

        public OrderGarnishModel(DishGarnishModel garnish, OrderDishGarnishModel orderGarnish)
        {
            Garnish = garnish ?? throw new System.ArgumentNullException(nameof(garnish));
            OrderGarnish = orderGarnish;
        }
    }
}
