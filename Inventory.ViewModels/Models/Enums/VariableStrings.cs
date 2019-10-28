namespace Inventory.Models.Enums
{
    /// <summary>
    /// Название настроек <see cref="VariableModel"/>
    /// </summary>
    public static class VariableStrings
    {
        /// <summary>
        /// Признак согласия Гостя на использование его номера телефона для рассылки информационных сообщений
        /// </summary>
        public static string SignOfConsentDefault { get => "SignOfConsentDefault"; }

        /// <summary>
        /// Настройка интервала "от" для доставки на ближайшее время (в минутах)
        /// </summary>
        public static string DeliverySoonIntervalFrom { get => "DeliveryTypeSoon_IntervalFrom"; }

        /// <summary>
        /// Настройка интервала "до" для доставки на ближайшее время (в минутах)
        /// </summary>
        public static string DeliverySoonIntervalTo { get => "DeliveryTypeSoon_IntervalTo"; }

        /// <summary>
        /// Ширина превью картинки
        /// </summary>
        public static string ThumbnailWidth { get => "ThumbnailWidth"; }

        /// <summary>
        /// Высота превью картинки
        /// </summary>
        public static string ThumbnailHeight { get => "ThumbnailHeight"; }

        /// <summary>
        /// Минимальная сума заказа
        /// </summary>
        public static string MinimumOrderSum { get => "MinimumOrderSum"; }
    }
}
