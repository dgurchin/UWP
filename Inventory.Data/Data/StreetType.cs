using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class StreetType : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string NameShort { get; set; }

        public virtual ICollection<Street> Streets { get; set; }
    }
}
