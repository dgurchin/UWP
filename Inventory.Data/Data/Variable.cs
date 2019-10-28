using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class Variable : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Data { get; set; }

        public string Description { get; set; }
    }
}
