using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class OrderType : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
