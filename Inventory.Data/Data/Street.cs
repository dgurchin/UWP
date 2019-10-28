using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class Street : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public int StreetTypeId { get; set; }

        [ForeignKey(nameof(StreetTypeId))]
        public virtual StreetType StreetType { get; set; }

        public int CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
