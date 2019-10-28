using System;

namespace Inventory.Data
{
    /// <summary>
    /// Особеность блюда <see cref="Data.Dish"/> (изображение: острое и т.д.) 
    /// </summary>
    public class DishMark : BaseEntity
    {
        public Guid RowGuid { get; set; }

        public Guid DishGuid { get; set; }

        public virtual Dish Dish { get; set; }

        public Guid MarkGuid { get; set; }

        public virtual Mark Mark { get; set; }
    }
}
