using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class OrderDishIngredient : BaseEntity
    {
        public int OrderDishId { get; set; }

        [ForeignKey(nameof(OrderDishId))]
        public OrderDish OrderDish { get; set; }

        /// <summary>
        /// Ссылка на <see cref="Data.Dish"/>
        /// </summary>
        public Guid DishGuid { get; set; }

        [ForeignKey(nameof(DishGuid))]
        public virtual Dish Dish { get; set; }

        /// <summary>
        /// Ссылка на <see cref="Data.DishIngredient"/>
        /// </summary>
        public Guid IngredientGuid { get; set; }

        [ForeignKey(nameof(IngredientGuid))]
        public virtual DishIngredient Ingredient { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
