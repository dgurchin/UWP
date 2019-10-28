namespace Inventory.Models.Enums
{
    /// <summary>
    /// Все состояния заказа
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// Не выбран
        /// </summary>
        None = 0,

        /// <summary>
        /// Новый с сайта
        /// </summary>
        NewFromSite = 1,

        /// <summary>
        /// Уточнение
        /// </summary>
        Clarification = 2,

        /// <summary>
        /// Оформление
        /// </summary>
        Registration = 3,

        /// <summary>
        /// Ожидание
        /// </summary>
        Waiting = 4,

        /// <summary>
        /// Принят
        /// </summary>
        Accepted = 5,

        /// <summary>
        /// Таймер
        /// </summary>
        Timer = 6,

        /// <summary>
        /// В работе
        /// </summary>
        InProcess = 7,

        /// <summary>
        /// Собран
        /// </summary>
        Gathered = 8,

        /// <summary>
        /// Едет
        /// </summary>
        OnRoad = 9,

        /// <summary>
        /// Доставлен
        /// </summary>
        Delivered = 10,

        /// <summary>
        /// Закрыт
        /// </summary>
        Closed = 11,

        /// <summary>
        /// Отклонен
        /// </summary>
        Canceled = 12,

        /// <summary>
        /// Корзина
        /// </summary>
        Basket = 13
    }
}
