using System;

namespace Inventory.Models
{
    public class ModifierModel : BaseModel
    {
        public Guid RowGuid { get; set; }

        public string Name { get; set; }

        public bool IsRequired { get; set; }
    }
}
