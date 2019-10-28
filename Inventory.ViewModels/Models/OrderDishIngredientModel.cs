using System;

namespace Inventory.Models
{
    public class OrderDishIngredientModel : BaseModel
    {
        public int OrderDishId { get; set; }

        public Guid DishGuid { get; set; }

        public Guid IngredientGuid { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is OrderDishIngredientModel)
            {
                Merge((OrderDishIngredientModel)source);
            }
        }

        private void Merge(OrderDishIngredientModel source)
        {
            Id = source.Id;
            OrderDishId = source.OrderDishId;
            DishGuid = source.DishGuid;
            IngredientGuid = source.IngredientGuid;
            Quantity = source.Quantity;
            Price = source.Price;
        }
    }
}
