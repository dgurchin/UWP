using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    /// <summary>
    /// Единица измерения продукта
    /// </summary>
    public class DishUnit : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public Guid RowGuid { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
