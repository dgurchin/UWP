using System;

namespace Inventory.Data
{
    /// <summary>
    /// Заказ курьера
    /// </summary>
    public class CourierOrder : BaseEntity
    {
        public Guid RowGuid { get; set; }

        public Guid CourierGuid { get; set; }

        public virtual Courier Courier { get; set; }

        public Guid OrderGuid { get; set; }

        public virtual Order Order { get; set; }
    }
}
