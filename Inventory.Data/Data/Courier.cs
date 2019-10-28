using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    /// <summary>
    /// Курьер
    /// </summary>
    public class Courier : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public Guid RowGuid { get; set; }

        public virtual ICollection<CourierOrder> CourierOrder { get; set; }
    }
}
