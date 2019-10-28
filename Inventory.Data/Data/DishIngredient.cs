﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    /// <summary>
    /// Ингредиент блюда <see cref="Data.Dish"/>
    /// </summary>
    public class DishIngredient : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public Guid RowGuid { get; set; }

        public Guid DishGuid { get; set; }

        public virtual Dish Dish { get; set; }

        public int RowPosition { get; set; }

        public decimal Price { get; set; }

        public byte[] Picture { get; set; }

        public byte[] Thumbnail { get; set; }

        public virtual ICollection<OrderDishIngredient> OrderDishIngredients { get; set; }
    }
}
