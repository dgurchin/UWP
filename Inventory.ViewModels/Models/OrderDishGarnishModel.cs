using System;

namespace Inventory.Models
{
    public class OrderDishGarnishModel : BaseModel
    {
        public int OrderDishId { get; set; }

        public Guid DishGuid { get; set; }

        public Guid GarnishGuid { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is OrderDishGarnishModel)
            {
                Merge((OrderDishGarnishModel)source);
            }
        }

        private void Merge(OrderDishGarnishModel source)
        {
            Id = source.Id;
            OrderDishId = source.OrderDishId;
            DishGuid = source.DishGuid;
            GarnishGuid = source.GarnishGuid;
            Quantity = source.Quantity;
            Price = source.Price;
        }
    }
}
