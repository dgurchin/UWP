using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class DeliveryType : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
