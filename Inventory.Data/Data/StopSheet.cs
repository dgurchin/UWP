using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    /// <summary>
    /// Стоп лист
    /// </summary>
    public class StopSheet : BaseEntity
    {
        public Guid RowGuid { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid DishGuid { get; set; }

        public virtual Dish Dish { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
