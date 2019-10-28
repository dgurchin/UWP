using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class OrderStatusHistory : BaseEntity
    {
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        //[Key, Column(Order = 1)]
        public int OrderLine { get; set; }

        /// <summary>
        /// Статус перед переходом
        /// </summary>
        public int StatusIdBeginning { get; set; }

        [ForeignKey(nameof(StatusIdBeginning))]
        public virtual OrderStatus OrderStatusBeginning { get; set; }

        /// <summary>
        /// Статус после перехода
        /// </summary>
        public int StatusIdEnd { get; set; }

        [ForeignKey(nameof(StatusIdEnd))]
        public virtual OrderStatus OrderStatusEnd { get; set; }

        /// <summary>
        /// Пользователь, установивший текущий статус
        /// </summary>
        [MaxLength(50)]
        public string StatusUser { get; set; }

        /// <summary>
        /// Дата время перехода в StatusIdEnd
        /// </summary>
        public DateTimeOffset? StatusDate { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }
    }
}

