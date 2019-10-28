using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class Restaurant : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        [MaxLength(50)]
        public string Manufacturer { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<StopSheet> StopSheets { get; set; }
    }
}
