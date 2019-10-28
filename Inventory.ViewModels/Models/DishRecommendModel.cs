using System;

namespace Inventory.Models
{
    /// <summary>
    /// Рекомендуемое блюдо
    /// </summary>
    public class DishRecommendModel : BaseModel
    {
        public Guid DishGuid { get; set; }

        public DishModel Dish { get; set; }

        public Guid RecommendGuid { get; set; }

        public DishModel Recommend { get; set; }

        public int RowPosition { get; set; }
    }
}
