using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class OrderDishGarnish : BaseEntity
    {
        /// <summary>
        /// Ссылка на <see cref="Data.OrderDish"/>
        /// </summary>
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
        /// Ссылка на <see cref="Data.DishGarnish"/>
        /// </summary>
        public Guid GarnishGuid { get; set; }
        
        [ForeignKey(nameof(GarnishGuid))]
        public virtual DishGarnish Garnish { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
    }
}
