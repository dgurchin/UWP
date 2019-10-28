using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class OrderStatusSequence : BaseEntity
    {
        /// <summary>
        /// Статус перед переходом
        /// </summary>
        public int? StatusIdBeginning { get; set; }

        [ForeignKey(nameof(StatusIdBeginning))]
        public virtual OrderStatus OrderStatusBeginning { get; set; }

        /// <summary>
        /// Статус после перехода
        /// </summary>
        public int? StatusIdEnd { get; set; }

        [ForeignKey(nameof(StatusIdEnd))]
        public virtual OrderStatus OrderStatusEnd { get; set; }

        /// <summary>
        /// Направление перехода - Beginning/End (к вып. заказа) 
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Дополнительные условия такого изменения статуса
        /// </summary>
        public string Algorithm { get; set; }

        /// <summary>
        /// Комментарий  
        /// </summary>
        public string Comment { get; set; }
    }
}
