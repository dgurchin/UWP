using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    /// <summary>
    /// Источник информации
    /// </summary>
    public class Source : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
