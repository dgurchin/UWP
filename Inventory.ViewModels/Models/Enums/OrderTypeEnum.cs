namespace Inventory.Models.Enums
{
    /// <summary>
    /// Тип заказа
    /// </summary>
    public enum OrderTypeEnum
    {
        /// <summary>
        /// Доставка
        /// </summary>
        Delivery = 1,

        /// <summary>
        /// На вынос
        /// </summary>
        Takeaway = 2,

        /// <summary>
        /// Дозаказ доставка
        /// </summary>
        OrderDelivery = 3,

        /// <summary>
        /// Дозаказ самовынос
        /// </summary>
        OrderTakeaway = 4,

        /// <summary>
        /// Food Tigra
        /// </summary>
        FoodTigra = 5
    }
}
