using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class City : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Street> Streets { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
