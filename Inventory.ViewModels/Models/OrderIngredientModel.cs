namespace Inventory.Models
{
    public class OrderIngredientModel : ObservableObject
    {
        private bool _isSelected;

        public DishIngredientModel Ingredient { get; }

        public OrderDishIngredientModel OrderIngredient { get; set; }

        public int OrderIngredientId
        {
            get => OrderIngredient?.Id ?? 0;
        }

        public string Name
        {
            get => Ingredient.Name;
        }

        public decimal Price
        {
            get => Ingredient.Price;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        public OrderIngredientModel(DishIngredientModel ingredient, OrderDishIngredientModel orderIngredient)
        {
            Ingredient = ingredient ?? throw new System.ArgumentNullException(nameof(ingredient));
            OrderIngredient = orderIngredient;
            IsSelected = OrderIngredientId > 0;
        }
    }
}
