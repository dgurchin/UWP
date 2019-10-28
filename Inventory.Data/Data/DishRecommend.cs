using System;

namespace Inventory.Data
{
    /// <summary>
    /// Рекомендация блюда
    /// </summary>
    public class DishRecommend : BaseEntity
    {
        public Guid DishGuid { get; set; }

        public virtual Dish Dish { get; set; }

        public Guid RecommendGuid { get; set; }

        public virtual Dish RecommendDish { get; set; }

        public int RowPosition { get; set; }
    }
}
