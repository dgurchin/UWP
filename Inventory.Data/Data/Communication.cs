using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class Communication : BaseEntity
    {
        public Guid RowGuid { get; set; }

        public Guid CustomerGuid { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        [Required]
        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public virtual CommunicationType Type { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
    }
}
