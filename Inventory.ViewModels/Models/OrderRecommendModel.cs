using System;

namespace Inventory.Models
{
    /// <summary>
    /// Рекомендуемое блюдо в блюде заказа
    /// </summary>
    public class OrderRecommendModel : ObservableObject
    {
        /// <summary>
        /// Рекомендуемое блюдо
        /// </summary>
        public DishRecommendModel DishRecommend { get; }

        /// <summary>
        /// <see cref="Guid"/> рекомендуемоего блюда
        /// </summary>
        public Guid RecommendGuid => DishRecommend.RecommendGuid;

        public string Name => DishRecommend.Recommend.Name;

        public decimal Price => DishRecommend.Recommend.Price;

        public int TaxTypeId => DishRecommend.Recommend.TaxType?.Id ?? 0;

        public int RowPosition => DishRecommend.RowPosition;

        public OrderRecommendModel(DishRecommendModel recommendModel)
        {
            DishRecommend = recommendModel ?? throw new ArgumentNullException(nameof(recommendModel));
            if (recommendModel.Recommend == null)
            {
                throw new ArgumentNullException($"{nameof(recommendModel)}.{nameof(recommendModel.Recommend)}");
            }
        }
    }
}
