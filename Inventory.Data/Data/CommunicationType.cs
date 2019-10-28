using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class CommunicationType : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Communication> Communications { get; set; }
    }
}
