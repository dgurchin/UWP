namespace Inventory.Models.Enums
{
    public enum PaymentTypeEnum
    {
        /// <summary>
        /// Наличные
        /// </summary>
        Cash = 1,

        /// <summary>
        /// Кредитная карта
        /// </summary>
        CreditCard = 2,

        /// <summary>
        /// Оплата на сайте
        /// </summary>
        Online = 3,

        /// <summary>
        /// Безнал
        /// </summary>
        Cashless = 4,

        /// <summary>
        /// Сертификат
        /// </summary>
        Certificate = 5,

        /// <summary>
        /// Взаимозачет
        /// </summary>
        Netting = 6,

        /// <summary>
        /// Неплательщики
        /// </summary>
        Defaulters = 7
    }
}
